using System;
using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("PowF")]
    public float MathPowF()
    {
        return MathF.Pow(XF, YF);
    }

    [Benchmark]
    [BenchmarkCategory("PowF")]
    public float CorePowF()
    {
        return StrictMathF.Pow(XF, YF);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("PowF")]
    public float MathPowF()
    {
        float sum = 0.0f;

        for (int i = 0; i < XF.Length; i++)
        {
            sum += MathF.Pow(XF[i], YF[i]);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("PowF")]
    public float CorePowF()
    {
        float sum = 0.0f;

        for (int i = 0; i < XF.Length; i++)
        {
            sum += StrictMathF.Pow(XF[i], YF[i]);
        }

        return sum;
    }
}
