using System.Globalization;

namespace CoreMathSharp.Tests;

public class Log2
{
    [Fact]
    public void TestDoubles()
    {
        foreach (var x in Helper.TestDoubles)
        {
            double expected = Math.Log2(x);
            double actual = StrictMath.Log2(x);
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
            double x = rng.NextDouble(-1.0, 1024.0);
            double expected = Math.Log2(x);
            double actual = StrictMath.Log2(x);
            double ulp = Math.Max(Math.BitIncrement(actual) - actual, actual - Math.BitDecrement(actual));
            double tolerance = Math.Max(double.IsNaN(ulp) ? 0.0 : ulp, 0);

            Assert.Equal(expected, actual, tolerance);
        }

        for (int i = 0; i < 1024 * 1024; i++)
        {
            double x = Polyfill.UInt64BitsToDouble(rng.Next() >> 1);

            double expected = Math.Log2(x);
            double actual = StrictMath.Log2(x);
            double ulp = Math.Max(Math.BitIncrement(actual) - actual, actual - Math.BitDecrement(actual));
            double tolerance = Math.Max(double.IsNaN(ulp) ? 0.0 : ulp, 0);

            Assert.Equal(expected, actual, tolerance);
        }
    }

    [Fact]
    public void TestVector()
    {
        string path = "../../../Binary64/log2.txt";

        foreach (var line in File.ReadLines(path))
        {
            var parsed = line.Split('\t');

            double x = Polyfill.UInt64BitsToDouble(ulong.Parse(parsed[0], NumberStyles.HexNumber));
            double a = Polyfill.UInt64BitsToDouble(ulong.Parse(parsed[1], NumberStyles.HexNumber));

            double actual = StrictMath.Log2(x);
            Assert.Equal(a, actual);
        }
    }
}
