using BenchmarkDotNet.Attributes;

namespace CoreMathSharp.Benchmarks;

public partial class StrictMathBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Hypot")]
    public double MathHypot()
    {
        return double.Hypot(X, Y);
    }

    [Benchmark]
    [BenchmarkCategory("Hypot")]
    public double CoreHypot()
    {
        return StrictMath.Hypot(X, Y);
    }
}

public partial class StrictMathMacroBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Hypot")]
    public double MathHypot()
    {
        double sum = 0.0;

        for (int i = 0; i < X.Length; i++)
        {
            sum += double.Hypot(X[i], Y[i]);
        }

        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("Hypot")]
    public double CoreHypot()
    {
        double sum = 0.0;

        for (int i = 0; i < X.Length; i++)
        {
            sum += StrictMath.Hypot(X[i], Y[i]);
        }

        return sum;
    }
}
