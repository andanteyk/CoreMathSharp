using System.Globalization;

namespace CoreMathSharp.Tests;

public class SinCosF
{

    [Fact]
    public void TestFloats()
    {
        foreach (var x in Helper.TestFloats)
        {
            (float expectedSin, float expectedCos) = (MathF.Sin(x), MathF.Cos(x));
            (float actualSin, float actualCos) = StrictMathF.SinCos(x);
            float ulpSin = Math.Max(MathF.BitIncrement(actualSin) - actualSin, actualSin - MathF.BitDecrement(actualSin));
            float ulpCos = Math.Max(MathF.BitIncrement(actualCos) - actualCos, actualCos - MathF.BitDecrement(actualCos));

            Assert.Equal(expectedSin, actualSin, float.IsNaN(ulpSin) ? 0.0 : ulpSin);
            Assert.Equal(expectedCos, actualCos, float.IsNaN(ulpCos) ? 0.0 : ulpCos);
        }
    }

    [Fact]
    public void Random()
    {
        var rng = new Seiran(1, 1);

        for (int i = 0; i < 1024 * 1024; i++)
        {
            float x = rng.NextFloat(-MathF.PI, MathF.PI);

            (float expectedSin, float expectedCos) = (MathF.Sin(x), MathF.Cos(x));
            (float actualSin, float actualCos) = StrictMathF.SinCos(x);
            float ulpSin = Math.Max(MathF.BitIncrement(actualSin) - actualSin, actualSin - MathF.BitDecrement(actualSin));
            float ulpCos = Math.Max(MathF.BitIncrement(actualCos) - actualCos, actualCos - MathF.BitDecrement(actualCos));

            Assert.Equal(expectedSin, actualSin, float.IsNaN(ulpSin) ? 0.0 : ulpSin);
            Assert.Equal(expectedCos, actualCos, float.IsNaN(ulpCos) ? 0.0 : ulpCos);
        }

        for (int i = 0; i < 1024 * 1024; i++)
        {
            float x = Polyfill.UInt32BitsToSingle((uint)rng.Next());

            (float expectedSin, float expectedCos) = (MathF.Sin(x), MathF.Cos(x));
            (float actualSin, float actualCos) = StrictMathF.SinCos(x);
            float ulpSin = Math.Max(MathF.BitIncrement(actualSin) - actualSin, actualSin - MathF.BitDecrement(actualSin));
            float ulpCos = Math.Max(MathF.BitIncrement(actualCos) - actualCos, actualCos - MathF.BitDecrement(actualCos));

            Assert.Equal(expectedSin, actualSin, float.IsNaN(ulpSin) ? 0.0 : ulpSin);
            Assert.Equal(expectedCos, actualCos, float.IsNaN(ulpCos) ? 0.0 : ulpCos);
        }
    }

    [Fact]
    public void TestVector()
    {
        string path = "../../../Binary32/sincosf.txt";

        foreach (var line in File.ReadLines(path))
        {
            var parsed = line.Split('\t');

            float x = Polyfill.UInt32BitsToSingle(uint.Parse(parsed[0], NumberStyles.HexNumber));
            float a = Polyfill.UInt32BitsToSingle(uint.Parse(parsed[1], NumberStyles.HexNumber));
            float b = Polyfill.UInt32BitsToSingle(uint.Parse(parsed[2], NumberStyles.HexNumber));


            (float actualSin, float actualCos) = StrictMathF.SinCos(x);
            Assert.Equal(a, actualSin);
            Assert.Equal(b, actualCos);
        }
    }
}
