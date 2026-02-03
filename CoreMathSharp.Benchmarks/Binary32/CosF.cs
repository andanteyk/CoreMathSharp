using System;
using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("CosF")]
    public float MathCosF()
    {
        return MathF.Cos(XF);
    }

    [Benchmark]
    [BenchmarkCategory("CosF")]
    public float CoreCosF()
    {
        return StrictMathF.Cos(XF);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("CosF")]
    public float MathCosF()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += MathF.Cos(xf);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("CosF")]
    public float CoreCosF()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += StrictMathF.Cos(xf);
        }

        return sum;
    }
}
