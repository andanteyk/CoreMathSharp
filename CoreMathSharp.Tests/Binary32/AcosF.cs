using System.Globalization;

namespace CoreMathSharp.Tests;

public class AcosF
{
    [Fact]
    public void TestFloats()
    {
        foreach (var x in Helper.TestFloats)
        {
            float expected = MathF.Acos(x);
            float actual = StrictMathF.Acos(x);
            float ulp = MathF.Max(MathF.BitIncrement(actual) - actual, actual - MathF.BitDecrement(actual));

            Assert.Equal(expected, actual, float.IsNaN(ulp) ? 0.0 : ulp);
        }
    }

    [Fact]
    public void Random()
    {
        var rng = new Seiran(1, 1);

        for (int i = 0; i < 1024; i++)
        {
            float x = rng.NextSignedFloat();

            float expected = MathF.Acos(x);
            float actual = StrictMathF.Acos(x);
            float ulp = MathF.Max(MathF.BitIncrement(actual) - actual, actual - MathF.BitDecrement(actual));

            Assert.Equal(expected, actual, ulp);
        }
    }

    [Fact]
    public void TestVector()
    {
        string path = "../../../Binary32/acosf.txt";

        foreach (var line in File.ReadLines(path))
        {
            var parsed = line.Split('\t');

            float x = StrictMath.UInt32BitsToSingle(uint.Parse(parsed[0], NumberStyles.HexNumber));
            float a = StrictMath.UInt32BitsToSingle(uint.Parse(parsed[1], NumberStyles.HexNumber));

            float actual = StrictMathF.Acos(x);
            Assert.Equal(a, actual);
        }
    }
}
