namespace CoreMathSharp.Tests;

public class CopySignF
{
    [Fact]
    public void TestFloats()
    {
        foreach (var y in Helper.TestFloats)
        {
            foreach (var x in Helper.TestFloats)
            {
                Assert.Equal(MathF.CopySign(x, y), StrictMathF.CopySign(x, y));
            }
        }
    }

    [Fact]
    public void Random()
    {
        var rng = new Seiran(1, 1);

        for (int i = 0; i < 1024 * 1024; i++)
        {
            float x = Polyfill.UInt32BitsToSingle((uint)rng.Next());
            float y = Polyfill.UInt32BitsToSingle((uint)rng.Next());

            Assert.Equal(MathF.CopySign(x, y), StrictMathF.CopySign(x, y));
        }
    }
}
