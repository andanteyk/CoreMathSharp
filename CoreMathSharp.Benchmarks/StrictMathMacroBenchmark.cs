using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;

namespace CoreMathSharp.Benchmarks;

[MemoryDiagnoser]
[DisassemblyDiagnoser]
[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net10_0)]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public partial class StrictMathMacroBenchmark
{
    private Seiran Rng { get; } = new(1234567890123456, 9876543210987654);

    public double[] X { get; set; } = [];
    public double[] Y { get; set; } = [];
    public double[] Z { get; set; } = [];

    public float[] XF { get; set; } = [];
    public float[] YF { get; set; } = [];
    public float[] ZF { get; set; } = [];

    [GlobalSetup]
    public void GlobalSetup()
    {
        const int Iterations = 5000;

        X = Enumerable.Range(0, Iterations).Select(_ => Rng.NextDouble()).ToArray();
        Y = Enumerable.Range(0, Iterations).Select(_ => Rng.NextDouble()).ToArray();
        Z = Enumerable.Range(0, Iterations).Select(_ => Rng.NextDouble()).ToArray();

        XF = Enumerable.Range(0, Iterations).Select(_ => Rng.NextFloat()).ToArray();
        YF = Enumerable.Range(0, Iterations).Select(_ => Rng.NextFloat()).ToArray();
        ZF = Enumerable.Range(0, Iterations).Select(_ => Rng.NextFloat()).ToArray();
    }

    [Benchmark]
    public double Baseline()
    {
        double sum = 0.0;

        foreach (var x in X)
        {
            sum += x;
        }

        return sum;
    }

    [Benchmark]
    public float BaselineF()
    {
        float sum = 0.0f;

        foreach (var xf in XF)
        {
            sum += xf;
        }

        return sum;
    }
}
