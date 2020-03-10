using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AzureKeyVaultLib
{
    public class AzureKeyVaultManager : IAzureKeyVaultManager
    {
        internal string KeyVaultUrl { get; set; }
        internal SecretClient SecretClient { get; set; }
        
        /// <summary>
        /// Constructor sets up the SecretClient using DefaultAzureCredentials object (which reads clientId, clientSecret and tenantId from environment variables, see ReadMe.md)
        /// </summary>
        /// <param name="vaultName">The name of the vault to open</param>
        public AzureKeyVaultManager(string vaultName)
        {
            KeyVaultUrl = vaultName;
            SecretClient = new SecretClient(vaultUri: new Uri(KeyVaultUrl), credential: new DefaultAzureCredential());
        }

        /// <summary>
        /// Add a secret to the vault
        /// </summary>
        /// <param name="secretName">The name to give the secret</param>
        /// <param name="secretValue">The value the secret should have</param>
        /// <returns>True if successfully added</returns>
        public bool AddSecret(string secretName, string secretValue)
        {            
            var secret = SecretClient.SetSecret(secretName, secretValue);

            return !string.IsNullOrEmpty(secret.Value.Value);            
        }

        /// <summary>
        /// Get the value of the specified secret
        /// </summary>
        /// <param name="secretName">The secret to get the value of</param>
        /// <returns>String value of the secret</returns>
        public string GetSecretValue(string secretName)
        {
            return SecretClient.GetSecret(secretName).Value.Value;
        }

        /// <summary>
        /// Get the specified secret object
        /// </summary>
        /// <param name="secretName">The secret to get</param>
        /// <returns>KeyVaultSecret object</returns>
        public KeyVaultSecret GetSecret(string secretName)
        {
            return SecretClient.GetSecret(secretName).Value;
        }

        /// <summary>
        /// Return an enumerable containing the properties of every secret in the vault i.e. Name etc. Note that the value is not included you must call the GetSecretValue method.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SecretProperties> GetAllSecretProperties()
        {
            return SecretClient.GetPropertiesOfSecrets().ToList();
        }

        public void Dispose()
        {
            this.SecretClient = null;
        }
    }
}
