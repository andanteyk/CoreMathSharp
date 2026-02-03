using System;
using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("TanF")]
    public float MathTanF()
    {
        return MathF.Tan(XF);
    }

    [Benchmark]
    [BenchmarkCategory("TanF")]
    public float CoreTanF()
    {
        return StrictMathF.Tan(XF);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("TanF")]
    public float MathTanF()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += MathF.Tan(xf);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("TanF")]
    public float CoreTanF()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += StrictMathF.Tan(xf);
        }

        return sum;
    }
}
