using System;
using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("AtanhF")]
    public float MathAtanhF()
    {
        return MathF.Atanh(XF);
    }

    [Benchmark]
    [BenchmarkCategory("AtanhF")]
    public float CoreAtanhF()
    {
        return StrictMathF.Atanh(XF);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("AtanhF")]
    public float MathAtanhF()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += MathF.Atanh(xf);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("AtanhF")]
    public float CoreAtanhF()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += StrictMathF.Atanh(xf);
        }

        return sum;
    }
}
