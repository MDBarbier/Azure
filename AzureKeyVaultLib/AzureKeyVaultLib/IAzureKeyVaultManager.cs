using System;
using System.Collections.Generic;
using Azure.Security.KeyVault.Secrets;

namespace AzureKeyVaultLib
{
    public interface IAzureKeyVaultManager : IDisposable
    {
        bool AddSecret(string secretName, string secretValue);
        string GetSecretValue(string secretName);
        KeyVaultSecret GetSecret(string secretName);
        IEnumerable<SecretProperties> GetAllSecretProperties();
    }
}