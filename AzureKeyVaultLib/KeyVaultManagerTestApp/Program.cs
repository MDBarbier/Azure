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

            evm.EnvironmentVariablePreflight(AzureParameters.TenantId, AzureParameters.ClientId, AzureParameters.ClientSecret);

            using IAzureKeyVaultManager kvm = new AzureKeyVaultManager(AzureParameters.VaultUrl);

            var allSecretProps = kvm.GetAllSecretProperties();

            foreach (var secretProps in allSecretProps)
            {
                Console.WriteLine($"Secret name: {secretProps.Name}, created on: {secretProps.CreatedOn}");
            }
        }
    }
}
