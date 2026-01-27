using System.Globalization;

namespace CoreMathSharp.Tests;

public class Erfc
{

    /*
    // dotnet currently does not have an implementation of the Erfc(), so there is no way to test it.
    // Since there is no simple and accurate implementation, we will rely on C-generated tests.
    [Fact]
    public void TestDoubles()
    {
        foreach (var x in Helper.TestDoubles)
        {
            double expected = EasyErfc(x);
            double actual = StrictMath.Erfc(x);
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
            double x = rng.NextDouble(-Math.PI, Math.PI);

            double expected = EasyErfc(x);
            double actual = StrictMath.Erfc(x);
            double ulp = Math.Max(Math.BitIncrement(actual) - actual, actual - Math.BitDecrement(actual));

            Assert.Equal(expected, actual, double.IsNaN(ulp) ? 0.0 : ulp);
        }

        for (int i = 0; i < 1024 * 1024; i++)
        {
            double x = Polyfill.UInt64BitsToDouble(rng.Next());

            double expected = EasyErfc(x);
            double actual = StrictMath.Erfc(x);
            double ulp = Math.Max(Math.BitIncrement(actual) - actual, actual - Math.BitDecrement(actual));

            Assert.Equal(expected, actual, double.IsNaN(ulp) ? 0.0 : ulp);
        }
    }
    //*/

    [Fact]
    public void TestVector()
    {
        string path = "../../../Binary64/erfc.txt";

        StrictMath.Erfc(2.764570834254463);

        foreach (var line in File.ReadLines(path))
        {
            var parsed = line.Split('\t');

            double x = Polyfill.UInt64BitsToDouble(ulong.Parse(parsed[0], NumberStyles.HexNumber));
            double a = Polyfill.UInt64BitsToDouble(ulong.Parse(parsed[1], NumberStyles.HexNumber));

            double actual = StrictMath.Erfc(x);
            Assert.Equal(a, actual);
        }
    }
}
