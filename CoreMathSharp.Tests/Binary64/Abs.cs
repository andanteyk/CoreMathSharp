namespace CoreMathSharp.Tests;

public class Abs
{
    [Fact]
    public void TestDoubles()
    {
        foreach (var x in Helper.TestDoubles)
        {
            Assert.Equal(Math.Abs(x), StrictMath.Abs(x));
        }
    }

    [Fact]
    public void Random()
    {
        var rng = new Seiran(1, 1);

        for (int i = 0; i < 1024; i++)
        {
            double x = Polyfill.UInt64BitsToDouble(rng.Next());

            Assert.Equal(Math.Abs(x), StrictMath.Abs(x));
        }
    }
}
