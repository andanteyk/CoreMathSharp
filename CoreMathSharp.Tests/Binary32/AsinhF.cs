using System.Globalization;

namespace CoreMathSharp.Tests;

public class AsinhF
{
    [Fact]
    public void TestFloats()
    {
        foreach (var x in Helper.TestFloats)
        {
            float expected = MathF.Asinh(x);
            float actual = StrictMathF.Asinh(x);
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
            float x = rng.NextFloat(1.0f, 10.0f);

            float expected = MathF.Asinh(x);
            float actual = StrictMathF.Asinh(x);
            float ulp = MathF.Max(MathF.BitIncrement(actual) - actual, actual - MathF.BitDecrement(actual));

            Assert.Equal(expected, actual, ulp);
        }
    }

    [Fact]
    public void TestVector()
    {
        string path = "../../../Binary32/asinhf.txt";

        StrictMathF.Asinh(2.7182817f);

        foreach (var line in File.ReadLines(path))
        {
            var parsed = line.Split('\t');

            float x = Polyfill.UInt32BitsToSingle(uint.Parse(parsed[0], NumberStyles.HexNumber));
            float a = Polyfill.UInt32BitsToSingle(uint.Parse(parsed[1], NumberStyles.HexNumber));

            float actual = StrictMathF.Asinh(x);
            Assert.Equal(a, actual);
        }
    }
}
