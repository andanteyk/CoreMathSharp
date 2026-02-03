using System;
using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("SinCosF")]
    public (float sin, float cos) MathSinCosF()
    {
        return MathF.SinCos(XF);
    }

    [Benchmark]
    [BenchmarkCategory("SinCosF")]
    public (float sin, float cos) CoreSinCosF()
    {
        return StrictMathF.SinCos(XF);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("SinCosF")]
    public (float sin, float cos) MathSinCosF()
    {
        float sinsum = 0.0f;
        float cossum = 0.0f;

        foreach (var xf in XF)
        {
            var (sin, cos) = MathF.SinCos(xf);
            sinsum += sin;
            cossum += cos;
        }

        return (sinsum, cossum);
    }

    [Benchmark]
    [BenchmarkCategory("SinCosF")]
    public (float sin, float cos) CoreSinCosF()
    {
        float sinsum = 0.0f;
        float cossum = 0.0f;

        foreach (var xf in XF)
        {
            var (sin, cos) = StrictMathF.SinCos(xf);
            sinsum += sin;
            cossum += cos;
        }

        return (sinsum, cossum);
    }
}
