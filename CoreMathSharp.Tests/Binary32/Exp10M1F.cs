using System.Globalization;

namespace CoreMathSharp.Tests;

public class Exp10FM1
{
    // The current implementation of Exp10M1() in .NET is `Pow(10, x) - 1`, which has a very large error.
    [Fact]
    public void TestFloats()
    {
        foreach (var x in Helper.TestFloats)
        {
            float expected = float.Exp10M1(x);
            float actual = StrictMathF.Exp10M1(x);
            float ulp = MathF.Max(MathF.BitIncrement(actual) - actual, actual - MathF.BitDecrement(actual)) * 4.0f;

            Assert.Equal(expected, actual, float.IsNaN(ulp) ? 0.0 : ulp);
        }
    }

    [Fact]
    public void Random()
    {
        var rng = new Seiran(1, 1);

        for (int i = 0; i < 1024 * 1024; i++)
        {
            float x = rng.NextFloat(-40.0f, 40.0f);

            float expected = float.Exp10M1(x);
            float actual = StrictMathF.Exp10M1(x);
            float ulp = MathF.Max(MathF.BitIncrement(actual) - actual, actual - MathF.BitDecrement(actual)) * 1024.0f;
            float tolerance = Math.Max(float.IsNaN(ulp) ? 0.0f : ulp, 9.5367431640625e-07f);

            Assert.Equal(expected, actual, tolerance);
        }

        for (int i = 0; i < 1024 * 1024; i++)
        {
            float x = Polyfill.UInt32BitsToSingle((uint)rng.Next());

            float expected = float.Exp10M1(x);
            float actual = StrictMathF.Exp10M1(x);
            float ulp = MathF.Max(MathF.BitIncrement(actual) - actual, actual - MathF.BitDecrement(actual)) * 1024.0f;
            float tolerance = Math.Max(float.IsNaN(ulp) ? 0.0f : ulp, 9.5367431640625e-07f);

            Assert.Equal(expected, actual, tolerance);
        }
    }

    [Fact]
    public void TestVector()
    {
        string path = "../../../Binary32/exp10m1f.txt";

        foreach (var line in File.ReadLines(path))
        {
            var parsed = line.Split('\t');

            float x = Polyfill.UInt32BitsToSingle(uint.Parse(parsed[0], NumberStyles.HexNumber));
            float a = Polyfill.UInt32BitsToSingle(uint.Parse(parsed[1], NumberStyles.HexNumber));

            float actual = StrictMathF.Exp10M1(x);
            Assert.Equal(a, actual);
        }
    }
}
