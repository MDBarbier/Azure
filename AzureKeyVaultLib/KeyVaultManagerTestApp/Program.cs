using System;
using AzureKeyVaultLib;

namespace KeyVaultManagerTestApp
{
    internal class Program
    {
        private static void Main()
        {
            Console.WriteLine("Testing KeyVaultManagerLib...");

            IEnvironmentVariableManager evm = new EnvironmentVariableManager();

            evm.EnvironmentVariablePreflight(AzureDevParameters.TenantId, AzureDevParameters.ClientId, AzureDevParameters.ClientSecret);

            using IAzureKeyVaultManager kvm = new AzureKeyVaultManager("https://bfindev1deploy-vlt.vault.azure.net/");

            var result = kvm.GetAllSecretProperties();
        }
    }
}
