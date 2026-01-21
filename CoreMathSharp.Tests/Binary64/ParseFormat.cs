namespace CoreMathSharp.Tests;

public class ParseFormat
{
#if NET10_0_OR_GREATER
    [Fact]
    public void Parse()
    {
        Assert.Equal(double.E, StrictMath.ParseHex("0x1.5bf0a8b145769p+1"));
        Assert.Equal(double.Epsilon, StrictMath.ParseHex("0x1.0000000000000p-1074"));
        Assert.Equal(double.MaxValue, StrictMath.ParseHex("0x1.fffffffffffffp+1023"));
        Assert.Equal(double.MinValue, StrictMath.ParseHex("-0x1.fffffffffffffp+1023"));
        Assert.Equal(double.NaN, StrictMath.ParseHex("NaN"));
        Assert.Equal(double.NegativeInfinity, StrictMath.ParseHex("-∞"));
        Assert.Equal(double.NegativeZero, StrictMath.ParseHex("-0x0.0000000000000p+0"));
        Assert.Equal(double.Pi, StrictMath.ParseHex("0x1.921fb54442d18p+1"));
        Assert.Equal(double.PositiveInfinity, StrictMath.ParseHex("∞"));
        Assert.Equal(double.Tau, StrictMath.ParseHex("0x1.921fb54442d18p+2"));
        Assert.Equal(-1.0, StrictMath.ParseHex("-0x1.0000000000000p+0"));
        Assert.Equal(0.0, StrictMath.ParseHex("0x0.0000000000000p+0"));
        Assert.Equal(1.0, StrictMath.ParseHex("0x1.0000000000000p+0"));
    }

    [Fact]
    public void Format()
    {
        Assert.Equal("0x1.5bf0a8b145769p+1", StrictMath.FormatHex(double.E));
        Assert.Equal("0x1.0000000000000p-1074", StrictMath.FormatHex(double.Epsilon));
        Assert.Equal("0x1.fffffffffffffp+1023", StrictMath.FormatHex(double.MaxValue));
        Assert.Equal("-0x1.fffffffffffffp+1023", StrictMath.FormatHex(double.MinValue));
        Assert.Equal("NaN", StrictMath.FormatHex(double.NaN));
        Assert.Equal("-∞", StrictMath.FormatHex(double.NegativeInfinity));
        Assert.Equal("-0x0.0000000000000p+0", StrictMath.FormatHex(double.NegativeZero));
        Assert.Equal("0x1.921fb54442d18p+1", StrictMath.FormatHex(double.Pi));
        Assert.Equal("∞", StrictMath.FormatHex(double.PositiveInfinity));
        Assert.Equal("0x1.921fb54442d18p+2", StrictMath.FormatHex(double.Tau));
        Assert.Equal("-0x1.0000000000000p+0", StrictMath.FormatHex(-1.0));
        Assert.Equal("0x0.0000000000000p+0", StrictMath.FormatHex(0.0));
        Assert.Equal("0x1.0000000000000p+0", StrictMath.FormatHex(1.0));
    }

    [Fact]
    public void ParseAndFormat()
    {
        var rng = new Seiran(1, 1);

        for (int i = 0; i < 1024; i++)
        {
            double expected = BitConverter.UInt64BitsToDouble(rng.Next());

            if (!double.IsFinite(expected))
            {
                continue;
            }

            string formatted = StrictMath.FormatHex(expected);
            double parsed = StrictMath.ParseHex(formatted);

            Assert.Equal(expected, parsed);
        }
    }
#endif
}