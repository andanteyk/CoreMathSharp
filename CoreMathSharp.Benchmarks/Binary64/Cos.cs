using System;
using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Cos")]
    public double MathCos()
    {
        return Math.Cos(X);
    }

    [Benchmark]
    [BenchmarkCategory("Cos")]
    public double CoreCos()
    {
        return StrictMath.Cos(X);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Cos")]
    public double MathCos()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += Math.Cos(x);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("Cos")]
    public double CoreCos()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += StrictMath.Cos(x);
        }

        return sum;
    }
}
