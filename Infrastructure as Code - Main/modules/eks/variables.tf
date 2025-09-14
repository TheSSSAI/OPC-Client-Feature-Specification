# ---------------------------------------------------------------------------------------------------------------------
# Input Variables for the EKS Cluster Module
# ---------------------------------------------------------------------------------------------------------------------

variable "cluster_name" {
  description = "The unique name of the EKS cluster."
  type        = string
  validation {
    condition     = can(regex("^[a-zA-Z0-9][a-zA-Z0-9-]*$", var.cluster_name))
    error_message = "Cluster name must start with a letter or number and may only contain letters, numbers, and hyphens."
  }
}

variable "cluster_version" {
  description = "The desired Kubernetes version for the EKS cluster control plane. (e.g., '1.29'). Fulfills part of REQ-1-089."
  type        = string
}

variable "vpc_id" {
  description = "The ID of the VPC where the EKS cluster and its nodes will be deployed."
  type        = string
}

variable "subnet_ids" {
  description = "A list of subnet IDs to deploy the EKS cluster and nodes into. Must span multiple Availability Zones for high availability (REQ-1-084)."
  type        = list(string)
}

variable "instance_types" {
  description = "The EC2 instance types for the worker nodes in the managed node group."
  type        = list(string)
  default     = ["t3.large"]
}

variable "node_group_name" {
  description = "The unique name for the default managed node group."
  type        = string
  default     = "default-worker-nodes"
}

variable "scaling_desired_size" {
  description = "The desired number of worker nodes in the node group. (REQ-1-085)"
  type        = number
  default     = 2
}

variable "scaling_max_size" {
  description = "The maximum number of worker nodes that the managed node group can scale out to. (REQ-1-085)"
  type        = number
  default     = 4
}

variable "scaling_min_size" {
  description = "The minimum number of worker nodes that the managed node group can scale in to. (REQ-1-085)"
  type        = number
  default     = 2
}

variable "node_disk_size" {
  description = "The disk size (in GiB) for the worker nodes."
  type        = number
  default     = 50
}

variable "tags" {
  description = "A map of tags to assign to all created resources."
  type        = map(string)
  default     = {}
}