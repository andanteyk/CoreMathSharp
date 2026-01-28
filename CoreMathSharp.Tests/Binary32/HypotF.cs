using System.Globalization;

namespace CoreMathSharp.Tests;

public class HypotF
{
    [Fact]
    public void TestFloats()
    {
        foreach (var y in Helper.TestFloats)
        {
            foreach (var x in Helper.TestFloats)
            {
                float expected = float.Hypot(x, y);
                float actual = StrictMathF.Hypot(x, y);
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
            float x = rng.NextFloat(-256.0f, 256.0f);
            float y = rng.NextFloat(-256.0f, 256.0f);

            float expected = float.Hypot(x, y);
            float actual = StrictMathF.Hypot(x, y);
            float ulp = MathF.Max(MathF.BitIncrement(actual) - actual, actual - MathF.BitDecrement(actual));

            Assert.Equal(expected, actual, float.IsNaN(ulp) ? 0.0f : ulp);
        }

        for (int i = 0; i < 1024 * 1024; i++)
        {
            float x = Polyfill.UInt32BitsToSingle((uint)rng.Next());
            float y = Polyfill.UInt32BitsToSingle((uint)rng.Next());

            float expected = float.Hypot(x, y);
            float actual = StrictMathF.Hypot(x, y);
            float ulp = MathF.Max(MathF.BitIncrement(actual) - actual, actual - MathF.BitDecrement(actual));

            Assert.Equal(expected, actual, float.IsNaN(ulp) ? 0.0f : ulp);
        }
    }

    [Fact]
    public void TestVector()
    {
        string path = "../../../Binary32/hypotf.txt";

        foreach (var line in File.ReadLines(path))
        {
            var parsed = line.Split('\t');

            float x = Polyfill.UInt32BitsToSingle(uint.Parse(parsed[0], NumberStyles.HexNumber));
            float y = Polyfill.UInt32BitsToSingle(uint.Parse(parsed[1], NumberStyles.HexNumber));
            float a = Polyfill.UInt32BitsToSingle(uint.Parse(parsed[2], NumberStyles.HexNumber));

            float actual = StrictMathF.Hypot(x, y);
            Assert.Equal(a, actual);
        }
    }
}
