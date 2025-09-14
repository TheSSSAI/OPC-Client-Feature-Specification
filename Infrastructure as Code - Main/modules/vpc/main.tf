# ##############################################################################
#
#  VPC Module - Main Configuration
#
#  Provisions the core networking infrastructure for an environment.
#  This module uses the official AWS VPC module to ensure best practices
#  are followed for creating a Virtual Private Cloud (VPC), including
#  public and private subnets across multiple Availability Zones,
#  Internet Gateways, and highly available NAT Gateways.
#
#  This configuration is designed to fulfill the following requirements:
#  - REQ-1-089: Infrastructure managed as code using Terraform.
#  - REQ-1-084: High availability through multi-AZ subnet deployment.
#
# ##############################################################################

# This module encapsulates the creation of a Virtual Private Cloud (VPC),
# subnets, an internet gateway, route tables, and NAT gateways.
# By using a well-vetted community module, we inherit best practices for
# security, availability, and scalability.
module "vpc" {
  source  = "terraform-aws-modules/vpc/aws"
  version = "5.9.0"

  # ----------------------------------------------------------------------------
  # Basic VPC Configuration
  # ----------------------------------------------------------------------------
  name = "${var.project_name}-${var.environment}-vpc"
  cidr = var.vpc_cidr

  # Availability Zones to operate in. The list is passed in from the root module,
  # ensuring that all resources are deployed in a multi-AZ fashion for high availability.
  azs = var.availability_zones

  # ----------------------------------------------------------------------------
  # Subnet Configuration
  #
  # We create both public and private subnets in each specified Availability Zone.
  # - Public subnets are for resources that need to be directly accessible from the internet,
  #   such as load balancers and bastion hosts.
  # - Private subnets are for backend resources like EKS worker nodes and RDS databases,
  #   which should not be exposed to the internet directly.
  # ----------------------------------------------------------------------------
  private_subnets = var.private_subnet_cidrs
  public_subnets  = var.public_subnet_cidrs

  # ----------------------------------------------------------------------------
  # NAT Gateway Configuration
  #
  # To enable resources in private subnets to initiate outbound connections to
  # the internet (e.g., for pulling container images or OS updates) while
  # remaining inaccessible from the outside, we provision NAT Gateways.
  # Setting this to `true` provisions one NAT Gateway per Availability Zone,
  # which is critical for high availability (REQ-1-084).
  # ----------------------------------------------------------------------------
  enable_nat_gateway = true
  single_nat_gateway = false
  one_nat_gateway_per_az = true

  # ----------------------------------------------------------------------------
  # DNS and Other VPC Settings
  #
  # These are standard best practices for AWS VPCs, enabling DNS resolution
  # and hostname assignment for resources within the VPC.
  # ----------------------------------------------------------------------------
  enable_dns_hostnames = true
  enable_dns_support   = true

  # ----------------------------------------------------------------------------
  # Tagging Strategy
  #
  # A consistent tagging strategy is crucial for cost allocation, automation,
  # and resource management. We apply a common set of tags to the VPC and all
  # sub-resources created by this module.
  # ----------------------------------------------------------------------------
  tags = {
    "Terraform"   = "true"
    "Project"     = var.project_name
    "Environment" = var.environment
  }

  vpc_tags = {
    "Name" = "${var.project_name}-${var.environment}-vpc"
  }

  public_subnet_tags = {
    "Type"                                = "public-subnets"
    "kubernetes.io/role/elb"              = "1"
    "kubernetes.io/cluster/${var.project_name}-${var.environment}" = "shared"
  }

  private_subnet_tags = {
    "Type"                                = "private-subnets"
    "kubernetes.io/role/internal-elb"     = "1"
    "kubernetes.io/cluster/${var.project_name}-${var.environment}" = "shared"
  }
}