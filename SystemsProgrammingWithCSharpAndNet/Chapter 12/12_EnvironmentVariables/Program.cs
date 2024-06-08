using ExtensionLibrary;

"Hello".Dump();

var x = System.Environment.GetEnvironmentVariable("MY_VARIABLE");
$"Found this in the registry: {x}".Dump();
