using System;
using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Sqrt")]
    public double MathSqrt()
    {
        return Math.Sqrt(X);
    }

    [Benchmark]
    [BenchmarkCategory("Sqrt")]
    public double CoreSqrt()
    {
        return StrictMath.Sqrt(X);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Sqrt")]
    public double MathSqrt()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += Math.Sqrt(x);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("Sqrt")]
    public double CoreSqrt()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += StrictMath.Sqrt(x);
        }

        return sum;
    }
}
