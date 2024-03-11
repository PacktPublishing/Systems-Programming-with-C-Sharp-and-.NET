using _08_SmartHttpsClient;
using ExtensionLibrary;

"Getting data from the server...".Dump(ConsoleColor.Cyan);
var client = HttpClientFactory.Instance;
var data = await client.GetStringAsync(
    "https://jsonplaceholder.typicode.com/posts");
$"Received: {data}".Dump(ConsoleColor.Yellow);
