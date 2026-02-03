using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("FusedMultiplyAdd")]
    public void BurstLowFusedMultiplyAdd()
    {
        MeasurePerformance("FusedMultiplyAdd", () =>
        {
            Result = BurstMath.FusedMultiplyAddLow(X, Y, Z);
        });
    }

    [Test, Performance]
    [Category("FusedMultiplyAdd")]
    public void BurstMediumFusedMultiplyAdd()
    {
        MeasurePerformance("FusedMultiplyAdd", () =>
        {
            Result = BurstMath.FusedMultiplyAddMedium(X, Y, Z);
        });
    }

    [Test, Performance]
    [Category("FusedMultiplyAdd")]
    public void BurstHighFusedMultiplyAdd()
    {
        MeasurePerformance("FusedMultiplyAdd", () =>
        {
            Result = BurstMath.FusedMultiplyAddHigh(X, Y, Z);
        });
    }

    [Test, Performance]
    [Category("FusedMultiplyAdd")]
    public void CoreFusedMultiplyAdd()
    {
        MeasurePerformance("FusedMultiplyAdd", () =>
        {
            Result = StrictMath.FusedMultiplyAdd(X, Y, Z);
        });
    }
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("FusedMultiplyAdd")]
    public void BurstLowFusedMultiplyAdd()
    {
        MeasurePerformance("FusedMultiplyAdd", () =>
        {
            double sum = 0.0;

            for (int i = 0; i < X.Length; i++)
            {
                sum += BurstMath.FusedMultiplyAddLow(X[i], Y[i], Z[i]);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("FusedMultiplyAdd")]
    public void BurstMediumFusedMultiplyAdd()
    {
        MeasurePerformance("FusedMultiplyAdd", () =>
        {
            double sum = 0.0;

            for (int i = 0; i < X.Length; i++)
            {
                sum += BurstMath.FusedMultiplyAddMedium(X[i], Y[i], Z[i]);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("FusedMultiplyAdd")]
    public void BurstHighFusedMultiplyAdd()
    {
        MeasurePerformance("FusedMultiplyAdd", () =>
        {
            double sum = 0.0;

            for (int i = 0; i < X.Length; i++)
            {
                sum += BurstMath.FusedMultiplyAddHigh(X[i], Y[i], Z[i]);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("FusedMultiplyAdd")]
    public void CoreFusedMultiplyAdd()
    {
        MeasurePerformance("FusedMultiplyAdd", () =>
        {
            double sum = 0.0;

            for (int i = 0; i < X.Length; i++)
            {
                sum += StrictMath.FusedMultiplyAdd(X[i], Y[i], Z[i]);
            }

            Result = sum;
        });
    }
}
