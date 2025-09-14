# This file pins the versions of Terraform and the providers used in this module.
# Pinning versions ensures that the module behaves predictably and prevents breaking
# changes from unexpected provider updates. This is a critical best practice for
# creating reusable and stable Terraform modules.

terraform {
  required_version = "~> 1.9.0"

  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 5.58.0"
    }
  }
}