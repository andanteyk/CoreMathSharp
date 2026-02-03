using System;
using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("ExpF")]
    public float MathExpF()
    {
        return MathF.Exp(XF);
    }

    [Benchmark]
    [BenchmarkCategory("ExpF")]
    public float CoreExpF()
    {
        return StrictMathF.Exp(XF);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("ExpF")]
    public float MathExpF()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += MathF.Exp(xf);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("ExpF")]
    public float CoreExpF()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += StrictMathF.Exp(xf);
        }

        return sum;
    }
}
