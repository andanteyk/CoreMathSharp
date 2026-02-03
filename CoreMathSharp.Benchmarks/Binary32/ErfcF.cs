using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark]
    [BenchmarkCategory("ErfcF")]
    public float CoreErfcF()
    {
        return StrictMathF.Erfc(XF);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark]
    [BenchmarkCategory("ErfcF")]
    public float CoreErfcF()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += StrictMathF.Erfc(xf);
        }

        return sum;
    }
}
