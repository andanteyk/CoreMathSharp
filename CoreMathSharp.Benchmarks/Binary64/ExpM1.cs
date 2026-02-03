using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("ExpM1")]
    public double MathExpM1()
    {
        return double.ExpM1(X);
    }

    [Benchmark]
    [BenchmarkCategory("ExpM1")]
    public double CoreExpM1()
    {
        return StrictMath.ExpM1(X);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("ExpM1")]
    public double MathExpM1()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += double.ExpM1(x);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("ExpM1")]
    public double CoreExpM1()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += StrictMath.ExpM1(x);
        }

        return sum;
    }
}
