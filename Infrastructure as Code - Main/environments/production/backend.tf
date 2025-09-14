# This file configures the remote state backend for the Production environment.
# Using a remote backend is essential for collaboration, security, and running
# Terraform in an automated CI/CD pipeline. Each environment MUST have its
# own separate state file to ensure strict isolation.

terraform {
  backend "s3" {
    # The name of the S3 bucket where the Terraform state file will be stored.
    # This bucket must be created beforehand (usually in the 'global' configuration)
    # and have versioning enabled for safety.
    bucket = "industrial-iot-platform-tfstate-prod"

    # The path to the state file within the S3 bucket.
    # This ensures that this environment's state is kept separate from others.
    key = "environments/production/terraform.tfstate"

    # The AWS region where the S3 bucket and DynamoDB table are located.
    # This should be a stable, primary region.
    region = "us-east-1"

    # The name of the DynamoDB table used for state locking.
    # State locking prevents concurrent runs of Terraform from corrupting the state.
    # This table must be created beforehand.
    dynamodb_table = "industrial-iot-platform-tfstate-lock-prod"

    # Encrypt the state file at rest for an additional layer of security.
    encrypt = true
  }
}