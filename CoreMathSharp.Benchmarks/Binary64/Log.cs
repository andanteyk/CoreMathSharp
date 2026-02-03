using System;
using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Log")]
    public double MathLog()
    {
        return Math.Log(X);
    }

    [Benchmark]
    [BenchmarkCategory("Log")]
    public double CoreLog()
    {
        return StrictMath.Log(X);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Log")]
    public double MathLog()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += Math.Log(x);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("Log")]
    public double CoreLog()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += StrictMath.Log(x);
        }

        return sum;
    }
}
