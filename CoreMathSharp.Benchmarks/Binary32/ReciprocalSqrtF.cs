using System;
using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("ReciprocalSqrtF")]
    public float MathReciprocalSqrtF()
    {
        return 1.0f / MathF.Sqrt(XF);
    }

    [BenchmarkCategory("ReciprocalSqrtF")]
    public float MathReciprocalSqrtEstimateF()
    {
        return MathF.ReciprocalSqrtEstimate(XF);
    }

    [Benchmark]
    [BenchmarkCategory("ReciprocalSqrtF")]
    public float CoreReciprocalSqrtF()
    {
        return StrictMathF.ReciprocalSqrt(XF);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("ReciprocalSqrtF")]
    public float MathReciprocalSqrtF()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += 1.0f / MathF.Sqrt(xf);
        }

        return sum;
    }

    [BenchmarkCategory("ReciprocalSqrtF")]
    public float MathReciprocalSqrtEstimateF()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += MathF.ReciprocalSqrtEstimate(xf);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("ReciprocalSqrtF")]
    public float CoreReciprocalSqrtF()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += StrictMathF.ReciprocalSqrt(xf);
        }

        return sum;
    }
}
