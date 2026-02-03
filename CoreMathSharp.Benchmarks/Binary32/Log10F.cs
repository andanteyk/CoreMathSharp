using System;
using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Log10F")]
    public float MathLog10F()
    {
        return MathF.Log10(XF);
    }

    [Benchmark]
    [BenchmarkCategory("Log10F")]
    public float CoreLog10F()
    {
        return StrictMathF.Log10(XF);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Log10F")]
    public float MathLog10F()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += MathF.Log10(xf);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("Log10F")]
    public float CoreLog10F()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += StrictMathF.Log10(xf);
        }

        return sum;
    }
}
