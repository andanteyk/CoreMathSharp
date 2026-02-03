using System;
using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Atan2F")]
    public float MathAtan2F()
    {
        return MathF.Atan2(YF, XF);
    }

    [Benchmark]
    [BenchmarkCategory("Atan2F")]
    public float CoreAtan2F()
    {
        return StrictMathF.Atan2(YF, XF);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Atan2F")]
    public float MathAtan2F()
    {
        float sum = 0.0f;

        for (int i = 0; i < XF.Length; i++)
        {
            sum += MathF.Atan2(YF[i], XF[i]);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("Atan2F")]
    public float CoreAtan2F()
    {
        float sum = 0.0f;

        for (int i = 0; i < XF.Length; i++)
        {
            sum += StrictMathF.Atan2(YF[i], XF[i]);
        }

        return sum;
    }
}
