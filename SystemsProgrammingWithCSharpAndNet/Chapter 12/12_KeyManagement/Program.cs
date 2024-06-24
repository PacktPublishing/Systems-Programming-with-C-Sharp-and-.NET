using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using ExtensionLibrary;


// Read the key from Azure Keyvault

var keyvaultHelper = new KeyVaultHelper();
string secret = await keyvaultHelper.GetSecretAsync("https://dvroegopkeyvault.vault.azure.net/", "MySecretValue");

$"Secret: {secret}".Dump();

public class KeyVaultHelper
{
public async Task<string> GetSecretAsync(string keyVaultUrl, string secretName)
{
    var client = 
        new SecretClient(
            new Uri(keyVaultUrl), 
            new DefaultAzureCredential());
    var secret = 
        await client.GetSecretAsync(secretName);
    return secret.Value.Value;
}
}
