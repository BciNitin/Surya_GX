variable "client_id" {}
variable "client_secret" {}

variable "agent_count" {
    default = 1
}

variable "ssh_public_key" {
    default = "keys/id_rsa.pub"
}

variable "dns_prefix" {
    default = "splatformk8s"
}

variable cluster_name {
    default = "splatformk8s"
}

variable resource_group_name {
    default = "azure-splatformk8s"
}

variable location {
    default = "eastus"
}

variable log_analytics_workspace_name {
    default = "testLogAnalyticsWorkspaceName"
}

# refer https://azure.microsoft.com/global-infrastructure/services/?products=monitor for log analytics available regions
variable log_analytics_workspace_location {
    default = "eastus"
}

# refer https://azure.microsoft.com/pricing/details/monitor/ for log analytics pricing 
variable log_analytics_workspace_sku {
    default = "PerGB2018"
}