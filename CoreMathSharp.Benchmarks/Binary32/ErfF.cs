using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark]
    [BenchmarkCategory("ErfF")]
    public float CoreErfF()
    {
        return StrictMathF.Erf(XF);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark]
    [BenchmarkCategory("ErfF")]
    public float CoreErfF()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += StrictMathF.Erf(xf);
        }

        return sum;
    }
}
