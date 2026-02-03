using System;
using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Cbrt")]
    public double MathCbrt()
    {
        return Math.Cbrt(X);
    }

    [Benchmark]
    [BenchmarkCategory("Cbrt")]
    public double CoreCbrt()
    {
        return StrictMath.Cbrt(X);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Cbrt")]
    public double MathCbrt()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += Math.Cbrt(x);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("Cbrt")]
    public double CoreCbrt()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += StrictMath.Cbrt(x);
        }

        return sum;
    }
}
