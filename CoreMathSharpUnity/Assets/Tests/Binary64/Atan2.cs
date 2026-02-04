using CoreMathSharp;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace CoreMathSharpUnity.Tests;

public partial class UnityBenchmark
{
    [Test, Performance]
    [Category("Atan2")]
    public void BurstLowAtan2()
    {
        MeasurePerformance("Atan2", () =>
        {
            Result = BurstMath.Atan2Low(Y, X);
        });
    }

    [Test, Performance]
    [Category("Atan2")]
    public void BurstMediumAtan2()
    {
        MeasurePerformance("Atan2", () =>
        {
            Result = BurstMath.Atan2Medium(Y, X);
        });
    }

    [Test, Performance]
    [Category("Atan2")]
    public void BurstHighAtan2()
    {
        MeasurePerformance("Atan2", () =>
        {
            Result = BurstMath.Atan2High(Y, X);
        });
    }

    [Test, Performance]
    [Category("Atan2")]
    public void CoreAtan2()
    {
        MeasurePerformance("Atan2", () =>
        {
            Result = StrictMath.Atan2(Y, X);
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("Atan2")]
    public void PInvokeAtan2()
    {
        MeasurePerformance("Atan2", () =>
        {
            Result = PInvoke.PInvoke.Atan2(Y, X);
        });
    }
#endif
}

public partial class UnityMacroBenchmark
{
    [Test, Performance]
    [Category("Atan2")]
    public void BurstLowAtan2()
    {
        MeasurePerformance("Atan2", () =>
        {
            double sum = 0.0;

            for (int i = 0; i < X.Length; i++)
            {
                sum += BurstMath.Atan2Low(Y[i], X[i]);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Atan2")]
    public void BurstMediumAtan2()
    {
        MeasurePerformance("Atan2", () =>
        {
            double sum = 0.0;

            for (int i = 0; i < X.Length; i++)
            {
                sum += BurstMath.Atan2Medium(Y[i], X[i]);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Atan2")]
    public void BurstHighAtan2()
    {
        MeasurePerformance("Atan2", () =>
        {
            double sum = 0.0;

            for (int i = 0; i < X.Length; i++)
            {
                sum += BurstMath.Atan2High(Y[i], X[i]);
            }

            Result = sum;
        });
    }

    [Test, Performance]
    [Category("Atan2")]
    public void CoreAtan2()
    {
        MeasurePerformance("Atan2", () =>
        {
            double sum = 0.0;

            for (int i = 0; i < X.Length; i++)
            {
                sum += StrictMath.Atan2(Y[i], X[i]);
            }

            Result = sum;
        });
    }

#if PINVOKE_ENABLED
    [Test, Performance]
    [Category("Atan2")]
    public void PInvokeAtan2()
    {
        MeasurePerformance("Atan2", () =>
        {
            double sum = 0.0;

            for (int i = 0; i < X.Length; i++)
            {
                sum += PInvoke.PInvoke.Atan2(Y[i], X[i]);
            }

            Result = sum;
        });
    }
#endif
}
