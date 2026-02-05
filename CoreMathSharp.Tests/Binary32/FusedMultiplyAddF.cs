using System.Globalization;

namespace CoreMathSharp.Tests;

public class FusedMultiplyAddF
{
    [Fact]
    public void TestFloats()
    {
        foreach (var z in Helper.TestFloats)
        {
            foreach (var y in Helper.TestFloats)
            {
                foreach (var x in Helper.TestFloats)
                {
                    Assert.Equal(MathF.FusedMultiplyAdd(x, y, z), StrictMathF.FusedMultiplyAdd(x, y, z));
                }
            }
        }
    }

    [Fact]
    public void Random()
    {
        var rng = new Seiran(1, 1);

        for (int i = 0; i < 1024 * 1024; i++)
        {
            float x = rng.NextSignedFloat();
            float y = rng.NextSignedFloat();
            float z = rng.NextSignedFloat();

            Assert.Equal(MathF.FusedMultiplyAdd(x, y, z), StrictMathF.FusedMultiplyAdd(x, y, z));
        }

        for (int i = 0; i < 1024 * 1024; i++)
        {
            float x = Polyfill.UInt32BitsToSingle((uint)rng.Next());
            float y = Polyfill.UInt32BitsToSingle((uint)rng.Next());
            float z = Polyfill.UInt32BitsToSingle((uint)rng.Next());

            Assert.Equal(MathF.FusedMultiplyAdd(x, y, z), StrictMathF.FusedMultiplyAdd(x, y, z));
        }

        // subnormals
        for (int i = 0; i < 1024; i++)
        {
            float x = Polyfill.UInt32BitsToSingle((uint)rng.Next() & ~0x7f800000u);
            float y = Polyfill.UInt32BitsToSingle((uint)rng.Next() & ~0x7f800000u);
            float z = Polyfill.UInt32BitsToSingle((uint)rng.Next() & ~0x7f800000u);

            Assert.Equal(MathF.FusedMultiplyAdd(x, y, z), StrictMathF.FusedMultiplyAdd(x, y, z));
        }
    }

    [Fact]
    public void TestVector()
    {
        string path = "../../../Binary32/fusedMultiplyAddF.txt";

        foreach (var line in File.ReadLines(path))
        {
            var parsed = line.Split('\t');

            float x = Polyfill.UInt32BitsToSingle(uint.Parse(parsed[0], NumberStyles.HexNumber));
            float y = Polyfill.UInt32BitsToSingle(uint.Parse(parsed[1], NumberStyles.HexNumber));
            float z = Polyfill.UInt32BitsToSingle(uint.Parse(parsed[2], NumberStyles.HexNumber));
            float a = Polyfill.UInt32BitsToSingle(uint.Parse(parsed[3], NumberStyles.HexNumber));

            float actual = StrictMathF.FusedMultiplyAdd(x, y, z);
            Assert.Equal(a, actual);
        }
    }
}
