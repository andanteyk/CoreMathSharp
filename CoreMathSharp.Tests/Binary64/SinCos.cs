using System.Globalization;

namespace CoreMathSharp.Tests;

public class SinCos
{
    // NOTE: in .NET 8, Math.SinCos().Cos != Math.Cos(). SinCos().Cos is slightly inaccurate.
    // Maybe related: https://github.com/dotnet/runtime/issues/98204
    // Therefore, we cannot rely on SinCos().
    [Fact]
    public void TestDoubles()
    {
        foreach (var x in Helper.TestDoubles)
        {
            (double expectedSin, double expectedCos) = (Math.Sin(x), Math.Cos(x));
            (double actualSin, double actualCos) = StrictMath.SinCos(x);
            double ulpSin = Math.Max(Math.BitIncrement(actualSin) - actualSin, actualSin - Math.BitDecrement(actualSin));
            double ulpCos = Math.Max(Math.BitIncrement(actualCos) - actualCos, actualCos - Math.BitDecrement(actualCos));

            Assert.Equal(expectedSin, actualSin, double.IsNaN(ulpSin) ? 0.0 : ulpSin);
            Assert.Equal(expectedCos, actualCos, double.IsNaN(ulpCos) ? 0.0 : ulpCos);
        }
    }

    [Fact]
    public void Random()
    {
        var rng = new Seiran(1, 1);

        for (int i = 0; i < 1024 * 1024; i++)
        {
            double x = rng.NextDouble(-Math.PI, Math.PI);

            (double expectedSin, double expectedCos) = (Math.Sin(x), Math.Cos(x));
            (double actualSin, double actualCos) = StrictMath.SinCos(x);
            double ulpSin = Math.Max(Math.BitIncrement(actualSin) - actualSin, actualSin - Math.BitDecrement(actualSin));
            double ulpCos = Math.Max(Math.BitIncrement(actualCos) - actualCos, actualCos - Math.BitDecrement(actualCos));

            Assert.Equal(expectedSin, actualSin, double.IsNaN(ulpSin) ? 0.0 : ulpSin);
            Assert.Equal(expectedCos, actualCos, double.IsNaN(ulpCos) ? 0.0 : ulpCos);
        }

        for (int i = 0; i < 1024 * 1024; i++)
        {
            double x = Polyfill.UInt64BitsToDouble(rng.Next());

            (double expectedSin, double expectedCos) = (Math.Sin(x), Math.Cos(x));
            (double actualSin, double actualCos) = StrictMath.SinCos(x);
            double ulpSin = Math.Max(Math.BitIncrement(actualSin) - actualSin, actualSin - Math.BitDecrement(actualSin));
            double ulpCos = Math.Max(Math.BitIncrement(actualCos) - actualCos, actualCos - Math.BitDecrement(actualCos));

            Assert.Equal(expectedSin, actualSin, double.IsNaN(ulpSin) ? 0.0 : ulpSin);
            Assert.Equal(expectedCos, actualCos, double.IsNaN(ulpCos) ? 0.0 : ulpCos);
        }
    }


    [Fact]
    public void TestVector()
    {
        string path = "../../../Binary64/sincos.txt";

        foreach (var line in File.ReadLines(path))
        {
            var parsed = line.Split('\t');

            double x = Polyfill.UInt64BitsToDouble(ulong.Parse(parsed[0], NumberStyles.HexNumber));
            double a = Polyfill.UInt64BitsToDouble(ulong.Parse(parsed[1], NumberStyles.HexNumber));
            double b = Polyfill.UInt64BitsToDouble(ulong.Parse(parsed[2], NumberStyles.HexNumber));

            (double sin, double cos) = StrictMath.SinCos(x);
            Assert.Equal(a, sin);
            Assert.Equal(b, cos);
        }
    }
}
