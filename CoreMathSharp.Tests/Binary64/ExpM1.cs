using System.Globalization;

namespace CoreMathSharp.Tests;

public class ExpM1
{
    [Fact]
    public void TestDoubles()
    {
        foreach (var x in Helper.TestDoubles)
        {
            double expected = double.ExpM1(x);
            double actual = StrictMath.ExpM1(x);
            double ulp = Math.Max(Math.BitIncrement(actual) - actual, actual - Math.BitDecrement(actual));

            Assert.Equal(expected, actual, double.IsNaN(ulp) ? 0.0 : ulp);
        }
    }

    // The current implementation of ExpM1() in .NET is `Exp(x) - 1`, which has a very large error.
    [Fact]
    public void Random()
    {
        var rng = new Seiran(1, 1);

        for (int i = 0; i < 1024 * 1024; i++)
        {
            double x = rng.NextDouble(-40.0, 256.0);
            double expected = double.ExpM1(x);
            double actual = StrictMath.ExpM1(x);
            double ulp = Math.Max(Math.BitIncrement(actual) - actual, actual - Math.BitDecrement(actual));
            double torelance = Math.Max(double.IsNaN(ulp) ? 0.0 : ulp, 8.88178419700125232339e-16);

            Assert.Equal(expected, actual, torelance);
        }

        for (int i = 0; i < 1024 * 1024; i++)
        {
            double x = Polyfill.UInt64BitsToDouble(rng.Next());

            double expected = double.ExpM1(x);
            double actual = StrictMath.ExpM1(x);
            double ulp = Math.Max(Math.BitIncrement(actual) - actual, actual - Math.BitDecrement(actual));
            double torelance = Math.Max(double.IsNaN(ulp) ? 0.0 : ulp, 8.88178419700125232339e-16);

            Assert.Equal(expected, actual, torelance);
        }
    }

    [Fact]
    public void TestVector()
    {
        string path = "../../../Binary64/expm1.txt";

        foreach (var line in File.ReadLines(path))
        {
            var parsed = line.Split('\t');

            double x = Polyfill.UInt64BitsToDouble(ulong.Parse(parsed[0], NumberStyles.HexNumber));
            double a = Polyfill.UInt64BitsToDouble(ulong.Parse(parsed[1], NumberStyles.HexNumber));

            double actual = StrictMath.ExpM1(x);
            Assert.Equal(a, actual);
        }
    }
}
