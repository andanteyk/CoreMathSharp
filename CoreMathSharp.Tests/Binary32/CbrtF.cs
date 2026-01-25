using System.Globalization;

namespace CoreMathSharp.Tests;

public class CbrtF
{
    [Fact]
    public void TestFloats()
    {
        foreach (var x in Helper.TestFloats)
        {
            float expected = MathF.Cbrt(x);
            float actual = StrictMathF.Cbrt(x);
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
            float x = Polyfill.UInt32BitsToSingle((uint)rng.Next());

            float expected = MathF.Cbrt(x);
            float actual = StrictMathF.Cbrt(x);
            float ulp = MathF.Max(MathF.BitIncrement(actual) - actual, actual - MathF.BitDecrement(actual)) * 2.0f;

            Assert.Equal(expected, actual, float.IsNaN(ulp) ? 0.0f : ulp);
        }
    }

    [Fact]
    public void TestVector()
    {
        string path = "../../../Binary32/cbrtf.txt";

        foreach (var line in File.ReadLines(path))
        {
            var parsed = line.Split('\t');

            float x = Polyfill.UInt32BitsToSingle(uint.Parse(parsed[0], NumberStyles.HexNumber));
            float a = Polyfill.UInt32BitsToSingle(uint.Parse(parsed[1], NumberStyles.HexNumber));

            float actual = StrictMathF.Cbrt(x);
            Assert.Equal(a, actual);
        }
    }
}
