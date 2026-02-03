using System;
using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Sin")]
    public double MathSin()
    {
        return Math.Sin(X);
    }

    [Benchmark]
    [BenchmarkCategory("Sin")]
    public double CoreSin()
    {
        return StrictMath.Sin(X);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Sin")]
    public double MathSin()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += Math.Sin(x);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("Sin")]
    public double CoreSin()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += StrictMath.Sin(x);
        }

        return sum;
    }
}
