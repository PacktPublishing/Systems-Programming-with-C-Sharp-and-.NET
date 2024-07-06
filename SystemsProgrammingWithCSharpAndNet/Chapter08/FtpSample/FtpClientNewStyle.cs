using ExtensionLibrary;
using FluentFTP;

namespace FtpSample;

internal class FtpClientNewStyle
{
    public static void FetchDirectoryContents(string ftpUrl, string username, string password)
    {
        // Create an FTP client
        var client = new FtpClient(ftpUrl, username, password);

        // Get the list of files and directories
        var items = client.GetListing(ftpUrl);

        // Print the list
        foreach (var item in items) item.Name.Dump();
    }
}