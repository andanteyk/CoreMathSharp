using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark]
    [BenchmarkCategory("LGamma")]
    public double CoreLGamma()
    {
        return StrictMath.LGamma(X).value;
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark]
    [BenchmarkCategory("LGamma")]
    public double CoreLGamma()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += StrictMath.LGamma(x).value;
        }

        return sum;
    }
}
