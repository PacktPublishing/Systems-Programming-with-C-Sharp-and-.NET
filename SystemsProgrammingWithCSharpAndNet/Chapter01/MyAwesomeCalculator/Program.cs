// See https://aka.ms/new-console-template for more information

using ExtensionLibrary;

"Starting the loop\n\n".Dump();
int i = 1;
while (true)
{
    $"In the loop at {DateTime.Now.TimeOfDay} for loop {i++}".Dump();
    Thread.Sleep(1000);
    if(i > 10)
        throw new Exception("Something weird happened...");
}