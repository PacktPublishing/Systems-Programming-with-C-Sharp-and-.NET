using Benchmark;
using BenchmarkDotNet.Running;

var summary = BenchmarkRunner.Run<ModuloTesters>();
