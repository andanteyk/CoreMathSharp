using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Exp2")]
    public double MathExp2()
    {
        return double.Exp2(X);
    }

    [Benchmark]
    [BenchmarkCategory("Exp2")]
    public double CoreExp2()
    {
        return StrictMath.Exp2(X);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Exp2")]
    public double MathExp2()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += double.Exp2(x);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("Exp2")]
    public double CoreExp2()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += StrictMath.Exp2(x);
        }

        return sum;
    }
}
