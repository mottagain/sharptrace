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

    [Fact]
    public void PrecomputeReflectionVector()
    {
        var shape = new Plane();
        var r = new Ray(Tuple.NewPoint(0, 1, -1), Tuple.NewVector(0, -MathExt.Sqrt2Over2, MathExt.Sqrt2Over2));
        var i = new Intersection((float)Math.Sqrt(2), shape);

        var comps = i.PrepareComputations(r);

        Assert.True(comps.ReflectVector == Tuple.NewVector(0, MathExt.Sqrt2Over2, MathExt.Sqrt2Over2));
    }

    [Fact]
    public void FindingN1AndN2AcrossSphereIntersections()
    {
        var expectedValues = new List<(float N1, float N2)> {
          (1.0f, 1.5f),
          (1.5f, 2.0f),
          (2.0f, 2.5f),
          (2.5f, 2.5f),
          (2.5f, 1.5f),
          (1.5f, 1.0f),
        };

        var a = new Sphere(Material.Glass);
        a.Transform = Matrix.Scaling(2, 2, 2);
        a.Material.RefractiveIndex = 1.5f;
        var b = new Sphere(Material.Glass);
        b.Transform = Matrix.Translation(0, 0, -0.25f);
        b.Material.RefractiveIndex = 2f;
        var c = new Sphere(Material.Glass);
        c.Transform = Matrix.Translation(0, 0, 0.25f);
        c.Material.RefractiveIndex = 2.5f;
        var r = new Ray(Tuple.NewPoint(0, 0, -4), Tuple.NewVector(0, 0, 1));
        var xs = new Intersections { new Intersection(2f, a), new Intersection(2.75f, b), new Intersection(3.25f, c), new Intersection(4.75f, b), new Intersection(5.25f, c), new Intersection(6, a) };

        Assert.True(xs.Count == 6);

        for (int index = 0; index < xs.Count; index++)
        {
            var comps = xs[index].PrepareComputations(r, xs);

            Assert.True(comps.N1 == expectedValues[index].N1);
            Assert.True(comps.N2 == expectedValues[index].N2);
        }
    }

    [Fact]
    public void UnderPointIsOffsetBelowTheSurface() 
    {
        var r = new Ray(Tuple.NewPoint(0, 0, -5), Tuple.NewVector(0, 0, 1));
        var shape = new Sphere(Material.Glass);
        shape.Transform = Matrix.Translation(0, 0, 1);
        var i = new Intersection(5, shape);
        var xs = new Intersections { i };

        var comps = i.PrepareComputations(r, xs);

        Assert.True(comps.UnderPoint.z > MathExt.Epsilon / 2f);
        Assert.True(comps.Point.z < comps.UnderPoint.z);
    }

    [Fact]
    public void SchlickApproximationUnderTotalInternalReflection() 
    {
        var shape = new Sphere(Material.Glass);
        var r = new Ray(Tuple.NewPoint(0, 0, MathExt.Sqrt2Over2), Tuple.NewVector(0, 1, 0));
        var xs = new Intersections { new Intersection(-MathExt.Sqrt2Over2, shape), new Intersection(MathExt.Sqrt2Over2, shape) };

        var comps = xs[1].PrepareComputations(r, xs);
        var reflectance = comps.Schlick();

        Assert.True(MathExt.Near(reflectance, 1f));
    }

    [Fact]
    public void SchlickApproximationWithPerpendicularViewingAngle()
    {
        var shape = new Sphere(Material.Glass);
        var r = new Ray(Tuple.NewPoint(0, 0, 0), Tuple.NewVector(0, 1, 0));
        var xs = new Intersections { new Intersection(-1, shape), new Intersection(1, shape) };

        var comps = xs[1].PrepareComputations(r, xs);
        var reflectance = comps.Schlick();

        Assert.True(MathExt.Near(reflectance, 0.04f));
    }

    [Fact]
    public void SchlickApproximationWithSmallAngleAndN2GreaterThanN1()
    {
        var shape = new Sphere(Material.Glass);
        var r = new Ray(Tuple.NewPoint(0, 0.99f, -2), Tuple.NewVector(0, 0, 1));
        var xs = new Intersections { new Intersection(1.8589f, shape) };

        var comps = xs[0].PrepareComputations(r, xs);
        var reflectance = comps.Schlick();

        Assert.True(MathExt.Near(reflectance, 0.48873f));
    }
}