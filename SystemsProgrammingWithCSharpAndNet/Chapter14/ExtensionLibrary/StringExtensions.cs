using static System.Threading.Thread;
namespace ExtensionLibrary;

public static class StringExtensions
{
    public static string? Dump(this string? message, ConsoleColor printColor = ConsoleColor.Cyan)
    {
        // I add the lock here to prevent multiple threads accessing this method at the same time.
        // This could otherwise cause the wrong colors to be used when printing.
        // Lock makes sure this doesn't happen.
        // Consider this a bonus lesson in thread safety.
        lock (new object())
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = printColor;

            Console.WriteLine($"({CurrentThread.ManagedThreadId})\t : {message}");

            Console.ForegroundColor = oldColor;
            return message;
        }
    }
}