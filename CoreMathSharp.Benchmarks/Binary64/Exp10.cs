using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Exp10")]
    public double MathExp10()
    {
        return double.Exp10(X);
    }

    [Benchmark]
    [BenchmarkCategory("Exp10")]
    public double CoreExp10()
    {
        return StrictMath.Exp10(X);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Exp10")]
    public double MathExp10()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += double.Exp10(x);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("Exp10")]
    public double CoreExp10()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += StrictMath.Exp10(x);
        }

        return sum;
    }
}
