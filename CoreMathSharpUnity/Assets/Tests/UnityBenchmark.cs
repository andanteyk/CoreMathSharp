using NUnit.Framework;
using System;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    private Seiran Rng { get; } = new(1234567890123456, 9876543210987654);

    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }

    public float XF { get; set; }
    public float YF { get; set; }
    public float ZF { get; set; }

    public double Result = 0.0;
    public float ResultF = 0.0f;

    [OneTimeSetUp]
    public void Setup()
    {
        X = Rng.NextDouble();
        Y = Rng.NextDouble();
        Z = Rng.NextDouble();

        XF = Rng.NextFloat();
        YF = Rng.NextFloat();
        ZF = Rng.NextFloat();
    }

    public void MeasurePerformance(string name, Action action)
    {
        Measure.Method(action)
            .WarmupCount(10)
            .MeasurementCount(100)
            .IterationsPerMeasurement(5000)
            .SampleGroup(name)
            .Run();
    }
}
