using System;
using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Acos")]
    public double MathAcos()
    {
        return Math.Acos(X);
    }

    [Benchmark]
    [BenchmarkCategory("Acos")]
    public double CoreAcos()
    {
        return StrictMath.Acos(X);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Acos")]
    public double MathAcos()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += Math.Acos(x);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("Acos")]
    public double CoreAcos()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += StrictMath.Acos(x);
        }

        return sum;
    }
}
