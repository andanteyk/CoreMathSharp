using System;
using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("ReciprocalSqrt")]
    public double MathReciprocalSqrt()
    {
        return 1.0 / Math.Sqrt(X);
    }

    [Benchmark]
    [BenchmarkCategory("ReciprocalSqrt")]
    public double MathReciprocalSqrtEstimate()
    {
        return Math.ReciprocalSqrtEstimate(X);
    }

    [Benchmark]
    [BenchmarkCategory("ReciprocalSqrt")]
    public double CoreReciprocalSqrt()
    {
        return StrictMath.ReciprocalSqrt(X);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("ReciprocalSqrt")]
    public double MathReciprocalSqrt()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += 1.0 / Math.Sqrt(x);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("ReciprocalSqrt")]
    public double MathReciprocalSqrtEstimate()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += Math.ReciprocalSqrtEstimate(x);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("ReciprocalSqrt")]
    public double CoreReciprocalSqrt()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += StrictMath.ReciprocalSqrt(x);
        }

        return sum;
    }
}
