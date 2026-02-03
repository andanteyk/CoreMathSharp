using System;
using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("FusedMultiplyAdd")]
    public double MathFusedMultiplyAdd()
    {
        return Math.FusedMultiplyAdd(X, Y, Z);
    }

    [Benchmark]
    [BenchmarkCategory("FusedMultiplyAdd")]
    public double CoreFusedMultiplyAdd()
    {
        return StrictMath.FusedMultiplyAdd(X, Y, Z);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("FusedMultiplyAdd")]
    public double MathFusedMultiplyAdd()
    {
        double sum = 0.0;

        for (int i = 0; i < X.Length; i++)
        {
            sum += Math.FusedMultiplyAdd(X[i], Y[i], Z[i]);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("FusedMultiplyAdd")]
    public double CoreFusedMultiplyAdd()
    {
        double sum = 0.0;

        for (int i = 0; i < X.Length; i++)
        {
            sum += StrictMath.FusedMultiplyAdd(X[i], Y[i], Z[i]);
        }

        return sum;
    }
}
