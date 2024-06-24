using ExtensionLibrary;

var mySecretdId = 
    Environment.GetEnvironmentVariable("MY_GLOBAL_SECRET_ID");
$"Found this in the registry: {mySecretdId}".Dump();

