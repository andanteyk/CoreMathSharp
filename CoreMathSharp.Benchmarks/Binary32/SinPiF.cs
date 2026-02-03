using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("SinPiF")]
    public float MathSinPiF()
    {
        return float.SinPi(XF);
    }

    [Benchmark]
    [BenchmarkCategory("SinPiF")]
    public float CoreSinPiF()
    {
        return StrictMathF.SinPi(XF);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("SinPiF")]
    public float MathSinPiF()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += float.SinPi(xf);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("SinPiF")]
    public float CoreSinPiF()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += StrictMathF.SinPi(xf);
        }

        return sum;
    }
}
