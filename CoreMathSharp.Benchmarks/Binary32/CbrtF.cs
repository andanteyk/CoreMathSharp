using System;
using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("CbrtF")]
    public float MathCbrtF()
    {
        return MathF.Cbrt(XF);
    }

    [Benchmark]
    [BenchmarkCategory("CbrtF")]
    public float CoreCbrtF()
    {
        return StrictMathF.Cbrt(XF);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("CbrtF")]
    public float MathCbrtF()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += MathF.Cbrt(xf);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("CbrtF")]
    public float CoreCbrtF()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += StrictMathF.Cbrt(xf);
        }

        return sum;
    }
}
