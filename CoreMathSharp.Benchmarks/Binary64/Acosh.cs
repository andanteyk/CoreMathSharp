using System;
using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Acosh")]
    public double MathAcosh()
    {
        return Math.Acosh(X);
    }

    [Benchmark]
    [BenchmarkCategory("Acosh")]
    public double CoreAcosh()
    {
        return StrictMath.Acosh(X);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Acosh")]
    public double MathAcosh()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += Math.Acosh(x);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("Acosh")]
    public double CoreAcosh()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += StrictMath.Acosh(x);
        }

        return sum;
    }
}
