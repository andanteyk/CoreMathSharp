using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Log10P1")]
    public double MathLog10P1()
    {
        return double.Log10P1(X);
    }

    [Benchmark]
    [BenchmarkCategory("Log10P1")]
    public double CoreLog10P1()
    {
        return StrictMath.Log10P1(X);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Log10P1")]
    public double MathLog10P1()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += double.Log10P1(x);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("Log10P1")]
    public double CoreLog10P1()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += StrictMath.Log10P1(x);
        }

        return sum;
    }
}
