using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("TanPi")]
    public double MathTanPi()
    {
        return double.TanPi(X);
    }

    [Benchmark]
    [BenchmarkCategory("TanPi")]
    public double CoreTanPi()
    {
        return StrictMath.TanPi(X);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("TanPi")]
    public double MathTanPi()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += double.TanPi(x);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("TanPi")]
    public double CoreTanPi()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += StrictMath.TanPi(x);
        }

        return sum;
    }
}
