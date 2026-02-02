using System.Globalization;

namespace CoreMathSharp.Tests;

public class TGamma
{
    /*
    [Fact]
    public void TestDoubles()
    {
        foreach (var x in Helper.TestDoubles)
        {
            double expected = Math.TGamma(x);
            double actual = StrictMath.TGamma(x);
            double ulp = Math.Max(Math.BitIncrement(actual) - actual, actual - Math.BitDecrement(actual));

            Assert.Equal(expected, actual, double.IsNaN(ulp) ? 0.0 : ulp);
        }
    }
    //*/

    /*
    [Fact]
    public void Random()
    {
        var rng = new Seiran(1, 1);

        for (int i = 0; i < 1024 * 1024; i++)
        {
            double x = rng.NextDouble(-256.0, 256.0);
            double expected = Math.TGamma(x);
            double actual = StrictMath.TGamma(x);
            double ulp = Math.Max(Math.BitIncrement(actual) - actual, actual - Math.BitDecrement(actual));

            Assert.Equal(expected, actual, double.IsNaN(ulp) ? 0.0 : ulp);
        }

        for (int i = 0; i < 1024 * 1024; i++)
        {
            double x = Polyfill.UInt64BitsToDouble(rng.Next());

            double expected = Math.TGamma(x);
            double actual = StrictMath.TGamma(x);
            double ulp = Math.Max(Math.BitIncrement(actual) - actual, actual - Math.BitDecrement(actual));

            Assert.Equal(expected, actual, double.IsNaN(ulp) ? 0.0 : ulp);
        }
    }
    //*/

    [Fact]
    public void TestVector()
    {
        string path = "../../../Binary64/tgamma.txt";

        StrictMath.TGamma(2.860486667597499);


        foreach (var line in File.ReadLines(path))
        {
            var parsed = line.Split('\t');

            double x = Polyfill.UInt64BitsToDouble(ulong.Parse(parsed[0], NumberStyles.HexNumber));
            double a = Polyfill.UInt64BitsToDouble(ulong.Parse(parsed[1], NumberStyles.HexNumber));

            double actualValue = StrictMath.TGamma(x);
            Assert.Equal(a, actualValue);
        }
    }
}
