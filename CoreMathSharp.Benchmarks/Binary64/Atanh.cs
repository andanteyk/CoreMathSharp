using System;
using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Atanh")]
    public double MathAtanh()
    {
        return Math.Atanh(X);
    }

    [Benchmark]
    [BenchmarkCategory("Atanh")]
    public double CoreAtanh()
    {
        return StrictMath.Atanh(X);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Atanh")]
    public double MathAtanh()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += Math.Atanh(x);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("Atanh")]
    public double CoreAtanh()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += StrictMath.Atanh(x);
        }

        return sum;
    }
}
