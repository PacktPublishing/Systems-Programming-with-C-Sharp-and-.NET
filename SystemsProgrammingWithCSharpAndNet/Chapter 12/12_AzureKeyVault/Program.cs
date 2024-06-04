// See https://aka.ms/new-console-template for more information

using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;

//Console.WriteLine("Hello, World!");
//const string subId = "279d1d3a-3490-47dd-85e5-ee752ad3da9d";
//const string vaultName = "myvault";

var myClientId = "7bce7159-ab26-4e79-aeb7-63aa3c4313bd";
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

/*
  {
     "clientId": "354836dc-7165-4e86-a742-e8b62c050333",
     "clientSecret": "0oe2j6-8kpSB1K.uSB-WjjOsVzk40rw8-9",
     "subscriptionId": "279d1d3a-3490-47dd-85e5-ee752ad3da9d",
     "tenantId": "2afe6ced-40b8-4c3b-b779-3ce5270cb674",
     "activeDirectoryEndpointUrl": "https://login.microsoftonline.com",
     "resourceManagerEndpointUrl": "https://management.azure.com/",
     "activeDirectoryGraphResourceId": "https://graph.windows.net/",
     "sqlManagementEndpointUrl": "https://management.core.windows.net:8443/",
     "galleryEndpointUrl": "https://gallery.azure.com/",
     "managementEndpointUrl": "https://management.core.windows.net/"
   }
*/

/*
 Result from assignment create

az role assignment create --role "Key Vault Secrets User" --assignee "354836dc-7165-4e86-a742-e8b62c050333" --scope "/subscriptions/279d1d3a-3490-47dd-85e5-ee752ad3da9d/resourceGroups/systemsprogrammingrg/providers/Microsoft.KeyVault/vaults/dvroegopkeyvault"

{
     "canDelegate": null,
     "condition": null,
     "conditionVersion": null,
     "description": null,
     "id": "/subscriptions/279d1d3a-3490-47dd-85e5-ee752ad3da9d/resourceGroups/systemsprogrammingrg/providers/Microsoft.KeyVault/vaults/dvroegopkeyvault/providers/Microsoft.Authorization/roleAssignments/3205dc3a-0a4b-4995-b1bb-6f292f21d50b",
     "name": "3205dc3a-0a4b-4995-b1bb-6f292f21d50b",
     "principalId": "a4738628-c538-4130-933e-d8b0ca613094",
     "principalType": "ServicePrincipal",
     "resourceGroup": "systemsprogrammingrg",
     "roleDefinitionId": "/subscriptions/279d1d3a-3490-47dd-85e5-ee752ad3da9d/providers/Microsoft.Authorization/roleDefinitions/4633458b-17de-408a-b874-0445c86b69e6",
     "scope": "/subscriptions/279d1d3a-3490-47dd-85e5-ee752ad3da9d/resourceGroups/systemsprogrammingrg/providers/Microsoft.KeyVault/vaults/dvroegopkeyvault",
     "type": "Microsoft.Authorization/roleAssignments"
   }
*/