using System;
using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("SinF")]
    public float MathSinF()
    {
        return MathF.Sin(XF);
    }

    [Benchmark]
    [BenchmarkCategory("SinF")]
    public float CoreSinF()
    {
        return StrictMathF.Sin(XF);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("SinF")]
    public float MathSinF()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += MathF.Sin(xf);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("SinF")]
    public float CoreSinF()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += StrictMathF.Sin(xf);
        }

        return sum;
    }
}
