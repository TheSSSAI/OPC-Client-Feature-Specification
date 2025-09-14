# ---------------------------------------------------------------------------------------------------------------------
# Output Variables for the RDS PostgreSQL Module
# ---------------------------------------------------------------------------------------------------------------------

output "db_instance_address" {
  description = "The address of the RDS instance."
  value       = aws_db_instance.main.address
}

output "db_instance_endpoint" {
  description = "The connection endpoint for the RDS instance."
  value       = aws_db_instance.main.endpoint
}

output "db_instance_port" {
  description = "The port on which the RDS instance is listening."
  value       = aws_db_instance.main.port
}

output "db_instance_name" {
  description = "The name of the database created in the RDS instance."
  value       = aws_db_instance.main.db_name
}

output "db_master_credentials_secret_arn" {
  description = "The ARN of the AWS Secrets Manager secret containing the master credentials."
  value       = aws_secretsmanager_secret.master_password.arn
}

output "db_security_group_id" {
  description = "The ID of the security group created for the RDS instance."
  value       = aws_security_group.db.id
}