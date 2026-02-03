using System;
using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Tan")]
    public double MathTan()
    {
        return Math.Tan(X);
    }

    [Benchmark]
    [BenchmarkCategory("Tan")]
    public double CoreTan()
    {
        return StrictMath.Tan(X);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Tan")]
    public double MathTan()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += Math.Tan(x);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("Tan")]
    public double CoreTan()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += StrictMath.Tan(x);
        }

        return sum;
    }
}
