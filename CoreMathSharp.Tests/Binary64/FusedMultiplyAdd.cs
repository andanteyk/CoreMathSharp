using System.Globalization;

namespace CoreMathSharp.Tests;

public class FusedMultiplyAdd
{
    [Fact]
    public void TestDoubles()
    {
        foreach (var z in Helper.TestDoubles)
        {
            foreach (var y in Helper.TestDoubles)
            {
                foreach (var x in Helper.TestDoubles)
                {
                    Assert.Equal(Math.FusedMultiplyAdd(x, y, z), StrictMath.FusedMultiplyAdd(x, y, z));
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
            double x = rng.NextSignedDouble();
            double y = rng.NextSignedDouble();
            double z = rng.NextSignedDouble();

            Assert.Equal(Math.FusedMultiplyAdd(x, y, z), StrictMath.FusedMultiplyAdd(x, y, z));
        }

        for (int i = 0; i < 1024 * 1024; i++)
        {
            double x = Polyfill.UInt64BitsToDouble(rng.Next());
            double y = Polyfill.UInt64BitsToDouble(rng.Next());
            double z = Polyfill.UInt64BitsToDouble(rng.Next());

            Assert.Equal(Math.FusedMultiplyAdd(x, y, z), StrictMath.FusedMultiplyAdd(x, y, z));
        }

        // subnormals
        for (int i = 0; i < 1024; i++)
        {
            double x = Polyfill.UInt64BitsToDouble(rng.Next() & ~0x7ff0000000000000ul);
            double y = Polyfill.UInt64BitsToDouble(rng.Next() & ~0x7ff0000000000000ul);
            double z = Polyfill.UInt64BitsToDouble(rng.Next() & ~0x7ff0000000000000ul);

            Assert.Equal(Math.FusedMultiplyAdd(x, y, z), StrictMath.FusedMultiplyAdd(x, y, z));
        }
    }

    [Fact]
    public void TestVector()
    {
        string path = "../../../Binary64/fusedMultiplyAdd.txt";

        foreach (var line in File.ReadLines(path))
        {
            var parsed = line.Split('\t');

            double x = Polyfill.UInt64BitsToDouble(ulong.Parse(parsed[0], NumberStyles.HexNumber));
            double y = Polyfill.UInt64BitsToDouble(ulong.Parse(parsed[1], NumberStyles.HexNumber));
            double z = Polyfill.UInt64BitsToDouble(ulong.Parse(parsed[2], NumberStyles.HexNumber));
            double a = Polyfill.UInt64BitsToDouble(ulong.Parse(parsed[3], NumberStyles.HexNumber));

            double actual = StrictMath.FusedMultiplyAdd(x, y, z);
            Assert.Equal(a, actual);
        }
    }
}
