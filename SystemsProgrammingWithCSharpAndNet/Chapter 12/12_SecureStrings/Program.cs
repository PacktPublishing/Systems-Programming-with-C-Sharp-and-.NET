
using System.Security;
using ExtensionLibrary;

var normalString = "This is a string";

SecureString secureString = new SecureString();
for (var index = 0; index < normalString.Length; index++)
{
    var c = normalString[index];
    secureString.AppendChar(c);
}

"String is loaded in memory".Dump();

"Done.".Dump();
Console.ReadLine();

$"Just for completeness: {normalString}".Dump();
$"And this one: {secureString}".Dump();
