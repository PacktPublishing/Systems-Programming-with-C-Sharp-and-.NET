using ExtensionLibrary;
using SmartHttpsClient;
using System.Net.NetworkInformation;

// First, check to see if our network is healthy
foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
{
    $"Name: {ni.Name}".Dump(ConsoleColor.DarkYellow);
    $"Type: {ni.NetworkInterfaceType}".Dump(ConsoleColor.DarkYellow);
    $"Status: {ni.OperationalStatus}".Dump(ConsoleColor.DarkYellow);
}


bool isHealthy = 
     System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
if (isHealthy)
{
    "Getting data from the server...".Dump(ConsoleColor.Cyan);
    var client = HttpClientFactory.Instance;

    var response = await HttpClientFactory.GetAsync(
        "https://jsonplaceholder.typicode.com/posts2");
    if (response.IsSuccessStatusCode)
    {
        string content = await response.Content.ReadAsStringAsync();
        $"Received: {content}".Dump(ConsoleColor.Yellow);
    }

}
else
{
    "Houston, we have a problem!".Dump(ConsoleColor.Red);
}


