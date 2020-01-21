Note the "new DefaultAzureCredential()" line of code relies on three environment variables:

AZURE_CLIENT_SECRET
AZURE_CLIENT_ID
AZURE_TENANT_ID

These can be set in powershell like this:

[System.Environment]::SetEnvironmentVariable('AZURE_TENANT_ID','xxxx-xxxxxxxx-xxxxxxxxxx-xxxx',[System.EnvironmentVariableTarget]::User)


--------



There must be a Service Principal, such as that linked to an App Registration, which has permissions on the KeyVault. Note this is assigned in the "Acces Policies" screen and not the IAM Access Control screen.