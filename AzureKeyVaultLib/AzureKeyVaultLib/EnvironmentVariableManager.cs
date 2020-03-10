using System;
using System.Collections.Generic;
using System.Text;

namespace AzureKeyVaultLib
{
    public class EnvironmentVariableManager : IEnvironmentVariableManager
    {
        public void SetEnvironmentVariable(string name, string value, EnvironmentVariableTarget target)
        {
            Environment.SetEnvironmentVariable(name, value, target);
        }

        public string GetEnvironmentVariableValue(string name, EnvironmentVariableTarget target)
        {
            return Environment.GetEnvironmentVariable(name, target);
        }

        public void DeleteEnvironmentVariable(string name, EnvironmentVariableTarget target)
        {
            Environment.SetEnvironmentVariable(name, null, target);
        }

        public void EnvironmentVariablePreflight(string tenantId, string clientId, string clientSecret)
        {
            var storedClientId = GetEnvironmentVariableValue("AZURE_CLIENT_ID", EnvironmentVariableTarget.User);
            var storedTenantId = GetEnvironmentVariableValue("AZURE_TENANT_ID", EnvironmentVariableTarget.User);
            var storedClientSecret = GetEnvironmentVariableValue("AZURE_CLIENT_SECRET", EnvironmentVariableTarget.User);

            if (string.IsNullOrEmpty(storedTenantId) || storedTenantId != tenantId)
            {
                SetEnvironmentVariable("AZURE_TENANT_ID", tenantId, EnvironmentVariableTarget.User);
            }

            if (string.IsNullOrEmpty(storedClientId) || storedClientId != clientId)
            {
                SetEnvironmentVariable("AZURE_CLIENT_ID", clientId, EnvironmentVariableTarget.User);
            }

            if (string.IsNullOrEmpty(storedClientSecret) || storedClientSecret != clientSecret)
            {
                SetEnvironmentVariable("AZURE_CLIENT_SECRET", clientSecret, EnvironmentVariableTarget.User);
            }
        }
    }
}
