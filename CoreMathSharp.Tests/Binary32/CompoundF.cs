using System.Globalization;

namespace CoreMathSharp.Tests;

public class CompoundF
{
    public static float SimulatedCompoundF(float x, float y) => (float)Math.Pow(1.0 + x, y);

    /*
    // Because it is a simulated function, it is not possible to test the results when singular values ​​are given.
    // Testing for singular values ​​is done with TestVector()
    [Fact]
    public void TestFloats()
    {
        foreach (var y in Helper.TestFloats)
        {
            foreach (var x in Helper.TestFloats)
            {
                float expected = SimulatedCompoundF(x, y);
                float actual = StrictMathF.Compound(x, y);
                float ulp = MathF.Max(MathF.BitIncrement(actual) - actual, actual - MathF.BitDecrement(actual));

                Assert.Equal(expected, actual, float.IsNaN(ulp) ? 0.0f : ulp);
            }
        }
    }
    //*/

    [Fact]
    public void Random()
    {
        var rng = new Seiran(1, 1);

        for (int i = 0; i < 1024 * 1024; i++)
        {
            float x = rng.NextSignedFloat();
            float y = rng.NextFloat(-10.0f, 10.0f);

            float expected = SimulatedCompoundF(x, y);
            float actual = StrictMathF.Compound(x, y);
            float ulp = MathF.Max(MathF.BitIncrement(actual) - actual, actual - MathF.BitDecrement(actual));

            Assert.Equal(expected, actual, float.IsNaN(ulp) ? 0.0f : ulp);
        }
    }

    [Fact]
    public void TestVector()
    {
        string path = "../../../Binary32/compoundf.txt";

        foreach (var line in File.ReadLines(path))
        {
            var parsed = line.Split('\t');

            float x = Polyfill.UInt32BitsToSingle(uint.Parse(parsed[0], NumberStyles.HexNumber));
            float y = Polyfill.UInt32BitsToSingle(uint.Parse(parsed[1], NumberStyles.HexNumber));
            float a = Polyfill.UInt32BitsToSingle(uint.Parse(parsed[2], NumberStyles.HexNumber));

            float actual = StrictMathF.Compound(x, y);
            Assert.Equal(a, actual);
        }
    }
}
