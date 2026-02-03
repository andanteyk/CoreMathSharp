using System;
using BenchmarkDotNet.Running;
using CoreMathSharp.Benchmarks;


BenchmarkRunner.Run<StrictMathBenchmark>(args: Environment.GetCommandLineArgs());

BenchmarkRunner.Run<StrictMathMacroBenchmark>(args: Environment.GetCommandLineArgs());



/*
void ParseLiteral()
{
    while (true)
    {
        Console.WriteLine("input hex fp literal(s) (Ctrl-c to terminate):");
        string line = Console.ReadLine() ?? "";
        Console.WriteLine(string.Join(", ", line.Split(",").Select(element => $"{StrictMath.ParseHex(element.Trim()):g17}")));

        if (line.Split(",").Any(element => StrictMath.ParseHex(element.Trim()) != double.Parse($"{StrictMath.ParseHex(element.Trim()):g17}")))
        {
            Console.WriteLine("ROUNDTRIP ERROR!!!!!");
            return;
        }

        Console.WriteLine();
    }
}
//*/
