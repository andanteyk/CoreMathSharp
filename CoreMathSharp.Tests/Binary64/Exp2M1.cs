using System.Globalization;

namespace CoreMathSharp.Tests;

public class Exp2M1
{
    [Fact]
    public void TestDoubles()
    {
        foreach (var x in Helper.TestDoubles)
        {
            double expected = double.Exp2M1(x);
            double actual = StrictMath.Exp2M1(x);
            double ulp = Math.Max(Math.BitIncrement(actual) - actual, actual - Math.BitDecrement(actual));

            Assert.Equal(expected, actual, double.IsNaN(ulp) ? 0.0 : ulp);
        }
    }

    // The current implementation of Exp2M1() in .NET is `Pow(2, x) - 1`, which has a very large error. 
    [Fact]
    public void Random()
    {
        var rng = new Seiran(1, 1);

        for (int i = 0; i < 1024 * 1024; i++)
        {
            double x = rng.NextDouble(-64.0, 1024.0);
            double expected = double.Exp2M1(x);
            double actual = StrictMath.Exp2M1(x);
            double ulp = Math.Max(Math.BitIncrement(actual) - actual, actual - Math.BitDecrement(actual));
            double torelance = Math.Max(double.IsNaN(ulp) ? 0.0 : ulp, 8.8817841970012523e-16);

            Assert.Equal(expected, actual, torelance);
        }

        for (int i = 0; i < 1024 * 1024; i++)
        {
            double x = Polyfill.UInt64BitsToDouble(rng.Next());

            double expected = double.Exp2M1(x);
            double actual = StrictMath.Exp2M1(x);
            double ulp = Math.Max(Math.BitIncrement(actual) - actual, actual - Math.BitDecrement(actual));
            double torelance = Math.Max(double.IsNaN(ulp) ? 0.0 : ulp, 8.8817841970012523e-16);

            Assert.Equal(expected, actual, torelance);
        }
    }

    [Fact]
    public void TestVector()
    {
        string path = "../../../Binary64/exp2m1.txt";

        foreach (var line in File.ReadLines(path))
        {
            var parsed = line.Split('\t');

            double x = Polyfill.UInt64BitsToDouble(ulong.Parse(parsed[0], NumberStyles.HexNumber));
            double a = Polyfill.UInt64BitsToDouble(ulong.Parse(parsed[1], NumberStyles.HexNumber));

            double actual = StrictMath.Exp2M1(x);
            Assert.Equal(a, actual);
        }
    }
}
