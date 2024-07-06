

using _11_Benchmark;
using BenchmarkDotNet.Running;

//var mt = new ModuloTesters();
//mt.TestMultiplicationAndDivisionOnGpu(12345, 1000000);
var summary = BenchmarkRunner.Run<ModuloTesters>();
