This library allows you to interact with an Azure KeyVault Secrets.

Setup:

- When you've cloned the code, create a class called "AzureParameters.cs" in the root directory of KeyVaultManagerTestApp
- Add the following properties and give them the appropriate values:
	    public static string TenantId { get; } = "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx";
        public static string ClientId { get; } = "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx";
        public static string ClientSecret { get; } = "xxxxxxxxxxxxxxxxxxxxxxxxxxx";
        public static string VaultUrl { get; } = "https://insertyourdomainhere.vault.azure.net/";
- Once these values have been added you should be able to run the test application, which demonstrates by downloading all secrets and outputting some information about them to the console.

Note that behind the scenes the values you specify in the code file mentioned are being added as environment variables under the user context the app is run.

This is because the Azure KeyVault module requires them to be added as EVs. 

These are the three environment variables:

AZURE_CLIENT_SECRET
AZURE_CLIENT_ID
AZURE_TENANT_ID

These can be viewed in powershell like this:

[System.Environment]::GetEnvironmentVariable('AZURE_TENANT_ID',[System.EnvironmentVariableTarget]::User)


--------

Note that the ClientId and Client secret must be from a Service Principal, such as that linked to an AAD App Registration, which has the appropriate permissions on the KeyVault. 
Note this is assigned in the "Acces Policies" screen and not the IAM Access Control screen.