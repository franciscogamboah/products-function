resource "aws_ecr_repository" "this" {
  for_each             = toset(var.ecr_name)
  name                 = each.key
  force_delete          = true
  image_tag_mutability = var.image_mutability

  encryption_configuration {
    encryption_type = "AES256"
  }

  image_scanning_configuration {
    scan_on_push = true
  }

  tags = var.tags
}