using System;
using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Atan2")]
    public double MathAtan2()
    {
        return Math.Atan2(Y, X);
    }

    [Benchmark]
    [BenchmarkCategory("Atan2")]
    public double CoreAtan2()
    {
        return StrictMath.Atan2(Y, X);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Atan2")]
    public double MathAtan2()
    {
        double sum = 0.0;

        for (int i = 0; i < X.Length; i++)
        {
            sum += Math.Atan2(Y[i], X[i]);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("Atan2")]
    public double CoreAtan2()
    {
        double sum = 0.0;

        for (int i = 0; i < X.Length; i++)
        {
            sum += StrictMath.Atan2(Y[i], X[i]);
        }

        return sum;
    }
}
