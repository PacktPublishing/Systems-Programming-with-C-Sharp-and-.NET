using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtensionLibrary;

namespace _11_MultiThreaded
{
    internal class Deadlock
    {
        public void Run()
        {
            "Starting the threads".Dump(ConsoleColor.Cyan);

            var lockA = new object();
            var lockB = new object();

            ThreadPool.QueueUserWorkItem(_ =>
            {
                lock (lockA)
                {
                    "Thread 1 acquired lock A".Dump(ConsoleColor.Yellow);
                    Thread.Sleep(1000);
                    lock (lockB)
                    {
                        "Thread 1 acquired lock B".Dump(ConsoleColor.Yellow);
                    }
                }
            });

            ThreadPool.QueueUserWorkItem(_ =>
            {
                lock (lockB)
                {
                    "Thread 2 acquired lock B".Dump(ConsoleColor.Blue);
                    Thread.Sleep(1000);
                    lock (lockA)
                    {
                        "Thread 2 acquired lock A".Dump(ConsoleColor.Blue);
                    }
                }
            });

            "Waiting for all threads to finish".Dump(ConsoleColor.Cyan);
            Console.ReadLine();
        }
    }
}
