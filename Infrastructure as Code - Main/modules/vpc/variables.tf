# ##############################################################################
#
#  VPC Module - Input Variables
#
#  Defines the input interface for the VPC module. These variables allow the
#  VPC to be parameterized and reused across different environments
#  (e.g., staging, production) with different configurations.
#
# ##############################################################################

variable "project_name" {
  description = "The name of the project. Used to prefix resource names and in tags."
  type        = string
  validation {
    condition     = can(regex("^[a-z0-9-]+$", var.project_name))
    error_message = "The project_name must be lowercase and contain only letters, numbers, and hyphens."
  }
}

variable "environment" {
  description = "The deployment environment (e.g., 'staging', 'production'). Used in resource names and tags."
  type        = string
  validation {
    condition     = can(regex("^[a-z]+$", var.environment))
    error_message = "The environment must be lowercase and contain only letters."
  }
}

variable "aws_region" {
  description = "The AWS region to deploy the resources in."
  type        = string
}

variable "vpc_cidr" {
  description = "The IPv4 CIDR block for the VPC."
  type        = string
  validation {
    condition     = can(regex("^([0-9]{1,3}\\.){3}[0-9]{1,3}(\\/([0-9]|[1-2][0-9]))$", var.vpc_cidr))
    error_message = "The vpc_cidr must be a valid IPv4 CIDR block, e.g., '10.0.0.0/16'."
  }
}

variable "availability_zones" {
  description = "A list of Availability Zones to create subnets in. Must provide at least two for high availability."
  type        = list(string)
  validation {
    condition     = length(var.availability_zones) >= 2
    error_message = "At least two Availability Zones are required for a high-availability deployment (REQ-1-084)."
  }
}

variable "private_subnet_cidrs" {
  description = "A list of IPv4 CIDR blocks for the private subnets. The number of CIDRs must match the number of availability zones."
  type        = list(string)
  validation {
    condition = alltrue([
      for cidr in var.private_subnet_cidrs : can(regex("^([0-9]{1,3}\\.){3}[0-9]{1,3}(\\/([0-9]|[1-2][0-9]))$", cidr))
    ])
    error_message = "Each value in private_subnet_cidrs must be a valid IPv4 CIDR block."
  }
}

variable "public_subnet_cidrs" {
  description = "A list of IPv4 CIDR blocks for the public subnets. The number of CIDRs must match the number of availability zones."
  type        = list(string)
  validation {
    condition = alltrue([
      for cidr in var.public_subnet_cidrs : can(regex("^([0-9]{1,3}\\.){3}[0-9]{1,3}(\\/([0-9]|[1-2][0-9]))$", cidr))
    ])
    error_message = "Each value in public_subnet_cidrs must be a valid IPv4 CIDR block."
  }
}