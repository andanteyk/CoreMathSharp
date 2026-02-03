using System;
using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Atan")]
    public double MathAtan()
    {
        return Math.Atan(X);
    }

    [Benchmark]
    [BenchmarkCategory("Atan")]
    public double CoreAtan()
    {
        return StrictMath.Atan(X);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Atan")]
    public double MathAtan()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += Math.Atan(x);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("Atan")]
    public double CoreAtan()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += StrictMath.Atan(x);
        }

        return sum;
    }
}
