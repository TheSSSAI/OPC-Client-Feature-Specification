# .tflint.hcl
# Configuration for the TFLint static analysis tool.
# This file sets up rules and plugins to enforce coding standards and best practices.

config {
  # Fail the linting process if any issue with this severity or higher is found.
  # Valid values are: "error", "warning", "notice", "info", "debug"
  force = false
  
  # Only report issues with this severity or higher.
  # This helps in focusing on critical issues first.
  log_level = "info"

  # A list of directories to ignore during linting.
  # Useful for excluding test fixtures or vendor modules.
  ignore_module = {
    # Example: "terraform-aws-modules/vpc/aws" = true
  }
}

# Enable the AWS ruleset, which is essential for validating AWS resources.
plugin "aws" {
  enabled = true
  version = "0.28.0"
  source  = "github.com/terraform-linters/tflint-ruleset-aws"
}

# Enable the Terraform ruleset for validating Terraform language constructs.
plugin "terraform" {
  enabled = true
  version = "0.5.0"
  source = "github.com/terraform-linters/tflint-ruleset-terraform"
}