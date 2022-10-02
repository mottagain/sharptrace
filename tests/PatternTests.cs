namespace tests;
using SharpTrace;
using System.Diagnostics;

public class TestPattern : Pattern
{
    public override Color PatternAt(Tuple point)
    {
        Debug.Assert(point.IsPoint);

        return new Color(point.x, point.y, point.z);
    }
}

public class PatternTests
{
    [Fact]
    public void DefaultPatternTransformation()
    {
        var p = new TestPattern();

        Assert.True(p.Transform == Matrix.Identity(4));
    }

    [Fact]
    public void AssignPatternTransformation()
    {
        var p = new TestPattern();
        p.Transform = Matrix.Translation(1, 2, 3);

        Assert.True(p.Transform == Matrix.Translation(1, 2, 3));
    }

    [Fact]
    public void PatternWithObjectTransformation()
    {
        var shape = new Sphere();
        shape.Transform = Matrix.Scaling(2, 2, 2);
        var pattern = new TestPattern();

        var c = pattern.PatternAtShape(shape, Tuple.NewPoint(2, 3, 4));

        Assert.True(c == new Color(1, 1.5f, 2));
    }

    [Fact]
    public void PatternWithPatternTransformation()
    {
        var shape = new Sphere();
        var pattern = new TestPattern();
        pattern.Transform = Matrix.Scaling(2, 2, 2);

        var c = pattern.PatternAtShape(shape, Tuple.NewPoint(2, 3, 4));

        Assert.True(c == new Color(1, 1.5f, 2));
    }

    [Fact]
    public void PatternWithBothObjectAndPatternTransformation()
    {
        var shape = new Sphere();
        shape.Transform = Matrix.Scaling(2, 2, 2);
        var pattern = new TestPattern();
        pattern.Transform = Matrix.Translation(0.5f, 1, 1.5f);

        var c = pattern.PatternAtShape(shape, Tuple.NewPoint(2.5f, 3, 3.5f));

        Assert.True(c == new Color(0.75f, 0.5f, 0.25f));
    }

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

        Assert.True(p.PatternAt(Tuple.NewPoint(0, 0, 0)) == Color.White);
        Assert.True(p.PatternAt(Tuple.NewPoint(0, 1, 0)) == Color.White);
        Assert.True(p.PatternAt(Tuple.NewPoint(0, 2, 0)) == Color.White);
    }

    [Fact]
    public void StipePatternIsConstantInZ()
    {
        var p = new StripePattern(Color.White, Color.Black);

        Assert.True(p.PatternAt(Tuple.NewPoint(0, 0, 0)) == Color.White);
        Assert.True(p.PatternAt(Tuple.NewPoint(0, 0, 1)) == Color.White);
        Assert.True(p.PatternAt(Tuple.NewPoint(0, 0, 2)) == Color.White);
    }

    [Fact]
    public void StripePatternAlternatesInX()
    {
        var p = new StripePattern(Color.White, Color.Black);

        Assert.True(p.PatternAt(Tuple.NewPoint(0, 0, 0)) == Color.White);
        Assert.True(p.PatternAt(Tuple.NewPoint(0.9f, 0, 0)) == Color.White);
        Assert.True(p.PatternAt(Tuple.NewPoint(1, 0, 0)) == Color.Black);
        Assert.True(p.PatternAt(Tuple.NewPoint(-0.1f, 0, 0)) == Color.Black);
        Assert.True(p.PatternAt(Tuple.NewPoint(-1, 0, 0)) == Color.Black);
        Assert.True(p.PatternAt(Tuple.NewPoint(-1.1f, 0, 0)) == Color.White);
    }

    [Fact]
    public void GradientLinearlyInterpolatesBetweenColors()
    {
        var p = new GradientPattern(Color.White, Color.Black);

        Assert.True(p.PatternAt(Tuple.NewPoint(0, 0, 0)) == Color.White);
        Assert.True(p.PatternAt(Tuple.NewPoint(0.25f, 0, 0)) == new Color(0.75f, 0.75f, 0.75f));
        Assert.True(p.PatternAt(Tuple.NewPoint(0.5f, 0, 0)) == new Color(0.5f, 0.5f, 0.5f));
        Assert.True(p.PatternAt(Tuple.NewPoint(0.75f, 0, 0)) == new Color(0.25f, 0.25f, 0.25f));
    }

    [Fact]
    public void RingShouldExtendInBothXAndZ()
    {
        var p = new RingPattern(Color.White, Color.Black);

        Assert.True(p.PatternAt(Tuple.NewPoint(0, 0, 0)) == Color.White);
        Assert.True(p.PatternAt(Tuple.NewPoint(1, 0, 0)) == Color.Black);
        Assert.True(p.PatternAt(Tuple.NewPoint(0, 0, 1)) == Color.Black);
        Assert.True(p.PatternAt(Tuple.NewPoint(0.708f, 0, 0.708f)) == Color.Black);
    }

    [Fact]
    public void CheckersShouldRepeatInX()
    {
        var p = new CheckerPattern(Color.White, Color.Black);

        Assert.True(p.PatternAt(Tuple.NewPoint(0, 0, 0)) == Color.White);
        Assert.True(p.PatternAt(Tuple.NewPoint(0.99f, 0, 0)) == Color.White);
        Assert.True(p.PatternAt(Tuple.NewPoint(1.01f, 0, 0)) == Color.Black);
    }

    [Fact]
    public void CheckersShouldRepeatInY()
    {
        var p = new CheckerPattern(Color.White, Color.Black);

        Assert.True(p.PatternAt(Tuple.NewPoint(0, 0, 0)) == Color.White);
        Assert.True(p.PatternAt(Tuple.NewPoint(0, 0.99f, 0)) == Color.White);
        Assert.True(p.PatternAt(Tuple.NewPoint(0, 1.01f, 0)) == Color.Black);
    }

    [Fact]
    public void CheckersShouldRepeatInZ()
    {
        var p = new CheckerPattern(Color.White, Color.Black);

        Assert.True(p.PatternAt(Tuple.NewPoint(0, 0, 0)) == Color.White);
        Assert.True(p.PatternAt(Tuple.NewPoint(0, 0, 0.99f)) == Color.White);
        Assert.True(p.PatternAt(Tuple.NewPoint(0, 0, 1.01f)) == Color.Black);
    }
}