# ---------------------------------------------------------------------------------------------------------------------
# Output Variables for the EKS Cluster Module
# ---------------------------------------------------------------------------------------------------------------------

output "cluster_endpoint" {
  description = "The endpoint for the EKS cluster's Kubernetes API server."
  value       = aws_eks_cluster.main.endpoint
}

output "cluster_ca_certificate" {
  description = "The base64 encoded certificate data required to communicate with the cluster."
  value       = aws_eks_cluster.main.certificate_authority[0].data
}

output "cluster_name" {
  description = "The name of the EKS cluster."
  value       = aws_eks_cluster.main.name
}

output "cluster_security_group_id" {
  description = "The ID of the security group associated with the EKS cluster."
  value       = aws_security_group.cluster.id
}

output "node_group_iam_role_arn" {
  description = "The ARN of the IAM role for the worker nodes."
  value       = aws_iam_role.nodes.arn
}