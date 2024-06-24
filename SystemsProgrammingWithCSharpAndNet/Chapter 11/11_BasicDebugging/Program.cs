using ExtensionLibrary;
using Timer = System.Timers.Timer;


var inThreadCounter = 0;
var timer = new Timer(1000);
timer.Elapsed += 
    (_, _) => 
    { 
        $"In the timer call with counter {inThreadCounter++}".Dump(ConsoleColor.Yellow); 
    };
timer.Start();

var outThreadCounter = 0;
while (true)
{
    var message = $"In the main thread with counter {outThreadCounter++}";
    message.Dump(ConsoleColor.Cyan);
   
    await Task.Delay(200);
}