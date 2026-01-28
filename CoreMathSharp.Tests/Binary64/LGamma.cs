using System.Globalization;

namespace CoreMathSharp.Tests;

public class LGamma
{
    /*
    [Fact]
    public void TestDoubles()
    {
        foreach (var x in Helper.TestDoubles)
        {
            double expected = Math.LGamma(x);
            double actual = StrictMath.LGamma(x);
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
            double expected = Math.LGamma(x);
            double actual = StrictMath.LGamma(x);
            double ulp = Math.Max(Math.BitIncrement(actual) - actual, actual - Math.BitDecrement(actual));

            Assert.Equal(expected, actual, double.IsNaN(ulp) ? 0.0 : ulp);
        }

        for (int i = 0; i < 1024 * 1024; i++)
        {
            double x = Polyfill.UInt64BitsToDouble(rng.Next());

            double expected = Math.LGamma(x);
            double actual = StrictMath.LGamma(x);
            double ulp = Math.Max(Math.BitIncrement(actual) - actual, actual - Math.BitDecrement(actual));

            Assert.Equal(expected, actual, double.IsNaN(ulp) ? 0.0 : ulp);
        }
    }
    //*/

    [Fact]
    public void TestVector()
    {
        string path = "../../../Binary64/lgamma.txt";

        foreach (var line in File.ReadLines(path))
        {
            var parsed = line.Split('\t');

            double x = Polyfill.UInt64BitsToDouble(ulong.Parse(parsed[0], NumberStyles.HexNumber));
            double a = Polyfill.UInt64BitsToDouble(ulong.Parse(parsed[1], NumberStyles.HexNumber));
            int b = (int)ulong.Parse(parsed[2], NumberStyles.HexNumber);

            (double actualValue, int actualSign) = StrictMath.LGamma(x);
            Assert.Equal(a, actualValue);
            Assert.Equal(b, actualSign);
        }
    }
}
