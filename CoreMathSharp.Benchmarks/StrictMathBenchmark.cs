using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;

namespace CoreMathSharp.Benchmarks;

[MemoryDiagnoser]
[DisassemblyDiagnoser]
[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net10_0)]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public partial class StrictMathBenchmark
{
    private Seiran Rng { get; } = new(1234567890123456, 9876543210987654);

    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }

    public float XF { get; set; }
    public float YF { get; set; }
    public float ZF { get; set; }


    [GlobalSetup]
    public void GlobalSetup()
    {
        X = Rng.NextDouble();
        Y = Rng.NextDouble();
        Z = Rng.NextDouble();

        XF = Rng.NextFloat();
        YF = Rng.NextFloat();
        ZF = Rng.NextFloat();
    }
}
