using System;
using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("SqrtF")]
    public float MathSqrtF()
    {
        return MathF.Sqrt(XF);
    }

    [Benchmark]
    [BenchmarkCategory("SqrtF")]
    public float CoreSqrtF()
    {
        return StrictMathF.Sqrt(XF);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("SqrtF")]
    public float MathSqrtF()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += MathF.Sqrt(xf);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("SqrtF")]
    public float CoreSqrtF()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += StrictMathF.Sqrt(xf);
        }

        return sum;
    }
}
