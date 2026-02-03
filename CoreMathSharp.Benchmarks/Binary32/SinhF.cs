using System;
using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("SinhF")]
    public float MathSinhF()
    {
        return MathF.Sinh(XF);
    }

    [Benchmark]
    [BenchmarkCategory("SinhF")]
    public float CoreSinhF()
    {
        return StrictMathF.Sinh(XF);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("SinhF")]
    public float MathSinhF()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += MathF.Sinh(xf);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("SinhF")]
    public float CoreSinhF()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += StrictMathF.Sinh(xf);
        }

        return sum;
    }
}
