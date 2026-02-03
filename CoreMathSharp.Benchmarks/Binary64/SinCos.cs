using System;
using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("SinCos")]
    public (double sin, double cos) MathSinCos()
    {
        return Math.SinCos(X);
    }

    [Benchmark]
    [BenchmarkCategory("SinCos")]
    public (double sin, double cos) CoreSinCos()
    {
        return StrictMath.SinCos(X);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("SinCos")]
    public (double sin, double cos) MathSinCos()
    {
        double sinsum = 0.0;
        double cossum = 0.0;

        foreach (var x in X)
        {
            var (sin, cos) = Math.SinCos(x);
            sinsum += sin;
            cossum += cos;
        }

        return (sinsum, cossum);
    }

    [Benchmark]
    [BenchmarkCategory("SinCos")]
    public (double sin, double cos) CoreSinCos()
    {
        double sinsum = 0.0;
        double cossum = 0.0;

        foreach (var x in X)
        {
            var (sin, cos) = StrictMath.SinCos(x);
            sinsum += sin;
            cossum += cos;
        }

        return (sinsum, cossum);
    }
}
