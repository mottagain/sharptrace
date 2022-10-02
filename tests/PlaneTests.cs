namespace tests;
using SharpTrace;

public class PlaneTests
{
    [Fact]
    public void PlaneConstruction()
    {
        var p = new Plane();

        var n1 = p.LocalNormalAt(Tuple.NewPoint(0, 0, 0));
        var n2 = p.LocalNormalAt(Tuple.NewPoint(10, 0, -10));
        var n3 = p.LocalNormalAt(Tuple.NewPoint(-5, 0, 150));

        Assert.True(n1 == Tuple.NewVector(0, 1, 0));
        Assert.True(n2 == Tuple.NewVector(0, 1, 0));
        Assert.True(n3 == Tuple.NewVector(0, 1, 0));
    }

    [Fact]
    public void PlaneIsShape()
    {
        var p = new Plane();

        Assert.True(p is Shape);
    }

    [Fact]
    public void IntersectWithARayParallelToThePlane()
    {
        var p = new Plane();
        var r = new Ray(Tuple.NewPoint(0, 10, 0), Tuple.NewVector(0, 0, 1));

        var xs = p.LocalIntersects(r);

        Assert.True(xs.Count == 0);
    }

    [Fact]
    public void IntersectWithACoplanarRay()
    {
        var p = new Plane();
        var r = new Ray(Tuple.NewPoint(0, 0, 0), Tuple.NewVector(0, 0, 1));

        var xs = p.LocalIntersects(r);

        Assert.True(xs.Count == 0);
    }

    [Fact]
    public void IntersectPlanFromAbove()
    {
        var p = new Plane();
        var r = new Ray(Tuple.NewPoint(0, 1, 0), Tuple.NewVector(0, -1, 0));

        var xs = p.LocalIntersects(r);

        Assert.True(xs.Count == 1);
        Assert.True(MathExt.Near(xs[0].Time, 1));
        Assert.True(xs[0].Object == p);
    }

    [Fact]
    public void IntersectPlanFromBelow()
    {
        var p = new Plane();
        var r = new Ray(Tuple.NewPoint(0, -1, 0), Tuple.NewVector(0, 1, 0));

        var xs = p.LocalIntersects(r);

        Assert.True(xs.Count == 1);
        Assert.True(MathExt.Near(xs[0].Time, 1));
        Assert.True(xs[0].Object == p);
    }
}