using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark]
    [BenchmarkCategory("Erf")]
    public double CoreErf()
    {
        return StrictMath.Erf(X);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark]
    [BenchmarkCategory("Erf")]
    public double CoreErf()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += StrictMath.Erf(x);
        }

        return sum;
    }
}
