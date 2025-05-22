region         = "us-east-1"
cluster_name   = "eks-fgamboa-cluster-utec"
key_name       = "node-alumnos-utec"
vpc_id           = "vpc-04073a3d1773d2a8a"  # VPC ID
public_subnet_id  = "subnet-07e4b11693ca60caa"    # Public subnet ID
private_subnet_ids = ["subnet-017db75415d8eb85d","subnet-0e006dde58b2d755d"]  # Private subnet IDs 
alumno_prefix    = "lab_utec"   # nombre de alumno
ec2_name         = "ec2-fgamboa01-utec"   # Nombre de la instancia EC2
ecr_names        = ["fgamboa01"]  # Lista de nombres para los repositorios ECR
tags = {
  Name        = "UTEC"
  Environment = "LAB"
}