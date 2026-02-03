using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Exp2F")]
    public float MathExp2F()
    {
        return float.Exp2(XF);
    }

    [Benchmark]
    [BenchmarkCategory("Exp2F")]
    public float CoreExp2F()
    {
        return StrictMathF.Exp2(XF);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Exp2F")]
    public float MathExp2F()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += float.Exp2(xf);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("Exp2F")]
    public float CoreExp2F()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += StrictMathF.Exp2(xf);
        }

        return sum;
    }
}
