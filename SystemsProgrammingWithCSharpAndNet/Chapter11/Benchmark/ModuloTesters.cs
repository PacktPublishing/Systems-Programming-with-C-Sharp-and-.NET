using BenchmarkDotNet.Attributes;

namespace Benchmark;

public class ModuloTesters
{
    private const int testNumber = 400;
    private const int numberOfLoopCount = 100000;

    [Benchmark]
    public void TestModulo()
    {
        var numberOfMatches = 0;
        for (var i = 3; i < numberOfLoopCount; i++)
            if (testNumber % i == 0)
                numberOfMatches++;
    }

    [Benchmark]
    public void TestMultiplicationAndDivision()
    {
        var numberOfMatches = 0;
        for (var i = 3; i < numberOfLoopCount; i++)
            if (testNumber - i * (testNumber / i) == 0)
                numberOfMatches++;
    }

    [Benchmark]
    public void TestMultiplicationAndDivisionOptimized()
    {
        var numberOfMatches = 0;
        var localNumberOfLoopCount = numberOfLoopCount; // Assuming numberOfLoopCount is a class member
        var localTestNumber = testNumber; // Assuming testNumber is a class member

        for (var i = 3; i < localNumberOfLoopCount; i++)
        {
            var div = localTestNumber / i;
            if (localTestNumber == i * div) numberOfMatches++;
        }
    }

    [Benchmark]
    public unsafe void TestMultiplicationAndDivisionUnsafe()
    {
        var numberOfMatches = 0;
        var localNumberOfLoopCount = numberOfLoopCount;
        var localTestNumber = testNumber;

        // Get the address of localTestNumber
        var ptrTestNumber = &localTestNumber;

        for (var i = 3; i < localNumberOfLoopCount; i++)
        {
            var div = *ptrTestNumber / i;
            if (*ptrTestNumber == i * div) numberOfMatches++;
        }
    }


    [Benchmark]
    public void TestMultiplicationAndDivisionInParallel()
    {
        var numberOfMatches = 0;
        var localNumberOfLoopCount = numberOfLoopCount;
        var localTestNumber = testNumber;

        var lockObj = new object();

        Parallel.For(3, localNumberOfLoopCount, i =>
        {
            var div = localTestNumber / i;
            if (localTestNumber == i * div)
                lock (lockObj)
                {
                    numberOfMatches++;
                }
        });
    }


}