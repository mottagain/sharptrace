namespace tests;
using SharpTrace;

public class IntersectionTests
{
    [Fact]
    public void PrecomputeStateOfIntersection()
    {
        var r = new Ray(Tuple.NewPoint(0, 0, -5), Tuple.NewVector(0, 0, 1));
        var shape = new Sphere();
        var i = new Intersection(4, shape);

        var comps = i.PrepareComputations(r);

        Assert.True(MathExt.Near(comps.Time, i.Time));
        Assert.True(comps.Object == i.Object);
        Assert.True(comps.Point == Tuple.NewPoint(0, 0, -1));
        Assert.True(comps.EyeVector == Tuple.NewVector(0, 0, -1));
        Assert.True(comps.NormalVector == Tuple.NewVector(0, 0, -1));
    }

    [Fact]
    public void HitWhenAnIntersectionOccursOnOutside()
    {
        var r = new Ray(Tuple.NewPoint(0, 0, -5), Tuple.NewVector(0, 0, 1));
        var shape = new Sphere();
        var i = new Intersection(4, shape);

        var comps = i.PrepareComputations(r);

        Assert.False(comps.Inside);
    }

    [Fact]
    public void HitWhenAnIntersectionOccursOnInside()
    {
        var r = new Ray(Tuple.NewPoint(0, 0, 0), Tuple.NewVector(0, 0, 1));
        var shape = new Sphere();
        var i = new Intersection(1, shape);

        var comps = i.PrepareComputations(r);

        Assert.True(comps.Point == Tuple.NewPoint(0, 0, 1));
        Assert.True(comps.EyeVector == Tuple.NewVector(0, 0, -1));
        Assert.True(comps.Inside);
        Assert.True(comps.NormalVector == Tuple.NewVector(0, 0, -1));
    }

    [Fact]
    public void HitShouldOffsetPoint()
    {
        var r = new Ray(Tuple.NewPoint(0, 0, -5), Tuple.NewVector(0, 0, 1));
        var s = new Sphere();
        s.Transform = Matrix.Translation(0, 0, 1);
        var i = new Intersection(5, s);
        
        var comps = i.PrepareComputations(r);

        Assert.True(comps.OverPoint.z < -MathExt.Epsilon / 2f);
    }
}