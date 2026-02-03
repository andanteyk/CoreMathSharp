using System;
using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("AsinF")]
    public float MathAsinF()
    {
        return MathF.Asin(XF);
    }

    [Benchmark]
    [BenchmarkCategory("AsinF")]
    public float CoreAsinF()
    {
        return StrictMathF.Asin(XF);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("AsinF")]
    public float MathAsinF()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += MathF.Asin(xf);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("AsinF")]
    public float CoreAsinF()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += StrictMathF.Asin(xf);
        }

        return sum;
    }
}
