using System.Security.AccessControl;
using ExtensionLibrary;
using Microsoft.Win32;
#pragma warning disable CA1416

"Windows Registry Sample".Dump(ConsoleColor.Cyan);


var key = Registry.LocalMachine.CreateSubKey(@"Software\SystemsProgrammers\Usage");

var currentUser = Environment.UserName;
var security = new RegistrySecurity();
var rule= new RegistryAccessRule(
          currentUser,
          RegistryRights.FullControl, 
          InheritanceFlags.None, 
          PropagationFlags.None, 
          AccessControlType.Allow);

security.AddAccessRule(rule);
key.SetAccessControl(security);

// get the value
var retrievedKey = key.GetValue("FirstAccess");
if (retrievedKey == null)
{
    // create the value
    key.SetValue(
        name: "FirstAccess", 
        value: DateTime.UtcNow.ToBinary(), 
        valueKind: RegistryValueKind.QWord);
    "First access recorded now".Dump(ConsoleColor.Cyan);
}
else
{
    if (retrievedKey is long firstAccessAsString)
    {
        var retrievedFirstAccess = 
            DateTime.FromBinary(firstAccessAsString);
        $"Retrieved first access: {retrievedFirstAccess}".Dump(ConsoleColor.Cyan);
    }
}
