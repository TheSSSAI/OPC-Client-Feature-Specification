# main.tf for the Staging Environment
# This file composes reusable infrastructure modules to build a complete, non-production
# environment for testing, UAT, and pre-release validation.

# ------------------------------------------------------------------------------
# DATA SOURCES
# ------------------------------------------------------------------------------

data "aws_availability_zones" "available" {
  state = "available"
}

data "aws_caller_identity" "current" {}

# ------------------------------------------------------------------------------
# NETWORKING - VPC
# ------------------------------------------------------------------------------

module "vpc" {
  source = "../../modules/vpc"

  name = "${var.project_name}-${var.environment}"
  cidr = var.vpc_cidr

  azs             = slice(data.aws_availability_zones.available.names, 0, var.vpc_availability_zones_count)
  private_subnets = var.vpc_private_subnets
  public_subnets  = var.vpc_public_subnets

  enable_nat_gateway   = true
  single_nat_gateway   = true # Cost optimization for staging
  enable_dns_hostnames = true

  tags = local.tags
}

# ------------------------------------------------------------------------------
# KUBERNETES - EKS CLUSTER
# ------------------------------------------------------------------------------

module "eks" {
  source = "../../modules/eks"

  cluster_name    = "${var.project_name}-${var.environment}-cluster"
  cluster_version = var.eks_cluster_version
  vpc_id          = module.vpc.vpc_id
  subnet_ids      = module.vpc.private_subnets

  eks_managed_node_groups = {
    general_purpose = {
      instance_types = var.eks_general_node_instance_types
      min_size       = var.eks_general_node_min_size
      max_size       = var.eks_general_node_max_size
      desired_size   = var.eks_general_node_desired_size
      disk_size      = 20
    }
  }

  tags = local.tags
}

# ------------------------------------------------------------------------------
# DATA STORES - RDS, ElastiCache, S3, QLDB
# ------------------------------------------------------------------------------

# RDS for PostgreSQL (Configuration & Relational Data) + TimescaleDB
module "rds_postgresql" {
  source = "../../modules/rds"

  identifier           = "${var.project_name}-${var.environment}-postgres"
  engine               = "postgres"
  engine_version       = var.rds_postgres_engine_version
  instance_class       = var.rds_postgres_instance_class
  allocated_storage    = var.rds_postgres_allocated_storage
  storage_type         = "gp3"
  username             = var.rds_postgres_username
  db_name              = var.rds_postgres_db_name
  multi_az             = false # Cost optimization for staging
  storage_encrypted    = true  # Maintain security parity
  skip_final_snapshot  = true
  deletion_protection  = false
  backup_retention_period = 7

  vpc_id               = module.vpc.vpc_id
  db_subnet_group_name = module.vpc.database_subnet_group_name
  vpc_security_group_ids = [
    aws_security_group.rds_sg.id
  ]

  manage_master_user_password = true
  master_user_password        = null
  master_user_secret_kms_key_id = aws_kms_key.rds_secrets_key.id

  tags = local.tags
}

# ElastiCache for Redis (Caching)
resource "aws_elasticache_subnet_group" "redis" {
  name       = "${var.project_name}-${var.environment}-redis-subnet-group"
  subnet_ids = module.vpc.private_subnets
  tags       = local.tags
}

resource "aws_elasticache_cluster" "redis" {
  cluster_id           = "${var.project_name}-${var.environment}-redis"
  engine               = "redis"
  engine_version       = var.redis_engine_version
  node_type            = var.redis_node_type
  num_cache_nodes      = var.redis_num_cache_nodes
  parameter_group_name = "default.redis7"
  subnet_group_name    = aws_elasticache_subnet_group.redis.name
  security_group_ids = [
    aws_security_group.redis_sg.id
  ]
  apply_immediately    = true
  tags                 = local.tags
}

# S3 Bucket for Tenant Data
resource "aws_s3_bucket" "tenant_data" {
  bucket = "${var.project_name}-${var.environment}-tenant-data-${data.aws_caller_identity.current.account_id}"
  tags   = local.tags
}

resource "aws_s3_bucket_server_side_encryption_configuration" "tenant_data_encryption" {
  bucket = aws_s3_bucket.tenant_data.id
  rule {
    apply_server_side_encryption_by_default {
      sse_algorithm = "AES256"
    }
  }
}

resource "aws_s3_bucket_public_access_block" "tenant_data_public_access" {
  bucket                  = aws_s3_bucket.tenant_data.id
  block_public_acls       = true
  block_public_policy     = true
  ignore_public_acls      = true
  restrict_public_buckets = true
}

resource "aws_s3_bucket_versioning" "tenant_data_versioning" {
  bucket = aws_s3_bucket.tenant_data.id
  versioning_configuration {
    status = "Enabled"
  }
}

# QLDB Ledger for Audit Trail Verification
resource "aws_qldb_ledger" "audit_verification" {
  name               = "${var.project_name}-${var.environment}-audit-verification-ledger"
  permissions_mode   = "STANDARD"
  deletion_protection = false # Can be disabled for staging to allow easier teardown

  tags = local.tags
}


# ------------------------------------------------------------------------------
# SECURITY & IAM
# ------------------------------------------------------------------------------

resource "aws_kms_key" "rds_secrets_key" {
  description             = "KMS key for encrypting RDS master password in Secrets Manager for Staging"
  deletion_window_in_days = 7
  enable_key_rotation     = true
  tags                    = local.tags
}

resource "aws_security_group" "rds_sg" {
  name        = "${var.project_name}-${var.environment}-rds-sg"
  description = "Allow traffic from EKS worker nodes to RDS"
  vpc_id      = module.vpc.vpc_id
  tags        = local.tags
}

resource "aws_security_group_rule" "rds_ingress_from_eks" {
  type                     = "ingress"
  from_port                = 5432
  to_port                  = 5432
  protocol                 = "tcp"
  security_group_id        = aws_security_group.rds_sg.id
  source_security_group_id = module.eks.node_security_group_id
  description              = "Allow PostgreSQL traffic from EKS nodes"
}

resource "aws_security_group" "redis_sg" {
  name        = "${var.project_name}-${var.environment}-redis-sg"
  description = "Allow traffic from EKS worker nodes to Redis"
  vpc_id      = module.vpc.vpc_id
  tags        = local.tags
}

resource "aws_security_group_rule" "redis_ingress_from_eks" {
  type                     = "ingress"
  from_port                = 6379
  to_port                  = 6379
  protocol                 = "tcp"
  security_group_id        = aws_security_group.redis_sg.id
  source_security_group_id = module.eks.node_security_group_id
  description              = "Allow Redis traffic from EKS nodes"
}

# ------------------------------------------------------------------------------
# LOCALS
# ------------------------------------------------------------------------------

locals {
  tags = {
    Project     = var.project_name
    Environment = var.environment
    ManagedBy   = "Terraform"
  }
}