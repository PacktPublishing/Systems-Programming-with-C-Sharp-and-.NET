
using _03Security;

string path = @"C:\temp\file.txt";

System.Security.AccessControl.AuthorizationRuleCollection rules;
// Get the rules for the file in path
//rules = System.IO.File.GetAccessControl(path).GetAccessRules(true, true, typeof(System.Security.Principal.NTAccount));