using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("TanPiF")]
    public float MathTanPiF()
    {
        return float.TanPi(XF);
    }

    [Benchmark]
    [BenchmarkCategory("TanPiF")]
    public float CoreTanPiF()
    {
        return StrictMathF.TanPi(XF);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("TanPiF")]
    public float MathTanPiF()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += float.TanPi(xf);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("TanPiF")]
    public float CoreTanPiF()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += StrictMathF.TanPi(xf);
        }

        return sum;
    }
}
