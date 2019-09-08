variable "AWS_ACCESS_KEY" {
}

variable "AWS_SECRET_KEY" {
}

variable "AWS_REGION" {
  default = "us-east-1"
}

variable "BUILD_BUILDNUMBER" {
}

variable "ENVIRONMENT" {
  default = "develop"
}

variable "RUNTIME" {
  default = "dotnetcore2.1"
}


variable "RESERVED_CAPACITY" {
  default = "1"
}

variable "MEM_SIZE" {
  default = "128"
}

variable "TIMEOUT" {
  default = "10"
}