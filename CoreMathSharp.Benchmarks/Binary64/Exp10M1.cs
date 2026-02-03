using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Exp10M1")]
    public double MathExp10M1()
    {
        return double.Exp10M1(X);
    }

    [Benchmark]
    [BenchmarkCategory("Exp10M1")]
    public double CoreExp10M1()
    {
        return StrictMath.Exp10M1(X);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Exp10M1")]
    public double MathExp10M1()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += double.Exp10M1(x);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("Exp10M1")]
    public double CoreExp10M1()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += StrictMath.Exp10M1(x);
        }

        return sum;
    }
}
