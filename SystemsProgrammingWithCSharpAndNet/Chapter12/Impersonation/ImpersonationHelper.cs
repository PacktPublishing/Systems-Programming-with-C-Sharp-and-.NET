using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Principal;
using Microsoft.Win32.SafeHandles;

#pragma warning disable CA1416

namespace _12_Impersontation;

internal class ImpersonationHelper
{
    private const int LOGON32_LOGON_BATCH = 4;
    private const int LOGON32_LOGON_INTERACTIVE = 2;
    private const int LOGON32_PROVIDER_DEFAULT = 0;

    [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern bool LogonUser(
        string lpszUsername,
        string lpszDomain,
        string lpszPassword,
        int dwLogonType,
        int dwLogonProvider,
        out SafeAccessTokenHandle phToken);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    public static extern bool CloseHandle(IntPtr handle);

    public static void RunAsAdmin(
        string userName, 
        string domain, 
        string password, 
        Action action)
    {
        var returnValue = LogonUser(
            userName, 
            domain, 
            password, 
            LOGON32_LOGON_BATCH,
            LOGON32_PROVIDER_DEFAULT, 
            out var safeAccessTokenHandle);
        if (!returnValue)
        {
            var ret = Marshal.GetLastWin32Error();
            throw new Win32Exception(ret);
        }

        try
        {
            WindowsIdentity.RunImpersonated(safeAccessTokenHandle, () => { action(); });
        }
        finally
        {
            safeAccessTokenHandle.Dispose();
        }
    }
}