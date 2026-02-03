using System;
using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("AcoshF")]
    public float MathAcoshF()
    {
        return MathF.Acosh(XF);
    }

    [Benchmark]
    [BenchmarkCategory("AcoshF")]
    public float CoreAcoshF()
    {
        return StrictMathF.Acosh(XF);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("AcoshF")]
    public float MathAcoshF()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += MathF.Acosh(xf);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("AcoshF")]
    public float CoreAcoshF()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += StrictMathF.Acosh(xf);
        }

        return sum;
    }
}
