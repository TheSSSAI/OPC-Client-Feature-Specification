# This test file validates the EKS module.
# It first provisions a prerequisite VPC, then provisions the EKS cluster
# within that VPC. Finally, it asserts that the EKS cluster outputs are correct,
# ensuring the module functions as expected.

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

# Test case: eks_cluster_creation
# This run block defines a test case for the EKS module. It includes creating
# the necessary VPC dependency before instantiating the EKS module.
run "eks_cluster_creation" {
  command = apply

  # This block defines a prerequisite module needed for the test.
  # Here, we create a VPC for the EKS cluster to reside in.
  module "vpc" {
    source = "../vpc"

    name = "eks-test-vpc-${random_string.suffix.result}"
    cidr = "10.2.0.0/16"
    azs  = ["${var.aws_region}a", "${var.aws_region}b"]

    public_subnets = [
      "10.2.1.0/24",
      "10.2.2.0/24",
    ]
    private_subnets = [
      "10.2.101.0/24",
      "10.2.102.0/24",
    ]

    enable_nat_gateway = true
    tags = {
      TerraformTest = "true"
      TestFor       = "EKS-Module"
    }
  }

  # This is the primary module being tested in this file.
  # Note how it consumes outputs from the prerequisite `vpc` module.
  variables {
    cluster_name    = "test-eks-cluster-${random_string.suffix.result}"
    cluster_version = "1.29"

    vpc_id             = module.vpc.vpc_id
    private_subnet_ids = module.vpc.private_subnet_ids

    instance_types = ["t3.medium"]
    min_size       = 1
    max_size       = 2
    desired_size   = 1

    tags = {
      TerraformTest = "true"
      Environment   = "test"
    }
  }

  # Assertions to validate the EKS module's outputs after creation.
  assert {
    condition     = module.eks.cluster_endpoint != null
    error_message = "EKS cluster endpoint should not be null."
  }

  assert {
    condition     = module.eks.cluster_name == "test-eks-cluster-${random_string.suffix.result}"
    error_message = "Cluster name output must match the input variable."
  }

  assert {
    condition     = module.eks.cluster_oidc_issuer_url != null
    error_message = "EKS cluster OIDC issuer URL should not be null."
  }

  assert {
    condition     = module.eks.cluster_security_group_id != null
    error_message = "Cluster security group ID should not be null."
  }

  assert {
    condition     = length(module.eks.node_group_arns) > 0
    error_message = "At least one node group ARN should be created and outputted."
  }
}