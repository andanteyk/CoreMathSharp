using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Atan2Pi")]
    public double MathAtan2Pi()
    {
        return double.Atan2Pi(Y, X);
    }

    [Benchmark]
    [BenchmarkCategory("Atan2Pi")]
    public double CoreAtan2Pi()
    {
        return StrictMath.Atan2Pi(Y, X);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Atan2Pi")]
    public double MathAtan2Pi()
    {
        double sum = 0.0;

        for (int i = 0; i < X.Length; i++)
        {
            sum += double.Atan2Pi(Y[i], X[i]);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("Atan2Pi")]
    public double CoreAtan2Pi()
    {
        double sum = 0.0;

        for (int i = 0; i < X.Length; i++)
        {
            sum += StrictMath.Atan2Pi(Y[i], X[i]);
        }

        return sum;
    }
}
