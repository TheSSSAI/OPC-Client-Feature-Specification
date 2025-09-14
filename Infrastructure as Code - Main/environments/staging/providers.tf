# This file configures the providers used for the Staging environment.
# It specifies the AWS region and any other provider-level settings.

provider "aws" {
  # The AWS region where the staging infrastructure will be deployed.
  # This should be the same as production to ensure environment parity,
  # unless a different region is used for specific testing purposes.
  region = var.aws_region

  # It's a best practice to define default tags at the provider level.
  # These tags will be applied to all resources that support them,
  # ensuring consistent metadata for cost allocation, automation, and governance.
  default_tags {
    tags = {
      Environment = "staging"
      ManagedBy   = "Terraform"
      Project     = "Industrial-IoT-Platform"
      TerraformWorkspace = terraform.workspace
    }
  }
}