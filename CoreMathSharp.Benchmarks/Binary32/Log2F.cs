using System;
using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Log2F")]
    public float MathLog2F()
    {
        return MathF.Log2(XF);
    }

    [Benchmark]
    [BenchmarkCategory("Log2F")]
    public float CoreLog2F()
    {
        return StrictMathF.Log2(XF);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Log2F")]
    public float MathLog2F()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += MathF.Log2(xf);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("Log2F")]
    public float CoreLog2F()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += StrictMathF.Log2(xf);
        }

        return sum;
    }
}
