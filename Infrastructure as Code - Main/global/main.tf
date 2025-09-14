# /global/main.tf

# -----------------------------------------------------------------------------
# Global Resources Configuration
#
# This file defines foundational AWS resources that are shared across all
# environments (staging, production). These resources typically have a longer
# lifecycle and are prerequisites for deploying environment-specific stacks.
# This includes:
#   - S3 Bucket for storing Terraform remote state.
#   - DynamoDB Table for Terraform state locking.
#   - IAM Role and OIDC provider for secure CI/CD with GitHub Actions.
#
# These resources are provisioned once and managed separately from the
# application environments to ensure stability and security.
# -----------------------------------------------------------------------------

# -----------------------------------------------------------------------------
# Terraform Remote State Resources
#
# These resources provide a secure and reliable backend for the staging and
# production environments' Terraform state files. Using a remote backend is
# crucial for team collaboration and CI/CD automation.
# -----------------------------------------------------------------------------

resource "aws_s3_bucket" "terraform_state" {
  bucket = "${var.project_name}-terraform-state-storage"

  tags = merge(var.global_tags, {
    Name = "${var.project_name}-terraform-state-storage"
  })
}

resource "aws_s3_bucket_versioning" "terraform_state" {
  bucket = aws_s3_bucket.terraform_state.id
  versioning_configuration {
    status = "Enabled"
  }
}

resource "aws_s3_bucket_server_side_encryption_configuration" "terraform_state" {
  bucket = aws_s3_bucket.terraform_state.id

  rule {
    apply_server_side_encryption_by_default {
      sse_algorithm = "AES256"
    }
  }
}

resource "aws_s3_bucket_public_access_block" "terraform_state" {
  bucket = aws_s3_bucket.terraform_state.id

  block_public_acls       = true
  block_public_policy     = true
  ignore_public_acls      = true
  restrict_public_buckets = true
}

resource "aws_dynamodb_table" "terraform_state_lock" {
  name         = "${var.project_name}-terraform-state-lock"
  billing_mode = "PAY_PER_REQUEST"
  hash_key     = "LockID"

  attribute {
    name = "LockID"
    type = "S"
  }

  tags = merge(var.global_tags, {
    Name = "${var.project_name}-terraform-state-lock"
  })
}

# -----------------------------------------------------------------------------
# CI/CD Integration Resources (GitHub Actions)
#
# These resources set up a secure, keyless authentication mechanism for the
# GitHub Actions CI/CD pipeline, following the principle of least privilege.
# An IAM OIDC provider establishes trust with GitHub, and a dedicated IAM role
# is created for the pipeline to assume.
#
# This approach avoids storing long-lived IAM user credentials as secrets in
# GitHub.
# -----------------------------------------------------------------------------

resource "aws_iam_oidc_provider" "github" {
  # This URL is standard for GitHub Actions OIDC.
  url = "https://token.actions.githubusercontent.com"

  client_id_list = [
    "sts.amazonaws.com"
  ]

  # The thumbprint is for the root CA of the OIDC provider.
  # This value is stable but should be periodically verified against GitHub's documentation.
  # As of late 2023, this is the standard thumbprint.
  thumbprint_list = ["6938fd4d98bab03faadb97b34396831e3780aea1"]

  tags = merge(var.global_tags, {
    Name = "${var.project_name}-github-oidc-provider"
  })
}

resource "aws_iam_role" "cicd_execution_role" {
  name               = "${var.project_name}-cicd-execution-role"
  assume_role_policy = data.aws_iam_policy_document.cicd_assume_role_policy.json

  description = "IAM role to be assumed by GitHub Actions for deploying infrastructure."

  tags = merge(var.global_tags, {
    Name = "${var.project_name}-cicd-execution-role"
  })
}

data "aws_iam_policy_document" "cicd_assume_role_policy" {
  statement {
    effect  = "Allow"
    actions = ["sts:AssumeRoleWithWebIdentity"]

    principals {
      type        = "Federated"
      identifiers = [aws_iam_oidc_provider.github.arn]
    }

    # Condition restricts which GitHub repository can assume this role.
    # This is a critical security control.
    condition {
      test     = "StringLike"
      variable = "token.actions.githubusercontent.com:sub"
      values   = ["repo:${var.github_organization}/${var.github_repository_name}:*"]
    }
  }
}

resource "aws_iam_policy" "cicd_permissions_policy" {
  name        = "${var.project_name}-cicd-permissions-policy"
  description = "Policy granting permissions needed for the CI/CD pipeline to manage application infrastructure."
  policy      = data.aws_iam_policy_document.cicd_permissions_policy.json

  tags = merge(var.global_tags, {
    Name = "${var.project_name}-cicd-permissions-policy"
  })
}

data "aws_iam_policy_document" "cicd_permissions_policy" {
  # This policy is intentionally broad for the initial setup.
  # In a mature production environment, this should be scoped down
  # to the exact permissions required.
  statement {
    sid    = "AllowFullInfrastructureManagement"
    effect = "Allow"
    actions = [
      "ec2:*",
      "eks:*",
      "rds:*",
      "s3:*",
      "iam:*",          # Required for creating roles, instance profiles, etc.
      "elasticache:*",
      "qldb:*",
      "cloudwatch:*",   # For creating log groups, alarms, etc.
      "kms:*",          # For managing encryption keys.
      "secretsmanager:*",
      "autoscaling:*",
      "elasticloadbalancing:*"
    ]
    resources = ["*"] # WARNING: In production, scope this to specific resources or regions.
  }

  statement {
    sid    = "AllowPassRole"
    effect = "Allow"
    actions = [
      "iam:PassRole"
    ]
    # Restrict PassRole to prevent privilege escalation.
    # Allows the pipeline to pass roles to services like EKS nodes.
    resources = [
      "arn:aws:iam::${data.aws_caller_identity.current.account_id}:role/${var.project_name}-*"
    ]
  }
}

resource "aws_iam_role_policy_attachment" "cicd_permissions_attachment" {
  role       = aws_iam_role.cicd_execution_role.name
  policy_arn = aws_iam_policy.cicd_permissions_policy.arn
}

data "aws_caller_identity" "current" {}