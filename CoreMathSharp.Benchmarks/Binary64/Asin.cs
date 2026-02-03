using System;
using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Asin")]
    public double MathAsin()
    {
        return Math.Asin(X);
    }

    [Benchmark]
    [BenchmarkCategory("Asin")]
    public double CoreAsin()
    {
        return StrictMath.Asin(X);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Asin")]
    public double MathAsin()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += Math.Asin(x);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("Asin")]
    public double CoreAsin()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += StrictMath.Asin(x);
        }

        return sum;
    }
}
