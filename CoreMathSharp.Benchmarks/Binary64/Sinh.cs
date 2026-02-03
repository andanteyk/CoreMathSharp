using System;
using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Sinh")]
    public double MathSinh()
    {
        return Math.Sinh(X);
    }

    [Benchmark]
    [BenchmarkCategory("Sinh")]
    public double CoreSinh()
    {
        return StrictMath.Sinh(X);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Sinh")]
    public double MathSinh()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += Math.Sinh(x);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("Sinh")]
    public double CoreSinh()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += StrictMath.Sinh(x);
        }

        return sum;
    }
}
