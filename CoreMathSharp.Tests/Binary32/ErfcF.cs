using System.Globalization;

namespace CoreMathSharp.Tests;

public class ErfcF
{
    // https://www.johndcook.com/blog/2009/01/19/stand-alone-error-function-erf/
    static float EasyErfc(float x)
    {
        const double a1 = 0.254829592;
        const double a2 = -0.284496736;
        const double a3 = 1.421413741;
        const double a4 = -1.453152027;
        const double a5 = 1.061405429;
        const double p = 0.3275911;

        float sign = MathF.CopySign(1.0f, x);
        x = MathF.Abs(x);

        double t = 1.0 / (1.0 + p * x);
        double y = 1.0 - (((((a5 * t + a4) * t) + a3) * t + a2) * t + a1) * t * Math.Exp(-x * x);
        return (float)(1.0 - sign * y);
    }


    /*
    // Because it is a simulated function, it is not possible to test the results when singular values ​​are given.
    // Testing for singular values ​​is done with TestVector()
    [Fact]
    public void TestFloats()
    {
        foreach (var x in Helper.TestFloats)
        {
            float expected = EasyErfc(x);
            float actual = StrictMathF.Erf(x);
            float ulp = MathF.Max(MathF.BitIncrement(actual) - actual, actual - MathF.BitDecrement(actual));

            Assert.Equal(expected, actual, float.IsNaN(ulp) ? 0.0 : ulp);
        }
    }
    //*/

    [Fact]
    public void Random()
    {
        var rng = new Seiran(1, 1);

        for (int i = 0; i < 1024 * 1024; i++)
        {
            float x = rng.NextFloat(-4.0f, 4.0f);

            float expected = EasyErfc(x);
            float actual = StrictMathF.Erfc(x);
            float torelance = 1.0e-6f;

            Assert.Equal(expected, actual, torelance);
        }
    }

    [Fact]
    public void TestVector()
    {
        string path = "../../../Binary32/erfcf.txt";

        foreach (var line in File.ReadLines(path))
        {
            var parsed = line.Split('\t');

            float x = Polyfill.UInt32BitsToSingle(uint.Parse(parsed[0], NumberStyles.HexNumber));
            float a = Polyfill.UInt32BitsToSingle(uint.Parse(parsed[1], NumberStyles.HexNumber));

            float actual = StrictMathF.Erfc(x);
            Assert.Equal(a, actual);
        }
    }
}
