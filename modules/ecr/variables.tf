variable "ecr_name" {
  description = "Lista de nombres de repositorios ECR a crear"
  type        = list(string)
}

variable "image_mutability" {
  description = "Mutabilidad de las etiquetas de imagen"
  type        = string
  default     = "IMMUTABLE"
}

variable "tags" {
  description = "Etiquetas para los repositorios"
  type        = map(string)
  default     = {}
}