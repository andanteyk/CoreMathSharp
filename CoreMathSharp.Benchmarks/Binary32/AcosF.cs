using System;
using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("AcosF")]
    public float MathAcosF()
    {
        return MathF.Acos(XF);
    }

    [Benchmark]
    [BenchmarkCategory("AcosF")]
    public float CoreAcosF()
    {
        return StrictMathF.Acos(XF);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("AcosF")]
    public float MathAcosF()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += MathF.Acos(xf);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("AcosF")]
    public float CoreAcosF()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += StrictMathF.Acos(xf);
        }

        return sum;
    }
}
