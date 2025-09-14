# This test file validates the functionality of the VPC module.
# It provisions a VPC with a specified configuration and then asserts
# that the created resources and outputs match the expected values.
# This ensures the module correctly creates the networking foundation.

# Define required providers for the test.
# The AWS provider is needed to provision resources.
# The random provider can be used for generating unique names.
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

# Define a variable for the AWS region to make the test configurable.
variable "aws_region" {
  type        = string
  description = "AWS region for the test resources."
  default     = "us-east-1"
}

# Configure the AWS provider for the test.
provider "aws" {
  region = var.aws_region
}

# Generate a random suffix for resource names to ensure uniqueness
# across concurrent test runs.
resource "random_string" "suffix" {
  length  = 8
  special = false
  upper   = false
}

# Test case: vpc_creation_and_outputs
# This run block defines a single test case that creates a VPC using the module
# and verifies its outputs.
run "vpc_creation_and_outputs" {
  # The command variable specifies which Terraform command to run.
  # For module tests, this is always `apply`.
  command = apply

  # Define variables to pass to the module under test.
  variables {
    name = "test-vpc-${random_string.suffix.result}"
    cidr = "10.1.0.0/16"
    azs  = ["${var.aws_region}a", "${var.aws_region}b"]
    public_subnets = [
      "10.1.1.0/24",
      "10.1.2.0/24"
    ]
    private_subnets = [
      "10.1.101.0/24",
      "10.1.102.0/24"
    ]
    tags = {
      TerraformTest = "true"
      Environment   = "test"
      Owner         = "automation"
    }
  }

  # Assertions to validate the module's behavior and outputs.
  # These checks run after the `terraform apply` is successful.
  assert {
    condition     = module.vpc.vpc_id != null && substr(module.vpc.vpc_id, 0, 4) == "vpc-"
    error_message = "VPC ID should not be null and should have the 'vpc-' prefix."
  }

  assert {
    condition     = length(module.vpc.public_subnet_ids) == 2
    error_message = "The number of public subnets created should match the number of AZs provided (2)."
  }

  assert {
    condition     = length(module.vpc.private_subnet_ids) == 2
    error_message = "The number of private subnets created should match the number of AZs provided (2)."
  }

  assert {
    condition     = module.vpc.vpc_cidr_block == "10.1.0.0/16"
    error_message = "The VPC CIDR block output should match the input variable."
  }

  assert {
    condition     = module.vpc.database_subnet_group_name != null
    error_message = "Database subnet group name should be outputted and not be null."
  }

  assert {
    condition     = module.vpc.nat_public_ips != null && length(module.vpc.nat_public_ips) > 0
    error_message = "NAT gateway public IPs should be outputted for private subnet internet access."
  }
}