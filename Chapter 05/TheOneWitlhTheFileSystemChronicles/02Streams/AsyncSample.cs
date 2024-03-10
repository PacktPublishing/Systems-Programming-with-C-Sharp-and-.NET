using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelperClasses;

namespace _02Streams
{
    internal class AsyncSample
    {

public async Task CreateBigFile(string fileName, CancellationToken cancellationToken)
{
    var stream = File.CreateText(fileName);
    for (int i = 0; i < Int32.MaxValue; i++)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            Console.WriteLine("We are being cancelled");
            break;
        }
        else
        {
            var value = $"This is line {i}";
            Console.WriteLine(value);
            await stream.WriteLineAsync(value);
            try
            {
                await Task.Delay(10, cancellationToken);
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine("We are being cancelled");
                break;
            }
        }
    }

    Console.WriteLine("Closing the stream");
    stream.Close();
    await stream.DisposeAsync();
}

        public async Task CreateBigFileNaively(string fileName)
        {
            var stream = File.CreateText(fileName);
            for (int i = 0; i < Int32.MaxValue; i++)
            {
                    var value = $"This is line {i}";
                    value.Dump();
                    await stream.WriteLineAsync(value);
                        await Task.Delay(10);
                
            }

            "Closing the stream".Dump();
            stream.Close();
            await stream.DisposeAsync();
        }

        public async Task<string[]> ReadContents(string fileName)
        {
            var allLines = File.ReadAllLines(fileName);
            var alLines = await File.ReadAllLinesAsync(fileName);

            return alLines;
        }
    }
}
