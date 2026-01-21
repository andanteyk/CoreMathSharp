namespace CoreMathSharp.Tests;

public class Sqrt
{
    [Fact]
    public void TestDoubles()
    {
        foreach (var x in Helper.TestDoubles)
        {
            Assert.Equal(Math.Sqrt(x), StrictMath.Sqrt(x));
        }
    }

    [Fact]
    public void Random()
    {
        var rng = new Seiran(1, 1);

        for (int i = 0; i < 1024; i++)
        {
            double x = rng.NextDouble() * 2;

            Assert.Equal(Math.Sqrt(x), StrictMath.Sqrt(x));
        }
    }
}
