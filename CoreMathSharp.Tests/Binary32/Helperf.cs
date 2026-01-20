namespace CoreMathSharp.Tests;

public class Helperf
{
    [Fact]
    public void Parse()
    {
        Assert.Equal(float.E, StrictMathF.ParseHex("0x1.5bf0a8p+1f"));
        Assert.Equal(float.Epsilon, StrictMathF.ParseHex("0x1.000000p-149f"));
        Assert.Equal(float.MaxValue, StrictMathF.ParseHex("0x1.fffffep+127f"));
        Assert.Equal(float.MinValue, StrictMathF.ParseHex("-0x1.fffffep+127f"));
        Assert.Equal(float.NaN, StrictMathF.ParseHex("NaN"));
        Assert.Equal(float.NegativeInfinity, StrictMathF.ParseHex("-∞"));
        Assert.Equal(float.NegativeZero, StrictMathF.ParseHex("-0x0.000000p+0f"));
        Assert.Equal(float.Pi, StrictMathF.ParseHex("0x1.921fb6p+1f"));
        Assert.Equal(float.PositiveInfinity, StrictMathF.ParseHex("∞"));
        Assert.Equal(float.Tau, StrictMathF.ParseHex("0x1.921fb6p+2f"));
        Assert.Equal(-1f, StrictMathF.ParseHex("-0x1.000000p+0f"));
        Assert.Equal(0f, StrictMathF.ParseHex("0x0.000000p+0f"));
        Assert.Equal(1f, StrictMathF.ParseHex("0x1.000000p+0f"));
    }

    [Fact]
    public void Format()
    {
        Assert.Equal("0x1.5bf0a8p+1f", StrictMathF.FormatHex(float.E));
        Assert.Equal("0x1.000000p-149f", StrictMathF.FormatHex(float.Epsilon));
        Assert.Equal("0x1.fffffep+127f", StrictMathF.FormatHex(float.MaxValue));
        Assert.Equal("-0x1.fffffep+127f", StrictMathF.FormatHex(float.MinValue));
        Assert.Equal("NaN", StrictMathF.FormatHex(float.NaN));
        Assert.Equal("-∞", StrictMathF.FormatHex(float.NegativeInfinity));
        Assert.Equal("-0x0.000000p+0f", StrictMathF.FormatHex(float.NegativeZero));
        Assert.Equal("0x1.921fb6p+1f", StrictMathF.FormatHex(float.Pi));
        Assert.Equal("∞", StrictMathF.FormatHex(float.PositiveInfinity));
        Assert.Equal("0x1.921fb6p+2f", StrictMathF.FormatHex(float.Tau));
        Assert.Equal("-0x1.000000p+0f", StrictMathF.FormatHex(-1f));
        Assert.Equal("0x0.000000p+0f", StrictMathF.FormatHex(0f));
        Assert.Equal("0x1.000000p+0f", StrictMathF.FormatHex(1f));
    }

    [Fact]
    public void ParseFormat()
    {
        var rng = new Seiran(1, 1);

        for (int i = 0; i < 1024; i++)
        {
            float expected = BitConverter.UInt32BitsToSingle((uint)rng.Next());

            if (!float.IsFinite(expected))
            {
                continue;
            }

            string formatted = StrictMathF.FormatHex(expected);
            float parsed = StrictMathF.ParseHex(formatted);

            Assert.Equal(expected, parsed);
        }
    }
}