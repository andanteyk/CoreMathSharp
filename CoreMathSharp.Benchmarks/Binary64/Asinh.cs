using System;
using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Asinh")]
    public double MathAsinh()
    {
        return Math.Asinh(X);
    }

    [Benchmark]
    [BenchmarkCategory("Asinh")]
    public double CoreAsinh()
    {
        return StrictMath.Asinh(X);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Asinh")]
    public double MathAsinh()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += Math.Asinh(x);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("Asinh")]
    public double CoreAsinh()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += StrictMath.Asinh(x);
        }

        return sum;
    }
}
