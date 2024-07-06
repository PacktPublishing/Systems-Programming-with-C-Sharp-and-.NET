// Get the name of the current user

using _12_Impersontation;
using ExtensionLibrary;

var userName = Environment.UserName;
$"Current user: {userName}".Dump();

var adminUserName = "MySecureAdmin";
var domain = "dennis-beast";
var password = "Welcome@1";

ImpersonationHelper.RunAsAdmin(
    adminUserName, domain, password, () =>
{
    var otherUserName = Environment.UserName;
    $"Username {otherUserName}".Dump();
});

var finalUserName = Environment.UserName;
$"Current user: {finalUserName}".Dump();
