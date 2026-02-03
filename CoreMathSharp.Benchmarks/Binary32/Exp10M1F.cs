using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Exp10M1F")]
    public float MathExp10M1F()
    {
        return float.Exp10M1(XF);
    }

    [Benchmark]
    [BenchmarkCategory("Exp10M1F")]
    public float CoreExp10M1F()
    {
        return StrictMathF.Exp10M1(XF);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Exp10M1F")]
    public float MathExp10M1F()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += float.Exp10M1(xf);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("Exp10M1F")]
    public float CoreExp10M1F()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += StrictMathF.Exp10M1(xf);
        }

        return sum;
    }
}
