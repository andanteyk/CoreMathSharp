using System;
using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("AsinhF")]
    public float MathAsinhF()
    {
        return MathF.Asinh(XF);
    }

    [Benchmark]
    [BenchmarkCategory("AsinhF")]
    public float CoreAsinhF()
    {
        return StrictMathF.Asinh(XF);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("AsinhF")]
    public float MathAsinhF()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += MathF.Asinh(xf);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("AsinhF")]
    public float CoreAsinhF()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += StrictMathF.Asinh(xf);
        }

        return sum;
    }
}
