namespace tests;
using SharpTrace;

public class PatternTests
{
    [Fact]
    public void CreatingAStripePattern()
    {
        var p = new StripePattern(Color.White, Color.Black);

        Assert.True(p.A == Color.White);
        Assert.True(p.B == Color.Black);
    }

    [Fact]
    public void StipePatternIsConstantInY()
    {
        var p = new StripePattern(Color.White, Color.Black);

        Assert.True(p.StripeAt(Tuple.NewPoint(0, 0, 0)) == Color.White);
        Assert.True(p.StripeAt(Tuple.NewPoint(0, 1, 0)) == Color.White);
        Assert.True(p.StripeAt(Tuple.NewPoint(0, 2, 0)) == Color.White);
    }

    [Fact]
    public void StipePatternIsConstantInZ()
    {
        var p = new StripePattern(Color.White, Color.Black);

        Assert.True(p.StripeAt(Tuple.NewPoint(0, 0, 0)) == Color.White);
        Assert.True(p.StripeAt(Tuple.NewPoint(0, 0, 1)) == Color.White);
        Assert.True(p.StripeAt(Tuple.NewPoint(0, 0, 2)) == Color.White);
    }

    [Fact]
    public void StripePatternAlternatesInX()
    {
        var p = new StripePattern(Color.White, Color.Black);

        Assert.True(p.StripeAt(Tuple.NewPoint(0, 0, 0)) == Color.White);
        Assert.True(p.StripeAt(Tuple.NewPoint(0.9f, 0, 0)) == Color.White);
        Assert.True(p.StripeAt(Tuple.NewPoint(1, 0, 0)) == Color.Black);
        Assert.True(p.StripeAt(Tuple.NewPoint(-0.1f, 0, 0)) == Color.Black);
        Assert.True(p.StripeAt(Tuple.NewPoint(-1, 0, 0)) == Color.Black);
        Assert.True(p.StripeAt(Tuple.NewPoint(-1.1f, 0, 0)) == Color.White);
    }
}