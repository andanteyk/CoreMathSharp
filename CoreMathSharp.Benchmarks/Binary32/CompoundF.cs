using System;
using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("CompoundF")]
    public float MathCompoundF()
    {
        return MathF.Pow(1.0f + XF, YF);
    }

    [Benchmark]
    [BenchmarkCategory("CompoundF")]
    public float CoreCompoundF()
    {
        return StrictMathF.Compound(XF, YF);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("CompoundF")]
    public float MathCompoundF()
    {
        float sum = 0.0f;

        for (int i = 0; i < XF.Length; i++)
        {
            sum += MathF.Pow(1.0f + XF[i], YF[i]);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("CompoundF")]
    public float CoreCompoundF()
    {
        float sum = 0.0f;

        for (int i = 0; i < XF.Length; i++)
        {
            sum += StrictMathF.Compound(XF[i], YF[i]);
        }

        return sum;
    }
}
