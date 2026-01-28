using System.Globalization;

namespace CoreMathSharp.Tests;

public class ExpM1F
{
    [Fact]
    public void TestFloats()
    {
        foreach (var x in Helper.TestFloats)
        {
            float expected = float.ExpM1(x);
            float actual = StrictMathF.ExpM1(x);
            float ulp = MathF.Max(MathF.BitIncrement(actual) - actual, actual - MathF.BitDecrement(actual));

            Assert.Equal(expected, actual, float.IsNaN(ulp) ? 0.0f : ulp);
        }
    }

    [Fact]
    public void Random()
    {
        var rng = new Seiran(1, 1);

        for (int i = 0; i < 1024 * 1024; i++)
        {
            float x = rng.NextFloat(-20.0f, 100.0f);

            float expected = float.ExpM1(x);
            float actual = StrictMathF.ExpM1(x);
            float ulp = MathF.Max(MathF.BitIncrement(actual) - actual, actual - MathF.BitDecrement(actual));
            float torelance = Math.Max(float.IsNaN(ulp) ? 0.0f : ulp, 9.5367431640625e-07f);

            Assert.Equal(expected, actual, torelance);
        }

        for (int i = 0; i < 1024 * 1024; i++)
        {
            float x = Polyfill.UInt32BitsToSingle((uint)rng.Next());

            float expected = float.ExpM1(x);
            float actual = StrictMathF.ExpM1(x);
            float ulp = MathF.Max(MathF.BitIncrement(actual) - actual, actual - MathF.BitDecrement(actual));
            float torelance = Math.Max(float.IsNaN(ulp) ? 0.0f : ulp, 9.5367431640625e-07f);

            Assert.Equal(expected, actual, torelance);
        }
    }

    [Fact]
    public void TestVector()
    {
        string path = "../../../Binary32/expm1f.txt";

        foreach (var line in File.ReadLines(path))
        {
            var parsed = line.Split('\t');

            float x = Polyfill.UInt32BitsToSingle(uint.Parse(parsed[0], NumberStyles.HexNumber));
            float a = Polyfill.UInt32BitsToSingle(uint.Parse(parsed[1], NumberStyles.HexNumber));

            float actual = StrictMathF.ExpM1(x);
            Assert.Equal(a, actual);
        }
    }
}
