using ExtensionLibrary;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


var myKeyFolder = Path.Combine(Directory.GetCurrentDirectory(), "MyKeys");
if(!Directory.Exists(myKeyFolder))
    Directory.CreateDirectory(myKeyFolder);

var serviceCollection = new ServiceCollection();
serviceCollection
    .AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(myKeyFolder));
var serviceProvider = serviceCollection.BuildServiceProvider();

var dataProtector = serviceProvider.GetDataProtector("MySecureData");

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json",
        false,
        true)
    .Build();

var secretSection = configuration.GetSection("MySecretSettings");

var json = File.ReadAllText("appsettings.json");


foreach (var key in secretSection.GetChildren())
{
    var originalValue = key.Value;
    var encryptedValue = dataProtector.Protect(originalValue);
    var oldValue = $"\"{key.Key}\": \"{originalValue}\"";
    var newValue = $"\"{key.Key}\": \"{encryptedValue}\""; 
    json = json.Replace(oldValue, newValue);
}

File.WriteAllText("appsettings.json", json);

// Force reloading.
configuration.Reload();
var encryptedSection = configuration.GetSection("MySecretSettings");
var someSecretValue = encryptedSection["MySecretSetting1"];

var secondProtector = serviceProvider.GetDataProtector("MySecureData");
var decryptedValue = secondProtector.Unprotect(someSecretValue);
$"Encrypted value was: {someSecretValue}\nDecrypted this becomes: {decryptedValue}".Dump();