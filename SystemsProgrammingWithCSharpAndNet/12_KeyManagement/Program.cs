// See https://aka.ms/new-console-template for more information

using System.Security.Cryptography;
using ExtensionLibrary;

#pragma warning disable CA1416

Console.WriteLine("Hello, World!");

string mySuperSecret = "This is my super secret data";
byte[] data = System.Text.Encoding.UTF8.GetBytes(mySuperSecret);

var encryptedData = ProtectedData.Protect(data, null, DataProtectionScope.LocalMachine);
var decryptedData = ProtectedData.Unprotect(encryptedData, null, DataProtectionScope.LocalMachine);

var decryptedString = System.Text.Encoding.UTF8.GetString(decryptedData);
$"Decrypted data: {decryptedString}".Dump();