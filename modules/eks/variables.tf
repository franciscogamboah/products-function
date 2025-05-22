variable "cluster_name" {}
variable "cluster_version" { default = "1.31" }
variable "vpc_id" {}
variable "private_subnets" { type = list(string) }
variable "key_name" {}
variable "tags" { type = map(string) }
