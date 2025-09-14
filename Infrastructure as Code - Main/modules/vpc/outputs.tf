# ##############################################################################
#
#  VPC Module - Outputs
#
#  Defines the outputs of the VPC module. These values are exposed so they
#  can be used by other modules (e.g., EKS, RDS) to create resources
#  within this VPC. This forms the public contract of the module.
#
# ##############################################################################

output "vpc_id" {
  description = "The ID of the created VPC."
  value       = module.vpc.vpc_id
}

output "vpc_cidr_block" {
  description = "The primary IPv4 CIDR block of the VPC."
  value       = module.vpc.vpc_cidr_block
}

output "private_subnets" {
  description = "A list of IDs of the private subnets."
  value       = module.vpc.private_subnets
}

output "public_subnets" {
  description = "A list of IDs of the public subnets."
  value       = module.vpc.public_subnets
}

output "nat_gateway_public_ips" {
  description = "A list of public IP addresses assigned to the NAT Gateways. Useful for whitelisting."
  value       = module.vpc.nat_public_ips
}

output "database_subnet_group_name" {
  description = "The name of the DB subnet group created in the private subnets. To be used by the RDS module."
  value       = module.vpc.database_subnet_group_name
}

output "default_security_group_id" {
  description = "The ID of the default security group for the VPC."
  value       = module.vpc.default_security_group_id
}