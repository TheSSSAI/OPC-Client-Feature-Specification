# ---------------------------------------------------------------------------------------------------------------------
# RDS PostgreSQL Module
#
# This module provisions a secure, highly available AWS RDS instance for PostgreSQL,
# suitable for running the TimescaleDB extension.
#
# Requirements Covered:
# - REQ-1-089: Provisions PostgreSQL 16 database.
# - REQ-1-084: Configured for Multi-AZ deployment for high availability.
# - REQ-1-081: Enforces encryption at rest for storage and manages the master password
#              via AWS Secrets Manager.
# - REQ-1-077: Enables Point-in-Time Recovery and daily snapshots via backup configuration.
# ---------------------------------------------------------------------------------------------------------------------

locals {
  db_identifier = var.db_identifier
  tags          = var.tags
}

# ---------------------------------------------------------------------------------------------------------------------
# Database Subnet Group
# Specifies which subnets the RDS instance can be placed in. Should be private subnets.
# ---------------------------------------------------------------------------------------------------------------------
resource "aws_db_subnet_group" "main" {
  name       = "${local.db_identifier}-subnet-group"
  subnet_ids = var.subnet_ids

  tags = merge(local.tags, {
    Name = "${local.db_identifier}-subnet-group"
  })
}

# ---------------------------------------------------------------------------------------------------------------------
# Database Security Group
# Controls network access to the RDS instance.
# ---------------------------------------------------------------------------------------------------------------------
resource "aws_security_group" "db" {
  name        = "${local.db_identifier}-sg"
  description = "Controls access to the ${local.db_identifier} RDS instance"
  vpc_id      = var.vpc_id

  # Allow ingress traffic on the PostgreSQL port from the EKS cluster's security group.
  ingress {
    from_port       = 5432
    to_port         = 5432
    protocol        = "tcp"
    security_groups = [var.eks_cluster_security_group_id]
    description     = "Allow PostgreSQL traffic from EKS cluster"
  }

  # Allow all outbound traffic.
  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }

  tags = merge(local.tags, {
    Name = "${local.db_identifier}-sg"
  })
}

# ---------------------------------------------------------------------------------------------------------------------
# Master Password Management via AWS Secrets Manager (REQ-1-081)
# ---------------------------------------------------------------------------------------------------------------------
resource "aws_secretsmanager_secret" "master_password" {
  name        = "${local.db_identifier}-master-password"
  description = "Master password for the ${local.db_identifier} RDS instance."

  # Generate a random password if one is not provided
  recovery_window_in_days = 0 # Set to 0 to permanently delete secret on destroy.
}

resource "aws_secretsmanager_secret_version" "master_password" {
  secret_id     = aws_secretsmanager_secret.master_password.id
  secret_string = jsonencode({ "username" = var.master_username, "password" = var.master_password })

  lifecycle {
    ignore_changes = [secret_string]
  }
}

# ---------------------------------------------------------------------------------------------------------------------
# RDS Database Instance
# ---------------------------------------------------------------------------------------------------------------------
resource "aws_db_instance" "main" {
  identifier               = local.db_identifier
  engine                   = var.engine
  engine_version           = var.engine_version
  instance_class           = var.instance_class
  allocated_storage        = var.allocated_storage
  max_allocated_storage    = var.max_allocated_storage
  storage_type             = "gp3"
  db_name                  = var.db_name
  username                 = jsondecode(aws_secretsmanager_secret_version.master_password.secret_string)["username"]
  password                 = jsondecode(aws_secretsmanager_secret_version.master_password.secret_string)["password"]
  db_subnet_group_name     = aws_db_subnet_group.main.name
  vpc_security_group_ids   = [aws_security_group.db.id]
  parameter_group_name     = var.parameter_group_name
  skip_final_snapshot      = var.skip_final_snapshot
  publicly_accessible      = false
  deletion_protection      = var.deletion_protection

  # --- Non-Functional Requirements ---
  multi_az                 = var.multi_az      # REQ-1-084: High Availability
  storage_encrypted        = true              # REQ-1-081: Encryption at Rest
  backup_retention_period  = 30                # REQ-1-077: Daily snapshots retained for 30 days
  copy_tags_to_snapshot    = true
  performance_insights_enabled = true

  tags = local.tags
}