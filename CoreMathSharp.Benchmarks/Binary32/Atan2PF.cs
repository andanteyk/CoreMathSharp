using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Atan2PiF")]
    public float MathAtan2PiF()
    {
        return float.Atan2Pi(YF, XF);
    }

    [Benchmark]
    [BenchmarkCategory("Atan2PiF")]
    public float CoreAtan2PiF()
    {
        return StrictMathF.Atan2Pi(YF, XF);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Atan2PiF")]
    public float MathAtan2PiF()
    {
        float sum = 0.0f;

        for (int i = 0; i < XF.Length; i++)
        {
            sum += float.Atan2Pi(YF[i], XF[i]);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("Atan2PiF")]
    public float CoreAtan2PiF()
    {
        float sum = 0.0f;

        for (int i = 0; i < XF.Length; i++)
        {
            sum += StrictMathF.Atan2Pi(YF[i], XF[i]);
        }

        return sum;
    }
}
