using System.Globalization;

namespace CoreMathSharp.Tests;

public class Hypot
{
    [Fact]
    public void TestDoubles()
    {
        foreach (var y in Helper.TestDoubles)
        {
            foreach (var x in Helper.TestDoubles)
            {
                double expected = double.Hypot(x, y);
                double actual = StrictMath.Hypot(x, y);
                double ulp = Math.Max(Math.BitIncrement(actual) - actual, actual - Math.BitDecrement(actual));

                Assert.Equal(expected, actual, double.IsNaN(ulp) ? 0.0 : ulp);
            }
        }
    }

    [Fact]
    public void Random()
    {
        var rng = new Seiran(1, 1);

        StrictMath.Hypot(1.1161785054958028E-305, -4.92211361567176E-309);

        for (int i = 0; i < 1024 * 1024; i++)
        {
            double x = rng.NextDouble(-256.0, 256.0);
            double y = rng.NextDouble(-256.0, 256.0);

            double expected = double.Hypot(x, y);
            double actual = StrictMath.Hypot(x, y);
            double ulp = Math.Max(Math.BitIncrement(actual) - actual, actual - Math.BitDecrement(actual));

            Assert.Equal(expected, actual, double.IsNaN(ulp) ? 0.0 : ulp);
        }

        for (int i = 0; i < 1024 * 1024; i++)
        {
            double x = Polyfill.UInt64BitsToDouble(rng.Next());
            double y = Polyfill.UInt64BitsToDouble(rng.Next());

            double expected = double.Hypot(x, y);
            double actual = StrictMath.Hypot(x, y);
            double ulp = Math.Max(Math.BitIncrement(actual) - actual, actual - Math.BitDecrement(actual));

            Assert.Equal(expected, actual, double.IsNaN(ulp) ? 0.0 : ulp);
        }
    }

    [Fact]
    public void TestVector()
    {
        string path = "../../../Binary64/hypot.txt";

        foreach (var line in File.ReadLines(path))
        {
            var parsed = line.Split('\t');

            double x = Polyfill.UInt64BitsToDouble(ulong.Parse(parsed[0], NumberStyles.HexNumber));
            double y = Polyfill.UInt64BitsToDouble(ulong.Parse(parsed[1], NumberStyles.HexNumber));
            double a = Polyfill.UInt64BitsToDouble(ulong.Parse(parsed[2], NumberStyles.HexNumber));

            double actual = StrictMath.Hypot(x, y);
            Assert.Equal(a, actual);
        }
    }
}
