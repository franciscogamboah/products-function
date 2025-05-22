variable "region" {
  type    = string
  default = "us-east-1"
}

variable "cluster_name" {
  description = "Nombre del cluster EKS"
  type        = string
}

variable "cluster_version" {
  description = "Versión del cluster EKS"
  type        = string
  default     = "1.31"
}

variable "vpc_id" {
  type = string
}

variable "public_subnet_id" {
  type = string
}

variable "private_subnet_ids" {
  type    = list(string)
  default = []
}

variable "key_name" {
  type = string
}

variable "alumno_prefix" {
  type = string
}

variable "ec2_name" {
  description = "Nombre de la instancia EC2"
  type        = string
}

variable "tags" {
  description = "The key-value maps for tagging"
  type        = map(string)
  default     = {}
}

variable "ecr_names" {
  description = "Lista de nombres para los repositorios ECR"
  type        = list(string)
}
