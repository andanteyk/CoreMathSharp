namespace CoreMathSharp.Tests;

public class SqrtF
{
    [Fact]
    public void TestFloats()
    {
        foreach (var x in Helper.TestFloats)
        {
            Assert.Equal(MathF.Sqrt(x), StrictMathF.Sqrt(x));
        }
    }

    [Fact]
    public void Random()
    {
        var rng = new Seiran(1, 1);

        for (int i = 0; i < 1024; i++)
        {
            float x = rng.NextFloat() * 2;

            Assert.Equal(MathF.Sqrt(x), StrictMathF.Sqrt(x));
        }
    }
}
