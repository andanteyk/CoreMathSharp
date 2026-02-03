using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Exp2M1F")]
    public float MathExp2M1F()
    {
        return float.Exp2M1(XF);
    }

    [Benchmark]
    [BenchmarkCategory("Exp2M1F")]
    public float CoreExp2M1F()
    {
        return StrictMathF.Exp2M1(XF);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Exp2M1F")]
    public float MathExp2M1F()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += float.Exp2M1(xf);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("Exp2M1F")]
    public float CoreExp2M1F()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += StrictMathF.Exp2M1(xf);
        }

        return sum;
    }
}
