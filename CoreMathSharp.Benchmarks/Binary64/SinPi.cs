using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("SinPi")]
    public double MathSinPi()
    {
        return double.SinPi(X);
    }

    [Benchmark]
    [BenchmarkCategory("SinPi")]
    public double CoreSinPi()
    {
        return StrictMath.SinPi(X);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("SinPi")]
    public double MathSinPi()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += double.SinPi(x);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("SinPi")]
    public double CoreSinPi()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += StrictMath.SinPi(x);
        }

        return sum;
    }
}
