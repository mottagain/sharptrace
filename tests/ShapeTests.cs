
namespace tests;
using SharpTrace;

class TestShape : Shape 
{
    public override Intersections LocalIntersects(Ray r) 
    {
        return new Intersections();
    }

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

}
