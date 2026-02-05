using System;
using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("FusedMultiplyAddF")]
    public float MathFusedMultiplyAddF()
    {
        return MathF.FusedMultiplyAdd(XF, YF, ZF);
    }

    [Benchmark]
    [BenchmarkCategory("FusedMultiplyAddF")]
    public float CoreFusedMultiplyAddF()
    {
        return StrictMathF.FusedMultiplyAdd(XF, YF, ZF);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("FusedMultiplyAddF")]
    public float MathFusedMultiplyAddF()
    {
        float sum = 0.0f;

        for (int i = 0; i < XF.Length; i++)
        {
            sum += MathF.FusedMultiplyAdd(XF[i], YF[i], ZF[i]);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("FusedMultiplyAddF")]
    public float CoreFusedMultiplyAddF()
    {
        float sum = 0.0f;

        for (int i = 0; i < XF.Length; i++)
        {
            sum += StrictMathF.FusedMultiplyAdd(XF[i], YF[i], ZF[i]);
        }

        return sum;
    }
}
