using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("CosPi")]
    public double MathCosPi()
    {
        return double.CosPi(X);
    }

    [Benchmark]
    [BenchmarkCategory("CosPi")]
    public double CoreCosPi()
    {
        return StrictMath.CosPi(X);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("CosPi")]
    public double MathCosPi()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += double.CosPi(x);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("CosPi")]
    public double CoreCosPi()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += StrictMath.CosPi(x);
        }

        return sum;
    }
}
