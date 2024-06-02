// Build configuration

using ExtensionLibrary;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.DataProtection;

var serviceCollection = new ServiceCollection();
serviceCollection.AddDataProtection();
var serviceProvider = serviceCollection.BuildServiceProvider();//(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });

var dataProtector = serviceProvider.GetDataProtector("MySecureData");


var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

var json = File.ReadAllText("appsettings.json");

var secretSection = configuration.GetSection("MySecretSettings");
foreach(var key in secretSection.GetChildren())
{
    var originalValue = key.Value;
    var encryptedValue = dataProtector.Protect(originalValue);
    var oldValue = $"\"{key.Key}\": \"{originalValue}\"";
    var newValue = $"\"{key.Key}\": \"{encryptedValue}\"";
    json = json.Replace(oldValue, newValue);
}

File.WriteAllText("appsettings.json", json);

