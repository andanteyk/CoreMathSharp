using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Exp10F")]
    public float MathExp10F()
    {
        return float.Exp10(XF);
    }

    [Benchmark]
    [BenchmarkCategory("Exp10F")]
    public float CoreExp10F()
    {
        return StrictMathF.Exp10(XF);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Exp10F")]
    public float MathExp10F()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += float.Exp10(xf);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("Exp10F")]
    public float CoreExp10F()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += StrictMathF.Exp10(xf);
        }

        return sum;
    }
}
