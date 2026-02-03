using System;
using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Log2")]
    public double MathLog2()
    {
        return Math.Log2(X);
    }

    [Benchmark]
    [BenchmarkCategory("Log2")]
    public double CoreLog2()
    {
        return StrictMath.Log2(X);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Log2")]
    public double MathLog2()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += Math.Log2(x);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("Log2")]
    public double CoreLog2()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += StrictMath.Log2(x);
        }

        return sum;
    }
}
