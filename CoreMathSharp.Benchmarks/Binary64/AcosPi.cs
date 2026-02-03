using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("AcosPi")]
    public double MathAcosPi()
    {
        return double.AcosPi(X);
    }

    [Benchmark]
    [BenchmarkCategory("AcosPi")]
    public double CoreAcosPi()
    {
        return StrictMath.AcosPi(X);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("AcosPi")]
    public double MathAcosPi()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += double.AcosPi(x);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("AcosPi")]
    public double CoreAcosPi()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += StrictMath.AcosPi(x);
        }

        return sum;
    }
}
