# /global/variables.tf

# -----------------------------------------------------------------------------
# Global Variables
#
# This file defines the input variables for the global resources stack.
# These variables parameterize the configuration, making it reusable and
# adaptable without changing the core logic in main.tf.
# -----------------------------------------------------------------------------

variable "aws_region" {
  description = "The AWS region where the global resources will be created."
  type        = string
  default     = "us-east-1"
}

variable "project_name" {
  description = "A unique name for the project, used as a prefix for all globally created resources to avoid naming conflicts."
  type        = string
  validation {
    condition     = can(regex("^[a-z0-9-]+$", var.project_name))
    error_message = "Project name must be lowercase alphanumeric with hyphens."
  }
}

variable "global_tags" {
  description = "A map of tags to apply to all resources created in the global stack for identification and cost allocation."
  type        = map(string)
  default = {
    "ManagedBy"   = "Terraform"
    "Project"     = "OPC-Platform"
    "Environment" = "Global"
  }
}

variable "github_organization" {
  description = "The name of the GitHub organization where the source code repository resides."
  type        = string
  sensitive   = false
}

variable "github_repository_name" {
  description = "The name of the GitHub repository for the application, used to scope the IAM role trust policy."
  type        = string
  sensitive   = false
}