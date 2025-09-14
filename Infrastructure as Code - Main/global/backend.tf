# This file configures the remote state backend for the Global resources.
# These resources, like the S3 buckets for Terraform state itself or global
# IAM roles, have their own lifecycle and state, separate from any specific
# application environment like staging or production.

terraform {
  backend "s3" {
    # The name of the S3 bucket where the Terraform state file for global resources is stored.
    # This bucket itself might need to be created manually as a one-time bootstrap process.
    bucket = "industrial-iot-platform-tfstate-global"

    # The path to the state file within the S3 bucket.
    key = "global/terraform.tfstate"

    # The AWS region where the S3 bucket and DynamoDB table are located.
    region = "us-east-1"

    # The name of the DynamoDB table used for state locking for global resources.
    dynamodb_table = "industrial-iot-platform-tfstate-lock-global"

    # Encrypt the state file at rest for an additional layer of security.
    encrypt = true
  }
}