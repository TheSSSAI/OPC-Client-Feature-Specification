# production.tfvars
# Contains the specific variable values for the PRODUCTION environment.
# WARNING: Do not commit sensitive data like passwords to this file.
# Use environment variables or a secure secret store in your CI/CD pipeline.

project_name = "opc-ua-management-platform"
aws_region   = "us-east-1"

# VPC Configuration - Class A network for ample IP space
vpc_cidr                 = "10.10.0.0/16"
vpc_availability_zones_count = 3
vpc_private_subnets      = ["10.10.1.0/24", "10.10.2.0/24", "10.10.3.0/24"]
vpc_public_subnets       = ["10.10.101.0/24", "10.10.102.0/24", "10.10.103.0/24"]

# EKS Cluster Configuration - Sized for production load with auto-scaling
eks_cluster_version             = "1.29"
eks_general_node_instance_types = ["m5.large", "m5a.large", "m6i.large"]
eks_general_node_min_size       = 3
eks_general_node_max_size       = 10
eks_general_node_desired_size   = 3

eks_memory_node_instance_types = ["r5.large", "r6i.large"]
eks_memory_node_min_size       = 2
eks_memory_node_max_size       = 8
eks_memory_node_desired_size   = 2

# RDS PostgreSQL Configuration - Production-grade instance with sufficient storage
rds_postgres_engine_version    = "16.2"
rds_postgres_instance_class    = "db.m5.large"
rds_postgres_allocated_storage = 200 # In GB
rds_postgres_username          = "opcadmin"
rds_postgres_db_name           = "opc_management_prod"

# ElastiCache Redis Configuration
redis_engine_version    = "7.1"
redis_node_type         = "cache.t4g.medium"
redis_num_cache_nodes   = 2 # For production, use a replication group with multi-AZ