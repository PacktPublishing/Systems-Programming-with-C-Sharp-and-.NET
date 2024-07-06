using ExtensionLibrary;
using JSON_RPC;

#pragma warning disable 4014
var cancellationTokenSource = new CancellationTokenSource();

"Starting the server".Dump(ConsoleColor.Green);
var server = new Server(cancellationTokenSource.Token);
// start the servers method StartServer in a background thread
Task.Run(() => server.StartServer(), cancellationTokenSource.Token);

var client = new Client(cancellationTokenSource.Token);
Task.Run(() => client.StartClient(), cancellationTokenSource.Token);

"Server and client are running, press any key to stop".Dump(ConsoleColor.Green);
var input = Console.ReadKey();

"Stopping all".Dump(ConsoleColor.Green);

#pragma warning restore 4014