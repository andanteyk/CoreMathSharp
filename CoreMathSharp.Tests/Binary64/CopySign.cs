namespace CoreMathSharp.Tests;

public class CopySign
{
    [Fact]
    public void TestDoubles()
    {
        foreach (var y in Helper.TestDoubles)
        {
            foreach (var x in Helper.TestDoubles)
            {
                Assert.Equal(Math.CopySign(x, y), StrictMath.CopySign(x, y));
            }
        }
    }

    [Fact]
    public void Random()
    {
        var rng = new Seiran(1, 1);

        for (int i = 0; i < 1024; i++)
        {
            double x = StrictMath.UInt64BitsToDouble(rng.Next());
            double y = StrictMath.UInt64BitsToDouble(rng.Next());

            Assert.Equal(Math.CopySign(x, y), StrictMath.CopySign(x, y));
        }
    }
}
