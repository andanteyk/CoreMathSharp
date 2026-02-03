using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("HypotF")]
    public float MathHypotF()
    {
        return float.Hypot(XF, YF);
    }

    [Benchmark]
    [BenchmarkCategory("HypotF")]
    public float CoreHypotF()
    {
        return StrictMathF.Hypot(XF, YF);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("HypotF")]
    public float MathHypotF()
    {
        float sum = 0.0f;

        for (int i = 0; i < XF.Length; i++)
        {
            sum += float.Hypot(XF[i], YF[i]);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("HypotF")]
    public float CoreHypotF()
    {
        float sum = 0.0f;

        for (int i = 0; i < XF.Length; i++)
        {
            sum += StrictMathF.Hypot(XF[i], YF[i]);
        }

        return sum;
    }
}
