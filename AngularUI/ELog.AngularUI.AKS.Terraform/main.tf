provider "azurerm" {
    # The "feature" block is required for AzureRM provider 2.x. 
    # If you are using version 1.x, the "features" block is not allowed.
    version = "~>2.0"
    # subscription_id = "294d51bc-6108-4cff-a8a4-e09870140f12"
    # client_id       = "4b2848cb-0f95-4192-8f6a-917b515f7224"
    # client_secret   = "1616f821-d87f-4cb7-9cc1-565b043c323e"
    # tenant_id       = "177cfeea-2e74-499f-853d-73e9fb3f88a7"
    features {}
}

terraform {
  backend "azurerm" {
    storage_account_name = "terraformstateblobstore"
    container_name       = "k8stfstate"
    key                  = "prod.terraform.tfstate"
    access_key = "NdbUmzOBfH+VH2D1oyutYUgqwwZbI+268uH48Xy+cvaRT2R35bKQEEnnPCE2mcxY8opeNdymb1sATr5MygmpSg=="
  }
}