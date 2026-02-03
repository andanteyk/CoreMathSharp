using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("AtanPi")]
    public double MathAtanPi()
    {
        return double.AtanPi(X);
    }

    [Benchmark]
    [BenchmarkCategory("AtanPi")]
    public double CoreAtanPi()
    {
        return StrictMath.AtanPi(X);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("AtanPi")]
    public double MathAtanPi()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += double.AtanPi(x);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("AtanPi")]
    public double CoreAtanPi()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += StrictMath.AtanPi(x);
        }

        return sum;
    }
}
