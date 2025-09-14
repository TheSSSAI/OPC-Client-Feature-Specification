# staging.tfvars
# Contains the specific variable values for the STAGING environment.
# This environment is a scaled-down version of production for cost-efficiency.

project_name = "opc-ua-management-platform"
aws_region   = "us-east-2"

# VPC Configuration - smaller CIDR for staging
vpc_cidr                 = "10.20.0.0/16"
vpc_availability_zones_count = 2
vpc_private_subnets      = ["10.20.1.0/24", "10.20.2.0/24"]
vpc_public_subnets       = ["10.20.101.0/24", "10.20.102.0/24"]

# EKS Cluster Configuration - Sized for staging/testing load
eks_cluster_version             = "1.29"
eks_general_node_instance_types = ["t3.medium"]
eks_general_node_min_size       = 2
eks_general_node_max_size       = 4
eks_general_node_desired_size   = 2

# RDS PostgreSQL Configuration - Smaller instance for cost savings
rds_postgres_engine_version    = "16.2"
rds_postgres_instance_class    = "db.t3.medium"
rds_postgres_allocated_storage = 50 # In GB
rds_postgres_username          = "opcadmin_stage"
rds_postgres_db_name           = "opc_management_stage"

# ElastiCache Redis Configuration
redis_engine_version    = "7.1"
redis_node_type         = "cache.t3.small"
redis_num_cache_nodes   = 1