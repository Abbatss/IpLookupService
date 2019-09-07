provider "aws" {
  access_key = var.AWS_ACCESS_KEY
  secret_key = var.AWS_SECRET_KEY
  region     = var.AWS_REGION
}

terraform {
  backend "s3" {
    bucket = "tz-ipsearch-terraform-backend"
    key    = "tz.ipsearch.tfstate"
    region = "eu-west-2"
  }
}

resource "aws_iam_role" "iam-tz-ipsearch-api-role" {
  name = "iam-tz-${terraform.workspace}-ipsearch-api-role"

  assume_role_policy = <<EOF
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Action": "sts:AssumeRole",
      "Principal": {
        "Service": "lambda.amazonaws.com"
      },
      "Effect": "Allow",
      "Sid": ""
    }
  ]
}
EOF

}

resource "aws_iam_role_policy" "iam-tz-ipsearch-api-policy" {
  name = "iam-tz-${terraform.workspace}-ipsearch-api-policy"
  role = aws_iam_role.iam-tz-ipsearch-api-role.id

  policy = <<EOF
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Action": [
        "logs:CreateLogGroup",
        "logs:CreateLogStream",
        "logs:PutLogEvents"
      ],
      "Resource": [
        "arn:aws:logs:*:*:*"
      ],
      "Effect": "Allow"
    }
  ]
}
EOF

}

data "aws_s3_bucket_object" "tz-ipsearch-api" {
bucket = "tz-ipsearch-api"
key    = "IPLookup.API.Host.Lambda-${var.BUILD_BUILDNUMBER}/IPLookup.API.Host.Lambda.zip"
}

resource "aws_lambda_function" "tz-ipsearch-api" {
function_name                  = "tz-${terraform.workspace}-ipsearch-api"
s3_bucket                      = data.aws_s3_bucket_object.tz-ipsearch-api.bucket
s3_key                         = data.aws_s3_bucket_object.tz-ipsearch-api.key
role                           = aws_iam_role.iam-tz-ipsearch-api-role.arn
source_code_hash               = data.aws_s3_bucket_object.tz-ipsearch-api.etag
reserved_concurrent_executions = var.RESERVED_CAPACITY
memory_size                    = var.MEM_SIZE
timeout                        = var.TIMEOUT
handler                        = "IPLookup.API.Host.Lambda::IPLookup.API.Host.Lambda.LambdaEntryPoint::FunctionHandlerAsync"
runtime                        = var.RUNTIME

environment {
variables = {
  }
}
}

resource "aws_api_gateway_resource" "proxy" {
rest_api_id = aws_api_gateway_rest_api.tz-ipsearch-api.id
parent_id   = aws_api_gateway_rest_api.tz-ipsearch-api.root_resource_id
path_part   = "{proxy+}"
}

resource "aws_api_gateway_method" "proxy" {
rest_api_id   = aws_api_gateway_rest_api.tz-ipsearch-api.id
resource_id   = aws_api_gateway_resource.proxy.id
http_method   = "ANY"
authorization = "NONE"
}

resource "aws_api_gateway_integration" "lambda" {
rest_api_id = aws_api_gateway_rest_api.tz-ipsearch-api.id
resource_id = aws_api_gateway_method.proxy.resource_id
http_method = aws_api_gateway_method.proxy.http_method

integration_http_method = "POST"
type                    = "AWS_PROXY"
uri                     = aws_lambda_function.tz-ipsearch-api.invoke_arn
}

resource "aws_api_gateway_rest_api" "tz-ipsearch-api" {
name = "tz-${terraform.workspace}-ipsearch-api"
}

resource "aws_api_gateway_method" "proxy_root" {
rest_api_id   = aws_api_gateway_rest_api.tz-ipsearch-api.id
resource_id   = aws_api_gateway_rest_api.tz-ipsearch-api.root_resource_id
http_method   = "ANY"
authorization = "NONE"
}

resource "aws_api_gateway_integration" "lambda_root" {
rest_api_id = aws_api_gateway_rest_api.tz-ipsearch-api.id
resource_id = aws_api_gateway_method.proxy_root.resource_id
http_method = aws_api_gateway_method.proxy_root.http_method

integration_http_method = "POST"
type                    = "AWS_PROXY"
uri                     = aws_lambda_function.tz-ipsearch-api.invoke_arn
}

resource "aws_api_gateway_deployment" "tz-ipsearch-api-gateway" {
depends_on = [
aws_api_gateway_integration.lambda,
aws_api_gateway_integration.lambda_root,
]

rest_api_id = aws_api_gateway_rest_api.tz-ipsearch-api.id
stage_name  = var.ENVIRONMENT
}

resource "aws_lambda_permission" "apigw" {
statement_id  = "AllowAPIGatewayInvoke"
action        = "lambda:InvokeFunction"
function_name = aws_lambda_function.tz-ipsearch-api.arn
principal     = "apigateway.amazonaws.com"
# The /*/* portion grants access from any method on any resource
# within the API Gateway "REST API".
source_arn = "${aws_api_gateway_deployment.tz-ipsearch-api-gateway.execution_arn}/*/*"
}

output "base_url" {
value = aws_api_gateway_deployment.tz-ipsearch-api-gateway.invoke_url
}