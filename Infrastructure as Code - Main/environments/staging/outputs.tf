# outputs.tf for the Staging Environment
# Exposes key infrastructure details for consumption by other systems, such as the CI/CD pipeline.

output "project_name" {
  description = "The name of the project."
  value       = var.project_name
}

output "environment" {
  description = "The environment name."
  value       = var.environment
}

output "aws_region" {
  description = "The AWS region where the infrastructure is deployed."
  value       = var.aws_region
}

output "vpc_id" {
  description = "The ID of the VPC."
  value       = module.vpc.vpc_id
}

output "vpc_cidr_block" {
  description = "The CIDR block of the VPC."
  value       = module.vpc.vpc_cidr_block
}

output "vpc_private_subnets" {
  description = "List of private subnet IDs."
  value       = module.vpc.private_subnets
}

output "vpc_public_subnets" {
  description = "List of public subnet IDs."
  value       = module.vpc.public_subnets
}

output "eks_cluster_name" {
  description = "The name of the EKS cluster."
  value       = module.eks.cluster_name
}

output "eks_cluster_endpoint" {
  description = "The endpoint for the EKS cluster's Kubernetes API server."
  value       = module.eks.cluster_endpoint
}

output "eks_cluster_oidc_issuer_url" {
  description = "The OIDC issuer URL for the EKS cluster, used for IAM Roles for Service Accounts (IRSA)."
  value       = module.eks.cluster_oidc_issuer_url
}

output "rds_postgresql_hostname" {
  description = "The hostname of the RDS PostgreSQL database."
  value       = module.rds_postgresql.db_instance_address
}

output "rds_postgresql_port" {
  description = "The port of the RDS PostgreSQL database."
  value       = module.rds_postgresql.db_instance_port
}

output "rds_postgresql_master_user_secret_arn" {
  description = "The ARN of the AWS Secrets Manager secret containing the master user credentials."
  value       = module.rds_postgresql.db_instance_master_user_secret_arn
  sensitive   = true
}

output "redis_primary_endpoint" {
  description = "The primary endpoint address for the Redis cluster."
  value       = aws_elasticache_cluster.redis.cache_nodes[0].address
}

output "redis_port" {
  description = "The port for the Redis cluster."
  value       = aws_elasticache_cluster.redis.cache_nodes[0].port
}

output "tenant_data_s3_bucket_id" {
  description = "The ID (name) of the S3 bucket for tenant data."
  value       = aws_s3_bucket.tenant_data.id
}

output "audit_verification_qldb_ledger_name" {
  description = "The name of the QLDB ledger for audit verification."
  value       = aws_qldb_ledger.audit_verification.name
}