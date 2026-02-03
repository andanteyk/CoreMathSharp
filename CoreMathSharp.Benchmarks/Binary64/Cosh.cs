using System;
using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Cosh")]
    public double MathCosh()
    {
        return Math.Cosh(X);
    }

    [Benchmark]
    [BenchmarkCategory("Cosh")]
    public double CoreCosh()
    {
        return StrictMath.Cosh(X);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Cosh")]
    public double MathCosh()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += Math.Cosh(x);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("Cosh")]
    public double CoreCosh()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += StrictMath.Cosh(x);
        }

        return sum;
    }
}
