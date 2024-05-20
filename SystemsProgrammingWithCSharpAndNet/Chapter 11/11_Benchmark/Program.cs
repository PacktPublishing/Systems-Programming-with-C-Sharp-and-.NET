

using _11_Benchmark;
using BenchmarkDotNet.Running;

//var mt = new ModuleTesters();
//mt.TestMultiplicationAndDivisionOnGpu(12345, 1000000);
var summary = BenchmarkRunner.Run<ModuleTesters>();
