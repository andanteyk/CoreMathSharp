using System.Globalization;

namespace CoreMathSharp.Tests;

public class AtanPi
{
    [Fact]
    public void TestDoubles()
    {
        foreach (var x in Helper.TestDoubles)
        {
            double expected = double.AtanPi(x);
            double actual = StrictMath.AtanPi(x);
            double ulp = Math.Max(Math.BitIncrement(actual) - actual, actual - Math.BitDecrement(actual));

            Assert.Equal(expected, actual, double.IsNaN(ulp) ? 0.0 : ulp);
        }
    }

    [Fact]
    public void Random()
    {
        var rng = new Seiran(1, 1);

        for (int i = 0; i < 1024 * 1024; i++)
        {
            double x = rng.NextSignedDouble();

            double expected = double.AtanPi(x);
            double actual = StrictMath.AtanPi(x);
            double ulp = Math.Max(Math.BitIncrement(actual) - actual, actual - Math.BitDecrement(actual)) * 2.0;

            Assert.Equal(expected, actual, double.IsNaN(ulp) ? 0.0 : ulp);
        }
    }

    [Fact]
    public void TestVector()
    {
        string path = "../../../Binary64/atanpi.txt";

        StrictMath.AtanPi(-0.31218868599068805);

        foreach (var line in File.ReadLines(path))
        {
            var parsed = line.Split('\t');

            double x = Polyfill.UInt64BitsToDouble(ulong.Parse(parsed[0], NumberStyles.HexNumber));
            double a = Polyfill.UInt64BitsToDouble(ulong.Parse(parsed[1], NumberStyles.HexNumber));

            double actual = StrictMath.AtanPi(x);
            Assert.Equal(a, actual);
        }
    }
}
