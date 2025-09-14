# ---------------------------------------------------------------------------------------------------------------------
# EKS Cluster Module
#
# This module provisions a secure and scalable Amazon EKS cluster, including the control plane,
# IAM roles, and a default managed node group. It is designed to be highly available by
# deploying nodes across multiple specified subnets (Availability Zones).
#
# Requirements Covered:
# - REQ-1-021: Deploys on Amazon Elastic Kubernetes Service (EKS).
# - REQ-1-084: Ensures high availability by spanning multiple AZs via subnet configuration.
# - REQ-1-081: Enforces encryption at rest for node group EBS volumes.
# - REQ-1-085: Configures node group autoscaling to support horizontal pod autoscalers.
# ---------------------------------------------------------------------------------------------------------------------

locals {
  cluster_name = var.cluster_name
  tags         = var.tags
}

# ---------------------------------------------------------------------------------------------------------------------
# IAM Role for EKS Cluster Control Plane
# ---------------------------------------------------------------------------------------------------------------------
resource "aws_iam_role" "cluster" {
  name = "${local.cluster_name}-cluster-role"

  assume_role_policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Action = "sts:AssumeRole"
        Effect = "Allow"
        Principal = {
          Service = "eks.amazonaws.com"
        }
      }
    ]
  })

  tags = local.tags
}

resource "aws_iam_role_policy_attachment" "cluster_policy" {
  policy_arn = "arn:aws:iam::aws:policy/AmazonEKSClusterPolicy"
  role       = aws_iam_role.cluster.name
}

# ---------------------------------------------------------------------------------------------------------------------
# Security Group for the EKS Cluster Control Plane
# This controls communication between the control plane and the nodes.
# ---------------------------------------------------------------------------------------------------------------------
resource "aws_security_group" "cluster" {
  name        = "${local.cluster_name}-cluster-sg"
  description = "Security group for EKS cluster control plane"
  vpc_id      = var.vpc_id

  tags = merge(local.tags, {
    Name = "${local.cluster_name}-cluster-sg"
  })
}

# ---------------------------------------------------------------------------------------------------------------------
# EKS Cluster Resource
# ---------------------------------------------------------------------------------------------------------------------
resource "aws_eks_cluster" "main" {
  name     = local.cluster_name
  role_arn = aws_iam_role.cluster.arn
  version  = var.cluster_version

  vpc_config {
    subnet_ids              = var.subnet_ids
    security_group_ids      = [aws_security_group.cluster.id]
    endpoint_private_access = true
    endpoint_public_access  = true # Can be restricted if a bastion host is used
  }

  enabled_cluster_log_types = ["api", "audit", "authenticator", "controllerManager", "scheduler"]

  depends_on = [
    aws_iam_role_policy_attachment.cluster_policy,
  ]

  tags = local.tags
}

# ---------------------------------------------------------------------------------------------------------------------
# IAM Role for EKS Worker Nodes
# ---------------------------------------------------------------------------------------------------------------------
resource "aws_iam_role" "nodes" {
  name = "${local.cluster_name}-node-group-role"

  assume_role_policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Action = "sts:AssumeRole"
        Effect = "Allow"
        Principal = {
          Service = "ec2.amazonaws.com"
        }
      }
    ]
  })

  tags = local.tags
}

resource "aws_iam_role_policy_attachment" "nodes_worker_policy" {
  policy_arn = "arn:aws:iam::aws:policy/AmazonEKSWorkerNodePolicy"
  role       = aws_iam_role.nodes.name
}

resource "aws_iam_role_policy_attachment" "nodes_cni_policy" {
  policy_arn = "arn:aws:iam::aws:policy/AmazonEKS_CNI_Policy"
  role       = aws_iam_role.nodes.name
}

resource "aws_iam_role_policy_attachment" "nodes_ecr_readonly_policy" {
  policy_arn = "arn:aws:iam::aws:policy/AmazonEC2ContainerRegistryReadOnly"
  role       = aws_iam_role.nodes.name
}


# ---------------------------------------------------------------------------------------------------------------------
# EKS Managed Node Group
# Deploys EC2 instances that will register with the EKS cluster as worker nodes.
# ---------------------------------------------------------------------------------------------------------------------
resource "aws_eks_node_group" "main" {
  cluster_name    = aws_eks_cluster.main.name
  node_group_name = var.node_group_name
  node_role_arn   = aws_iam_role.nodes.arn
  subnet_ids      = var.subnet_ids
  instance_types  = var.instance_types
  ami_type        = "AL2_x86_64"
  disk_size       = var.node_disk_size

  scaling_config {
    desired_size = var.scaling_desired_size
    max_size     = var.scaling_max_size
    min_size     = var.scaling_min_size
  }

  # Enforces encryption for EBS volumes attached to worker nodes. (REQ-1-081)
  launch_template {
    name = "${local.cluster_name}-lt"
    block_device_mappings {
      device_name = "/dev/xvda"
      ebs {
        volume_size           = var.node_disk_size
        volume_type           = "gp3"
        encrypted             = true
        delete_on_termination = true
      }
    }
  }

  update_config {
    max_unavailable = 1
  }

  depends_on = [
    aws_iam_role_policy_attachment.nodes_worker_policy,
    aws_iam_role_policy_attachment.nodes_cni_policy,
    aws_iam_role_policy_attachment.nodes_ecr_readonly_policy,
  ]

  tags = merge(local.tags, {
    Name = "${local.cluster_name}-node-group"
  })
}