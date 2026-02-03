using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark]
    [BenchmarkCategory("TGamma")]
    public double CoreTGamma()
    {
        return StrictMath.TGamma(X);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark]
    [BenchmarkCategory("TGamma")]
    public double CoreTGamma()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += StrictMath.TGamma(x);
        }

        return sum;
    }
}
