# ---------------------------------------------------------------------------------------------------------------------
# Input Variables for the RDS PostgreSQL Module
# ---------------------------------------------------------------------------------------------------------------------

variable "db_identifier" {
  description = "The unique identifier for the RDS instance."
  type        = string
}

variable "db_name" {
  description = "The name of the initial database to create in the RDS instance."
  type        = string
}

variable "engine" {
  description = "The database engine to use."
  type        = string
  default     = "postgres"
}

variable "engine_version" {
  description = "The version of the PostgreSQL database engine to use. (REQ-1-089)"
  type        = string
  default     = "16" # Corresponds to PostgreSQL 16
}

variable "instance_class" {
  description = "The instance type for the RDS database (e.g., 'db.t3.medium')."
  type        = string
}

variable "allocated_storage" {
  description = "The initial allocated storage in GiB."
  type        = number
  default     = 100
}

variable "max_allocated_storage" {
  description = "The maximum storage to allow for autogrowing. Set to 0 to disable autogrow."
  type        = number
  default     = 200
}

variable "vpc_id" {
  description = "The ID of the VPC where the RDS instance will be deployed."
  type        = string
}

variable "subnet_ids" {
  description = "A list of private subnet IDs for the RDS instance. Must be in at least two AZs for Multi-AZ."
  type        = list(string)
}

variable "eks_cluster_security_group_id" {
  description = "The ID of the EKS cluster security group to allow inbound traffic from."
  type        = string
}

variable "master_username" {
  description = "The master username for the database."
  type        = string
  default     = "masteruser"
}

variable "master_password" {
  description = "The master password for the database. Will be stored in AWS Secrets Manager. (REQ-1-081)"
  type        = string
  sensitive   = true
}

variable "parameter_group_name" {
  description = "Name of the DB parameter group to associate. Can be used to enable TimescaleDB extension."
  type        = string
  default     = null
}

variable "multi_az" {
  description = "Specifies if the RDS instance is multi-AZ for high availability. (REQ-1-084)"
  type        = bool
  default     = true
}

variable "deletion_protection" {
  description = "If the DB instance should have deletion protection enabled. The database can't be deleted when this value is set to true."
  type        = bool
  default     = true
}

variable "skip_final_snapshot" {
  description = "Determines whether a final DB snapshot is created before the DB instance is deleted."
  type        = bool
  default     = false
}

variable "tags" {
  description = "A map of tags to assign to all created resources."
  type        = map(string)
  default     = {}
}