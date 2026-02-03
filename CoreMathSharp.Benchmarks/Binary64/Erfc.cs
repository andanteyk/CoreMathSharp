using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark]
    [BenchmarkCategory("Erfc")]
    public double CoreErfc()
    {
        return StrictMath.Erfc(X);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark]
    [BenchmarkCategory("Erfc")]
    public double CoreErfc()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += StrictMath.Erfc(x);
        }

        return sum;
    }
}
