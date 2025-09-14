# This test file validates the RDS module.
# It provisions a VPC as a prerequisite, then deploys an RDS instance
# into the private subnets of that VPC. It asserts key outputs and validates
# that security and high-availability settings are correctly applied.

terraform {
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 5.58.0"
    }
    random = {
      source  = "hashicorp/random"
      version = "~> 3.5.1"
    }
  }
}

variable "aws_region" {
  type        = string
  description = "AWS region for the test resources."
  default     = "us-east-1"
}

provider "aws" {
  region = var.aws_region
}

resource "random_string" "suffix" {
  length  = 8
  special = false
  upper   = false
}

# Generate a random password for the test database instance.
# This avoids hardcoding secrets in test code, which is a security best practice.
resource "random_password" "db_password" {
  length  = 16
  special = true
}

# Test case: rds_instance_creation_and_validation
# This run block creates a VPC dependency and then the RDS instance,
# followed by assertions to validate its configuration.
run "rds_instance_creation_and_validation" {
  command = apply

  # Define the prerequisite VPC module for networking.
  module "vpc" {
    source = "../vpc"

    name = "rds-test-vpc-${random_string.suffix.result}"
    cidr = "10.3.0.0/16"
    azs  = ["${var.aws_region}a", "${var.aws_region}b"]

    public_subnets = [
      "10.3.1.0/24",
      "10.3.2.0/24",
    ]
    private_subnets = [
      "10.3.101.0/24",
      "10.3.102.0/24",
    ]
    
    enable_nat_gateway = false
    tags = {
      TerraformTest = "true"
      TestFor       = "RDS-Module"
    }
  }

  # This is the primary module being tested.
  variables {
    identifier_prefix = "test-rds-db-${random_string.suffix.result}"
    engine            = "postgres"
    engine_version    = "16.2"
    instance_class    = "db.t3.micro"
    allocated_storage = 20

    db_name  = "testdb"
    username = "testuser"
    password = random_password.db_password.result

    vpc_id             = module.vpc.vpc_id
    db_subnet_group_name = module.vpc.database_subnet_group_name
    vpc_security_group_ids = [module.vpc.default_security_group_id]
    
    # These validate NFRs REQ-1-084 and REQ-1-081
    multi_az          = true
    storage_encrypted = true

    tags = {
      TerraformTest = "true"
      Environment   = "test"
    }
  }

  # Assertions to validate the RDS module's outputs and configuration.
  assert {
    condition     = module.rds.db_instance_arn != null
    error_message = "RDS instance ARN should not be null."
  }

  assert {
    condition     = module.rds.db_instance_endpoint != null
    error_message = "RDS instance endpoint should not be null."
  }

  assert {
    condition     = module.rds.db_instance_username == "testuser"
    error_message = "Database username output should match the input."
  }

  assert {
    condition     = module.rds.db_instance_port == 5432
    error_message = "Database port for PostgreSQL should be 5432."
  }

  # This is a critical check to ensure the module adheres to security policies.
  assert {
    condition     = module.rds.db_instance_storage_encrypted == true
    error_message = "Storage encryption must be enabled as per security requirements (REQ-1-081)."
  }

  # This is a critical check to ensure the module adheres to high-availability policies.
  assert {
    condition     = module.rds.db_instance_multi_az == true
    error_message = "Multi-AZ deployment must be enabled as per availability requirements (REQ-1-084)."
  }
}