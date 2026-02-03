using System;
using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Tanh")]
    public double MathTanh()
    {
        return Math.Tanh(X);
    }

    [Benchmark]
    [BenchmarkCategory("Tanh")]
    public double CoreTanh()
    {
        return StrictMath.Tanh(X);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Tanh")]
    public double MathTanh()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += Math.Tanh(x);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("Tanh")]
    public double CoreTanh()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += StrictMath.Tanh(x);
        }

        return sum;
    }
}
