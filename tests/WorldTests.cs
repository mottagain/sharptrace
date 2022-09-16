namespace tests;
using SharpTrace;

public class WorldTests
{
    [Fact]
    public void WorldIsEmpty()
    {
        var w = new World();

        Assert.True(w.Objects.Count == 0, "Default world has no objects.");
        Assert.True(w.Light == null, "Default world has no light.");
    }

    [Fact]
    public void DefaultTestWorld() 
    {
        var w = CreateDefaultTestWorld();

        Assert.True(w.Light != null, "Default test world has a light.");
        Assert.True(w.Objects.Count == 2, "Default test world has two objects.");
    }

    [Fact]
    public void IntersectAWorldWithARay() 
    {
        var w = CreateDefaultTestWorld();
        var r = new Ray(Tuple.NewPoint(0, 0, -5), Tuple.NewVector(0, 0, 1));
        
        var xs = w.Intersects(r);

        Assert.True(xs.Count == 4, "Expected 4 intersections along target ray.");
        Assert.True(MathExt.Near(xs[0].Time, 4f));
        Assert.True(MathExt.Near(xs[1].Time, 4.5f));
        Assert.True(MathExt.Near(xs[2].Time, 5.5f));
        Assert.True(MathExt.Near(xs[3].Time, 6f));
    }

    [Fact]
    public void ShadingAnIntersection()
    {
        var w = CreateDefaultTestWorld();
        var r = new Ray(Tuple.NewPoint(0, 0, -5), Tuple.NewVector(0, 0, 1));
        var shape = w.Objects.First();
        var i = new Intersection(4, shape);

        var comps = i.PrepareComputations(r);
        var c = w.ShadeHit(comps);

        Assert.True(c == new Color(0.38066f, 0.47583f, 0.2855f));
    }

    [Fact]
    public void ShadingAnIntersectionFromInside()
    {
        var w = CreateDefaultTestWorld();
        w.Light = new PointLight(Tuple.NewPoint(0, 0.25f, 0), Color.White);
        var r = new Ray(Tuple.NewPoint(0, 0, 0), Tuple.NewVector(0, 0, 1));
        var shape = w.Objects.Last();
        var i = new Intersection(0.5f, shape);

        var comps = i.PrepareComputations(r);
        var c = w.ShadeHit(comps);

        Assert.True(c == new Color(0.90498f, 0.90498f, 0.90498f));
    }

    [Fact]
    public void ColorWhenRayHits()
    {
        var w = CreateDefaultTestWorld();
        var r = new Ray(Tuple.NewPoint(0, 0, -5), Tuple.NewVector(0, 0, 1));

        var c = w.ColorAt(r);

        Assert.True(c == new Color(0.38066f, 0.47583f, 0.2855f));
    }

    [Fact]
    public void ColorWhenIntersectionBehindRay() 
    {
        var w = CreateDefaultTestWorld();
        var outer = w.Objects.First();
        outer.Material.Ambient = 1f;
        var inner = w.Objects.Last();
        inner.Material.Ambient = 1f;
        var r = new Ray(Tuple.NewPoint(0, 0, 0.75f), Tuple.NewVector(0, 0, -1));

        var c = w.ColorAt(r);

        Assert.True(c == inner.Material.Color);
    }

    [Fact]
    public void TransformationMatrixForDefaultOrientation()
    {
        var from = Tuple.NewPoint(0, 0, 0);
        var to = Tuple.NewPoint(0, 0, -1);
        var up = Tuple.NewVector(0, 1, 0);

        var t = Matrix.ViewTransform(from, to, up);

        Assert.True(t == Matrix.Identity(4));
    }

    [Fact]
    public void ViewTransformationMatrixLookingInPositiveZ()
    {
        var from = Tuple.NewPoint(0, 0, 0);
        var to = Tuple.NewPoint(0, 0, 1);
        var up = Tuple.NewVector(0, 1, 0);

        var t = Matrix.ViewTransform(from, to, up);

        Assert.True(t == Matrix.Scaling(-1, 1, -1));
    }

    [Fact]
    public void ViewTransformationMovesTheWorld()
    {
        var from = Tuple.NewPoint(0, 0, 8);
        var to = Tuple.NewPoint(0, 0, 0);
        var up = Tuple.NewVector(0, 1, 0);

        var t = Matrix.ViewTransform(from, to, up);

        Assert.True(t == Matrix.Translation(0, 0, -8));
    }

    [Fact]
    public void ViewTransformationArbitraryView()
    {
        var from = Tuple.NewPoint(1, 3, 2);
        var to = Tuple.NewPoint(4, -2, 8);
        var up = Tuple.NewVector(1, 1, 0);

        var t = Matrix.ViewTransform(from, to, up);

        Assert.True(t == new Matrix(new float[,] { 
            { -0.50709f, 0.50709f,  0.67612f, -2.36643f }, 
            {  0.76772f, 0.60609f,  0.12122f, -2.82843f }, 
            { -0.35857f, 0.59761f, -0.71714f,  0.00000f }, 
            {  0.00000f, 0.00000f,  0.00000f,  1.00000f }, 
        }));
    }

    private static World CreateDefaultTestWorld()
    {
        var result = new World();
        
        var s1 = new Sphere();
        s1.Material.Color = new Color(0.8f, 1.0f, 0.6f);
        s1.Material.Diffuse = 0.7f;
        s1.Material.Specular = 0.2f;
        result.Objects.Add(s1);
        
        var s2 = new Sphere();
        s2.Transform = Matrix.Scaling(0.5f, 0.5f, 0.5f);
        result.Objects.Add(s2);

        result.Light = new PointLight(Tuple.NewPoint(-10, 10, -10), Color.White);

        return result;
    }
}