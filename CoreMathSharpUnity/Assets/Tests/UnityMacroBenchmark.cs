using NUnit.Framework;
using System;
using System.Linq;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityMacroBenchmark
{
    private Seiran Rng { get; } = new(1234567890123456, 9876543210987654);

    public double[] X { get; set; } = { };
    public double[] Y { get; set; } = { };
    public double[] Z { get; set; } = { };

    public float[] XF { get; set; } = { };
    public float[] YF { get; set; } = { };
    public float[] ZF { get; set; } = { };

    public double Result = 0.0;
    public float ResultF = 0.0f;

    [OneTimeSetUp]
    public void Setup()
    {
        const int Iterations = 5000;

        X = Enumerable.Range(0, Iterations).Select(_ => Rng.NextDouble()).ToArray();
        Y = Enumerable.Range(0, Iterations).Select(_ => Rng.NextDouble()).ToArray();
        Z = Enumerable.Range(0, Iterations).Select(_ => Rng.NextDouble()).ToArray();

        XF = Enumerable.Range(0, Iterations).Select(_ => Rng.NextFloat()).ToArray();
        YF = Enumerable.Range(0, Iterations).Select(_ => Rng.NextFloat()).ToArray();
        ZF = Enumerable.Range(0, Iterations).Select(_ => Rng.NextFloat()).ToArray();
    }

    public void MeasurePerformance(string name, Action action)
    {
        Measure.Method(action)
            .WarmupCount(10)
            .MeasurementCount(100)
            .IterationsPerMeasurement(16)
            .SampleGroup(name)
            .Run();
    }

    [Test, Performance]
    public void Baseline()
    {
        MeasurePerformance("Baseline", () =>
        {
            double sum = 0.0;

            foreach (var x in X)
            {
                sum += x;
            }

            Result = sum;
        });
    }

    [Test, Performance]
    public void BaselineF()
    {
        MeasurePerformance("BaselineF", () =>
        {
            float sum = 0.0f;

            foreach (var xf in XF)
            {
                sum += xf;
            }

            ResultF = sum;
        });
    }

}
