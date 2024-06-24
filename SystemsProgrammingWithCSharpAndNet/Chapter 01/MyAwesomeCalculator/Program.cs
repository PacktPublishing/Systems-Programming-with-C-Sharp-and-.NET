// See https://aka.ms/new-console-template for more information

Console.WriteLine("Starting the loop\n\n");
int i = 1;
while (true)
{
    Console.WriteLine($"In the loop at {DateTime.Now.TimeOfDay} for loop {i++}");
    Thread.Sleep(1000);
    if(i > 10)
        throw new Exception("Something weird happened...");
}