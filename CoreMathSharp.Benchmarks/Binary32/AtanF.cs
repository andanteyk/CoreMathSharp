using System;
using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("AtanF")]
    public float MathAtanF()
    {
        return MathF.Atan(XF);
    }

    [Benchmark]
    [BenchmarkCategory("AtanF")]
    public float CoreAtanF()
    {
        return StrictMathF.Atan(XF);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("AtanF")]
    public float MathAtanF()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += MathF.Atan(xf);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("AtanF")]
    public float CoreAtanF()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += StrictMathF.Atan(xf);
        }

        return sum;
    }
}
