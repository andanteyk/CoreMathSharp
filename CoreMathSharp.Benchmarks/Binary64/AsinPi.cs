using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("AsinPi")]
    public double MathAsinPi()
    {
        return double.AsinPi(X);
    }

    [Benchmark]
    [BenchmarkCategory("AsinPi")]
    public double CoreAsinPi()
    {
        return StrictMath.AsinPi(X);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("AsinPi")]
    public double MathAsinPi()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += double.AsinPi(x);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("AsinPi")]
    public double CoreAsinPi()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += StrictMath.AsinPi(x);
        }

        return sum;
    }
}
