using System;
using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("LogF")]
    public float MathLogF()
    {
        return MathF.Log(XF);
    }

    [Benchmark]
    [BenchmarkCategory("LogF")]
    public float CoreLogF()
    {
        return StrictMathF.Log(XF);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("LogF")]
    public float MathLogF()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += MathF.Log(xf);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("LogF")]
    public float CoreLogF()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += StrictMathF.Log(xf);
        }

        return sum;
    }
}
