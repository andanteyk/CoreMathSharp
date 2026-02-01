using System.Globalization;

namespace CoreMathSharp.Tests;

public class Atan2Pi
{
    [Fact]
    public void TestDoubles()
    {
        foreach (var y in Helper.TestDoubles)
        {
            foreach (var x in Helper.TestDoubles)
            {
                double expected = double.Atan2Pi(y, x);
                double actual = StrictMath.Atan2Pi(y, x);
                double ulp = Math.Max(Math.BitIncrement(actual) - actual, actual - Math.BitDecrement(actual));

                Assert.Equal(expected, actual, double.IsNaN(ulp) ? 0.0 : ulp);
            }
        }
    }

    [Fact]
    public void Random()
    {
        var rng = new Seiran(1, 1);

        for (int i = 0; i < 1024 * 1024; i++)
        {
            double x = Polyfill.UInt64BitsToDouble(rng.Next());
            double y = Polyfill.UInt64BitsToDouble(rng.Next());

            double expected = double.Atan2Pi(y, x);
            double actual = StrictMath.Atan2Pi(y, x);
            double ulp = Math.Max(Math.BitIncrement(actual) - actual, actual - Math.BitDecrement(actual));

            Assert.Equal(expected, actual, double.IsNaN(ulp) ? 0.0 : ulp);
        }

        for (int i = 0; i < 1024 * 1024; i++)
        {
            double x = rng.NextSignedDouble();
            double y = rng.NextSignedDouble();

            double expected = double.Atan2Pi(y, x);
            double actual = StrictMath.Atan2Pi(y, x);
            double ulp = Math.Max(Math.BitIncrement(actual) - actual, actual - Math.BitDecrement(actual));

            Assert.Equal(expected, actual, double.IsNaN(ulp) ? 0.0 : ulp);
        }
    }

    [Fact]
    public void TestVector()
    {
        string path = "../../../Binary64/atan2pi.txt";

        foreach (var line in File.ReadLines(path))
        {
            var parsed = line.Split('\t');

            double x = Polyfill.UInt64BitsToDouble(ulong.Parse(parsed[0], NumberStyles.HexNumber));
            double y = Polyfill.UInt64BitsToDouble(ulong.Parse(parsed[1], NumberStyles.HexNumber));
            double a = Polyfill.UInt64BitsToDouble(ulong.Parse(parsed[2], NumberStyles.HexNumber));

            double actual = StrictMath.Atan2Pi(y, x);
            Assert.Equal(a, actual);
        }
    }
}
