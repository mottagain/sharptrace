
namespace tests;
using SharpTrace;

public class RayTests
{
    [Fact]
    public void CreatingAndQueryingARay()
    {
        var origin = Tuple.NewPoint(1, 2, 3);
        var direction = Tuple.NewVector(4, 5, 6);

        var r = new Ray(origin, direction);
        
        Assert.True(r.Origin == origin, "Ray returns the origin it was constructed with.");
        Assert.True(r.Direction == direction, "Ray returns the direction it was constructed with.");
    }

    [Fact]
    public void CreatePointFromDistance()
    {
        var r = new Ray(Tuple.NewPoint(2, 3, 4), Tuple.NewVector(1, 0, 0));

        Assert.True(r.Position(0) == Tuple.NewPoint(2, 3, 4));
        Assert.True(r.Position(1) == Tuple.NewPoint(3, 3, 4));
        Assert.True(r.Position(-1) == Tuple.NewPoint(1, 3, 4));
        Assert.True(r.Position(2.5f) == Tuple.NewPoint(4.5f, 3, 4));
    }

    [Fact]
    public void IntersectionsHitWhenAllHavePositiveT()
    {
        var s = new Sphere();
        var i1 = new Intersection(1, s);
        var i2 = new Intersection(2, s);

        var xs = new Intersections() { i1, i2 };

        Assert.True(xs.Hit() == i1, "Hit is the closest positive t.");
    }

    [Fact]
    public void IntersectionsHitWhenSomeHaveNegativeT()
    {
        var s = new Sphere();
        var i1 = new Intersection(-1, s);
        var i2 = new Intersection(1, s);

        var xs = new Intersections() { i1, i2 };

        Assert.True(xs.Hit() == i2, "Hit is the closest positive t.");
    }

    [Fact]
    public void IntersectionsHitWhenAllHaveNegativeT()
    {
        var s = new Sphere();
        var i1 = new Intersection(-2, s);
        var i2 = new Intersection(-1, s);

        var xs = new Intersections() { i1, i2 };

        Assert.True(xs.Hit() == null, "No intersection is a hit.");
    }

    [Fact]
    public void IntersectionsHitIsLowestNonnegative()
    {
        var s = new Sphere();
        var i1 = new Intersection(5, s);
        var i2 = new Intersection(7, s);
        var i3 = new Intersection(-3, s);
        var i4 = new Intersection(2, s);

        var xs = new Intersections() { i1, i2, i3, i4 };

        Assert.True(xs.Hit() == i4, "His is the lowest non-negative t.");
    }

    [Fact]
    public void TranslateRay() 
    {
        var r = new Ray(Tuple.NewPoint(1, 2, 3), Tuple.NewVector(0, 1, 0));
        var m = Matrix.Translation(3, 4, 5);

        var r2 = r.Transform(m);

        Assert.True(r2.Origin == Tuple.NewPoint(4, 6, 8), "Origin is appropriately translated.");
        Assert.True(r2.Direction == Tuple.NewVector(0, 1, 0), "Direction is not impacted by translation.");
    }

    [Fact]
    public void ScaleRay() 
    {
        var r = new Ray(Tuple.NewPoint(1, 2, 3), Tuple.NewVector(0, 1, 0));
        var m = Matrix.Scaling(2, 3, 4);

        var r2 = r.Transform(m);

        Assert.True(r2.Origin == Tuple.NewPoint(2, 6, 12), "Origin is appropriately scaled.");
        Assert.True(r2.Direction == Tuple.NewVector(0, 3, 0), "Direction is appropriately scaled.");
    }
}