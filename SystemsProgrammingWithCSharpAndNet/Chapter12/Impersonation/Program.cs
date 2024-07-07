// Get the name of the current user

using ExtensionLibrary;
using Impersontation;

var userName = Environment.UserName;
$"Current user: {userName}".Dump();

var adminUserName = "MySecureAdmin";
var domain = "yourdomain";
var password = "YourPassword";

ImpersonationHelper.RunAsAdmin(
    adminUserName, domain, password, () =>
{
    var otherUserName = Environment.UserName;
    $"Username {otherUserName}".Dump();
});

var finalUserName = Environment.UserName;
$"Current user: {finalUserName}".Dump();
