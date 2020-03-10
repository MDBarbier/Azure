using System;

namespace AzureKeyVaultLib
{
    public interface IEnvironmentVariableManager
    {
        void SetEnvironmentVariable(string name, string value, EnvironmentVariableTarget target);
        string GetEnvironmentVariableValue(string name, EnvironmentVariableTarget target);
        void DeleteEnvironmentVariable(string name, EnvironmentVariableTarget target);
        void EnvironmentVariablePreflight(string tenantId, string clientId, string clientSecret);
    }
}