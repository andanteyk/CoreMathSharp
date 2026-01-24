using System.Globalization;

namespace CoreMathSharp.Tests;

public class Atan2F
{
    [Fact]
    public void TestFloats()
    {
        foreach (var y in Helper.TestFloats)
        {
            foreach (var x in Helper.TestFloats)
            {
                float expected = MathF.Atan2(y, x);
                float actual = StrictMathF.Atan2(y, x);
                float ulp = MathF.Max(MathF.BitIncrement(actual) - actual, actual - MathF.BitDecrement(actual));

                Assert.Equal(expected, actual, float.IsNaN(ulp) ? 0.0f : ulp);
            }
        }
    }

    [Fact]
    public void Random()
    {
        var rng = new Seiran(1, 1);

        for (int i = 0; i < 1024 * 1024; i++)
        {
            float x = Polyfill.UInt32BitsToSingle((uint)rng.Next());
            float y = Polyfill.UInt32BitsToSingle((uint)rng.Next());

            float expected = MathF.Atan2(y, x);
            float actual = StrictMathF.Atan2(y, x);
            float ulp = MathF.Max(MathF.BitIncrement(actual) - actual, actual - MathF.BitDecrement(actual));

            Assert.Equal(expected, actual, float.IsNaN(ulp) ? 0.0f : ulp);
        }

        for (int i = 0; i < 1024 * 1024; i++)
        {
            float x = rng.NextSignedFloat();
            float y = rng.NextSignedFloat();

            float expected = MathF.Atan2(y, x);
            float actual = StrictMathF.Atan2(y, x);
            float ulp = MathF.Max(MathF.BitIncrement(actual) - actual, actual - MathF.BitDecrement(actual));

            Assert.Equal(expected, actual, float.IsNaN(ulp) ? 0.0f : ulp);
        }
    }

    [Fact]
    public void TestVector()
    {
        string path = "../../../Binary32/atan2f.txt";

        foreach (var line in File.ReadLines(path))
        {
            var parsed = line.Split('\t');

            float x = Polyfill.UInt32BitsToSingle(uint.Parse(parsed[0], NumberStyles.HexNumber));
            float y = Polyfill.UInt32BitsToSingle(uint.Parse(parsed[1], NumberStyles.HexNumber));
            float a = Polyfill.UInt32BitsToSingle(uint.Parse(parsed[2], NumberStyles.HexNumber));

            float actual = StrictMathF.Atan2(y, x);
            Assert.Equal(a, actual);
        }
    }
}
