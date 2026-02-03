using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Log2P1")]
    public double MathLog2P1()
    {
        return double.Log2P1(X);
    }

    [Benchmark]
    [BenchmarkCategory("Log2P1")]
    public double CoreLog2P1()
    {
        return StrictMath.Log2P1(X);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Log2P1")]
    public double MathLog2P1()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += double.Log2P1(x);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("Log2P1")]
    public double CoreLog2P1()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += StrictMath.Log2P1(x);
        }

        return sum;
    }
}
