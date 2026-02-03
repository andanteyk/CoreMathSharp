using System;
using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Exp")]
    public double MathExp()
    {
        return Math.Exp(X);
    }

    [Benchmark]
    [BenchmarkCategory("Exp")]
    public double CoreExp()
    {
        return StrictMath.Exp(X);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Exp")]
    public double MathExp()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += Math.Exp(x);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("Exp")]
    public double CoreExp()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += StrictMath.Exp(x);
        }

        return sum;
    }
}
