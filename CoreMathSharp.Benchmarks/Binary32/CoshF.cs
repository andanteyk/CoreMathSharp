using System;
using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("CoshF")]
    public float MathCoshF()
    {
        return MathF.Cosh(XF);
    }

    [Benchmark]
    [BenchmarkCategory("CoshF")]
    public float CoreCoshF()
    {
        return StrictMathF.Cosh(XF);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("CoshF")]
    public float MathCoshF()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += MathF.Cosh(xf);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("CoshF")]
    public float CoreCoshF()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += StrictMathF.Cosh(xf);
        }

        return sum;
    }
}
