using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtensionLibrary;

namespace _11_Profiling
{
    internal class PrimeCalculator
    {
        public void Run()
        {
            var limit = 100000;
            
                Stopwatch stopwatch = Stopwatch.StartNew();
                long sum = SumOfPrimes(limit);
                stopwatch.Stop();

                $"Sum of primes up to {limit}: {sum}".Dump(ConsoleColor.Cyan);
                $"Time taken: {stopwatch.ElapsedMilliseconds} ms".Dump(ConsoleColor.Cyan);

        }

        long SumOfPrimes(int limit)
        {
            long sum = 0;
            for (int i = 2; i <= limit; i++)
            {
                if (IsPrime(i))
                {
                    sum += i;
                }
            }

            return sum;
        }

        bool IsPrime(int number)
        {
            if (number < 2) return false;
            if(number == 2) return true;
            if(number % 2 == 0) return false;
            var upperLoad = Math.Sqrt(number);
            for (int i = 2; i <= upperLoad; i++) // Inefficiency: redundant check
            {
                if (number % i == 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
