using System;
using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Log1P")]
    public double MathLog1P()
    {
        return Math.Log(X + 1);
    }

    [Benchmark]
    [BenchmarkCategory("Log1P")]
    public double CoreLog1P()
    {
        return StrictMath.Log1P(X);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Log1P")]
    public double MathLog1P()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += Math.Log(x + 1);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("Log1P")]
    public double CoreLog1P()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += StrictMath.Log1P(x);
        }

        return sum;
    }
}
