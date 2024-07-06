using System.Net;
using ExtensionLibrary;

namespace _08_FtpSample;

internal class FtpClientOldStyle
{
    public static void FetchDirectoryContents(string ftpUrl, string username, string password)
    {
        // Create an FTP Request to the directory URL
        var request = (FtpWebRequest) WebRequest.Create(ftpUrl);
        request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

        // Add credentials
        request.Credentials = new NetworkCredential(username, password);

        try
        {
            // Get the server's response
            using var response = (FtpWebResponse) request.GetResponse();
            // Read the response stream
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                var line = string.Empty;
                while ((line = streamReader.ReadLine()) != null) Console.WriteLine(line);
            }

            $"Directory List Complete, status {response.StatusDescription}".Dump(ConsoleColor.Cyan);
        }
        catch (WebException ex)
        {
            // In case of a problem, log it.
            var status = ((FtpWebResponse) ex.Response).StatusDescription;
            $"Error: {status}".Dump(ConsoleColor.Red);
        }
    }
}