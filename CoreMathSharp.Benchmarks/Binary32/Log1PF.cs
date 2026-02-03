using System;
using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Log1PF")]
    public float MathLog1PF()
    {
        return MathF.Log(XF + 1.0f);
    }

    [Benchmark]
    [BenchmarkCategory("Log1PF")]
    public float CoreLog1PF()
    {
        return StrictMathF.Log1P(XF);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Log1PF")]
    public float MathLog1PF()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += MathF.Log(xf + 1.0f);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("Log1PF")]
    public float CoreLog1PF()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += StrictMathF.Log1P(xf);
        }

        return sum;
    }
}
