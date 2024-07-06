using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;


var myClientId = "";//
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json",
        false,
        true)
    .Build();

var keyVaultName = configuration.GetSection("keyVault")["vaultName"];
var kvUri = "https://" + keyVaultName + ".vault.azure.net/";
var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());
var secret = await client.GetSecretAsync("MySecretValue");
Console.WriteLine(secret.Value);

