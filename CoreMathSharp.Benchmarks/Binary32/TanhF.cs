using System;
using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("TanhF")]
    public float MathTanhF()
    {
        return MathF.Tanh(XF);
    }

    [Benchmark]
    [BenchmarkCategory("TanhF")]
    public float CoreTanhF()
    {
        return StrictMathF.Tanh(XF);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("TanhF")]
    public float MathTanhF()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += MathF.Tanh(xf);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("TanhF")]
    public float CoreTanhF()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += StrictMathF.Tanh(xf);
        }

        return sum;
    }
}
