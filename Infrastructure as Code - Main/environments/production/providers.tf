# This file configures the providers used for the Production environment.
# It specifies the AWS region and any other provider-level settings.

provider "aws" {
  # The AWS region where the production infrastructure will be deployed.
  # This is a critical configuration that should be explicitly set for each environment.
  # It is passed via a variable to ensure flexibility and consistency.
  region = var.aws_region

  # It's a best practice to define default tags at the provider level.
  # These tags will be applied to all resources that support them,
  # ensuring consistent metadata for cost allocation, automation, and governance.
  default_tags {
    tags = {
      Environment = "production"
      ManagedBy   = "Terraform"
      Project     = "Industrial-IoT-Platform"
      TerraformWorkspace = terraform.workspace
    }
  }
}