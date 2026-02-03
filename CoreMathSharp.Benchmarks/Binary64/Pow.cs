using System;
using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Pow")]
    public double MathPow()
    {
        return Math.Pow(X, Y);
    }

    [Benchmark]
    [BenchmarkCategory("Pow")]
    public double CorePow()
    {
        return StrictMath.Pow(X, Y);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Pow")]
    public double MathPow()
    {
        double sum = 0.0;

        for (int i = 0; i < X.Length; i++)
        {
            sum += Math.Pow(X[i], Y[i]);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("Pow")]
    public double CorePow()
    {
        double sum = 0.0;

        for (int i = 0; i < X.Length; i++)
        {
            sum += StrictMath.Pow(X[i], Y[i]);
        }

        return sum;
    }
}
