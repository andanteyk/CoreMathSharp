using System;
using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Log10")]
    public double MathLog10()
    {
        return Math.Log10(X);
    }

    [Benchmark]
    [BenchmarkCategory("Log10")]
    public double CoreLog10()
    {
        return StrictMath.Log10(X);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Log10")]
    public double MathLog10()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += Math.Log10(x);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("Log10")]
    public double CoreLog10()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += StrictMath.Log10(x);
        }

        return sum;
    }
}
