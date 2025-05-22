module "eks" {
  source  = "terraform-aws-modules/eks/aws"
  version = "~> 19.0"

  cluster_name    = var.cluster_name
  cluster_version = var.cluster_version

  cluster_endpoint_public_access = true

  cluster_addons = {
    coredns = { most_recent = true }
    kube-proxy = { most_recent = true }
    vpc-cni = { most_recent = true }
    eks-pod-identity-agent = { most_recent = true }
  }

  vpc_id     = var.vpc_id
  subnet_ids = var.private_subnets

  eks_managed_node_groups = {
    public = {
      min_size     = 1
      max_size     = 2
      desired_size = 1

      instance_types = ["t3.large"]
      capacity_type  = "ON_DEMAND"
      key_name       = var.key_name
    }
  }

  aws_auth_users = [
    {
      userarn  = "arn:aws:iam::654654589924:user/jcontreras_utec"
      username = "user-utec-tf"
      groups   = ["system:masters"]
    }
  ]

  tags = var.tags
}