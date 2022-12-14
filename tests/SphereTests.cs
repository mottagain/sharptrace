
namespace tests;
using SharpTrace;

public class SphereTests
{
    [Fact]
    public void SphereDefaultConstructor()
    {
        var s = new Sphere();

        Assert.True(s.Origin == Tuple.NewPoint(0, 0, 0), "Default sphere origin is 0, 0, 0.");
        Assert.True(MathExt.Near(s.Radius, 1f), "Default sphere radius is 1.");
    }

    [Fact]
    public void SphereIsAShape()
    {
        var s = new Sphere();
        Assert.True(s is Shape);
    }

    [Fact]
    public void RayIntersectsSphereAtTwoPoints()
    {
        var r = new Ray(Tuple.NewPoint(0, 0, -5), Tuple.NewVector(0, 0, 1));
        var s = new Sphere();

        var xs = s.LocalIntersects(r);

        Assert.True(xs[0].Time == 4.0f, "First intersection is at t = 4.0.");
        Assert.True(xs[0].Object == s, "Interesected object is correct.");
        Assert.True(xs[1].Time == 6.0f, "Second intersection is at t = 6.0.");
        Assert.True(xs[1].Object == s, "Interesected object is correct.");
    }

    [Fact]
    public void TangentialRayIntersectsSphereAtOnePoint()
    {
        var r = new Ray(Tuple.NewPoint(0, 1, -5), Tuple.NewVector(0, 0, 1));
        var s = new Sphere();

        var xs = s.LocalIntersects(r);

        Assert.True(xs[0].Time == 5.0f, "First intersection is at t = 5.0.");
        Assert.True(xs[1].Time == 5.0f, "Second intersection is at t = 5.0.");
    }

    [Fact]
    public void RayDoesNotIntersectSphere()
    {
        var r = new Ray(Tuple.NewPoint(0, 2, -5), Tuple.NewVector(0, 0, 1));
        var s = new Sphere();

        var xs = s.LocalIntersects(r);

        Assert.True(xs.Count == 0, "Intersects returned no intersections.");
    }

    [Fact]
    public void RayOriginatingInSphereStillIntersectsSphereAtTwoPoints()
    {
        var r = new Ray(Tuple.NewPoint(0, 0, 0), Tuple.NewVector(0, 0, 1));
        var s = new Sphere();

        var xs = s.LocalIntersects(r);

        Assert.True(xs[0].Time == -1.0f, "First intersection is at t = -1.0.");
        Assert.True(xs[1].Time == 1.0f, "Second intersection is at t = 1.0.");
    }

    [Fact]
    public void RayOriginatingAheadOfSphereStillIntersectsSphereAtTwoPoints()
    {
        var r = new Ray(Tuple.NewPoint(0, 0, 5), Tuple.NewVector(0, 0, 1));
        var s = new Sphere();

        var xs = s.LocalIntersects(r);

        Assert.True(xs[0].Time == -6.0f, "First intersection is at t = -6.0.");
        Assert.True(xs[1].Time == -4.0f, "Second intersection is at t = -4.0.");
    }

    [Fact]
    public void RayIntersectsScaledSphere()
    {
        var r = new Ray(Tuple.NewPoint(0, 0, -5), Tuple.NewVector(0, 0, 1));
        var s = new Sphere();

        s.Transform = Matrix.Scaling(2, 2, 2);
        var xs = s.Intersects(r);

        Assert.True(xs.Count == 2, "Two intersections are expected.");
        Assert.True(xs[0].Time == 3.0f, "First intersection is at t = 6.0.");
        Assert.True(xs[1].Time == 7.0f, "Second intersection is at t = 6.0.");
    }

    [Fact]
    public void RayDoesNotIntersectScaledSphere()
    {
        var r = new Ray(Tuple.NewPoint(0, 0, -5), Tuple.NewVector(0, 0, 1));
        var s = new Sphere();

        s.Transform = Matrix.Translation(5, 0, 0);
        var xs = s.Intersects(r);

        Assert.True(xs.Count == 0, "No intersections are expected.");
    }

    [Fact]
    public void NormalOnSphereAtPointOnXAxis()
    {
        var s = new Sphere();

        var n = s.LocalNormalAt(Tuple.NewPoint(1, 0, 0));

        Assert.True(n == Tuple.NewVector(1, 0, 0), "Normal on point on x axis should be a unit vector on the x axis.");
    }

    [Fact]
    public void NormalOnSphereAtPointOnYAxis()
    {
        var s = new Sphere();

        var n = s.LocalNormalAt(Tuple.NewPoint(0, 1, 0));

        Assert.True(n == Tuple.NewVector(0, 1, 0), "Normal on point on y axis should be a unit vector on the y axis.");
    }

    [Fact]
    public void NormalOnSphereAtPointOnZAxis()
    {
        var s = new Sphere();

        var n = s.LocalNormalAt(Tuple.NewPoint(0, 0, 1));

        Assert.True(n == Tuple.NewVector(0, 0, 1), "Normal on point on z axis should be a unit vector on the z axis.");
    }

    [Fact]
    public void NormalOnSphereAtNonaxialPoint()
    {
        var s = new Sphere();

        float sqrtOf3Over3 = (float)Math.Sqrt(3.0) / 3f;
        var n = s.LocalNormalAt(Tuple.NewPoint(sqrtOf3Over3, sqrtOf3Over3, sqrtOf3Over3));

        Assert.True(n == Tuple.NewVector(sqrtOf3Over3, sqrtOf3Over3, sqrtOf3Over3), "Normal on non-axial point on sphere should be a unit vector to that point.");
    }

    [Fact]
    public void NormalOnSphereIsANormalized()
    {
        var s = new Sphere();

        float sqrtOf3Over3 = (float)Math.Sqrt(3.0) / 3f;
        var n = s.LocalNormalAt(Tuple.NewPoint(sqrtOf3Over3, sqrtOf3Over3, sqrtOf3Over3));

        Assert.True(n == n.Normalize(), "Normal vectors on the sphere are normal.");
    }

}