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

    [Fact]
    public void StripesWithObjectTransformation() 
    {
        var obj = new Sphere();
        obj.Transform = Matrix.Scaling(2, 2, 2);
        var pattern = new StripePattern(Color.White, Color.Black);

        var c = pattern.StripeAtObject(obj, Tuple.NewPoint(1.5f, 0, 0));

        Assert.True(c == Color.White);
    }

    [Fact]
    public void StripesWithPatternTransformation()
    {
        var obj = new Sphere();
        var pattern = new StripePattern(Color.White, Color.Black);
        pattern.Transform = Matrix.Scaling(2, 2, 2);

        var c = pattern.StripeAtObject(obj, Tuple.NewPoint(1.5f, 0, 0));

        Assert.True(c == Color.White);
    }

    [Fact]
    public void StripesWithBothObjectAndPatternTransformation()
    {
        var obj = new Sphere();
        obj.Transform = Matrix.Scaling(2, 2, 2);
        var pattern = new StripePattern(Color.White, Color.Black);
        pattern.Transform = Matrix.Translation(0.5f, 0, 0);

        var c = pattern.StripeAtObject(obj, Tuple.NewPoint(2.5f, 0, 0));

        Assert.True(c == Color.White);
    }

}