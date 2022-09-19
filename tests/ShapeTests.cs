
namespace tests;
using SharpTrace;

class TestShape : Shape 
{
    public override Intersections LocalIntersects(Ray r) 
    {
        this.LocalRay = r;
        return new Intersections();
    }

    public override Tuple LocalNormalAt(Tuple objectPoint)
    {
        this.LocalPoint = objectPoint;
        return Tuple.NewPoint(0, 0, 0);
    }


    public Ray? LocalRay { get; set; }
    public Tuple? LocalPoint { get; set; }

}

public class ShapeTests
{
    [Fact]
    public void ShapeHasDefaultTransform()
    {
        var s = new TestShape();

        Assert.True(s.Transform == Matrix.Identity(4));
    }

    [Fact]
    public void ChangingAShapesTransform()
    {
        var s = new TestShape();

        var t = Matrix.Translation(2, 3, 4);

        s.Transform = t;

        Assert.True(s.Transform == t, "Setting a transform yeilds expected value.");
    }

    [Fact]
    public void ShapeHasDefaultMaterial() 
    {
        var s = new TestShape();

        var m = s.Material;

        Assert.True(m.Specular == 0.9f, "Spot check shape is constructed with a default material.");
    }

    [Fact]
    public void ShapeMayBeAssignedMaterial() 
    {
        var s = new TestShape();
        var m = new Material();

        s.Material = m;

        Assert.True(s.Material == m, "Shape may be assigned a material.");
    }

    [Fact]
    public void IntersectingAScaledShapeWithRay()
    {
        var r = new Ray(Tuple.NewPoint(0, 0, -5), Tuple.NewVector(0, 0, 1));
        var s = new TestShape();

        s.Transform = Matrix.Scaling(2, 2, 2);
        var xs = s.Intersects(r);

        Assert.True(s.LocalRay!.Origin == Tuple.NewPoint(0, 0, -2.5f));
        Assert.True(s.LocalRay!.Direction == Tuple.NewVector(0, 0, 0.5f));
    }

    [Fact]
    public void IntersectingATranslatedShapeWithRay()
    {
        var r = new Ray(Tuple.NewPoint(0, 0, -5), Tuple.NewVector(0, 0, 1));
        var s = new TestShape();

        s.Transform = Matrix.Translation(5, 0, 0);
        var xs = s.Intersects(r);

        Assert.True(s.LocalRay!.Origin == Tuple.NewPoint(-5, 0, -5));
        Assert.True(s.LocalRay!.Direction == Tuple.NewVector(0, 0, 1));
    }

}