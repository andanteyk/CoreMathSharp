using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Exp2M1")]
    public double MathExp2M1()
    {
        return double.Exp2M1(X);
    }

    [Benchmark]
    [BenchmarkCategory("Exp2M1")]
    public double CoreExp2M1()
    {
        return StrictMath.Exp2M1(X);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Exp2M1")]
    public double MathExp2M1()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += double.Exp2M1(x);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("Exp2M1")]
    public double CoreExp2M1()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += StrictMath.Exp2M1(x);
        }

        return sum;
    }
}
