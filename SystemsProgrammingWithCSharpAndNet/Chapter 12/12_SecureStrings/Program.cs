
using System.Net;
using System.Security;
using ExtensionLibrary;

var secureString = new SecureString();
var sourceString = "MyPassword";
foreach (var c in sourceString)
{
    secureString.AppendChar(c);
}
secureString.MakeReadOnly();

var nc = new NetworkCredential("myUserName", secureString, "MyDomain");

return;

void OverwriteAndClearString(ref string str)
{
    if (str == null) return;
    unsafe
    {
        fixed (char* ptr = str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                ptr[i] = '\0'; // Overwrite with null characters
            }
        }
    }

    str = null; // Dereference the string
}