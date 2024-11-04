// See https://aka.ms/new-console-temp9late for more information
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using MyBoards.Benchmark;

Console.WriteLine("Hello, World!");

BenchmarkRunner.Run<TrackingBenchmark>();