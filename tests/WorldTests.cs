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
        var c = w.ShadeHit(comps, 5);

        Assert.True(c == new Color(0.38054f, 0.47568f, 0.28541f));
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
        var c = w.ShadeHit(comps, 5);

        Assert.True(c == new Color(0.90335f, 0.90335f, 0.90335f));
    }

    [Fact]
    public void ColorWhenRayHits()
    {
        var w = CreateDefaultTestWorld();
        var r = new Ray(Tuple.NewPoint(0, 0, -5), Tuple.NewVector(0, 0, 1));

        var c = w.ColorAt(r, 5);

        Assert.True(c == new Color(0.38054f, 0.47568f, 0.28541f));
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

        var c = w.ColorAt(r, 5);

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

    [Fact]
    public void RenderWorldWithCamera()
    {
        var w = CreateDefaultTestWorld();
        var c = new Camera(11, 11, MathExt.PiOver2);
        var from = Tuple.NewPoint(0, 0, -5);
        var to = Tuple.NewPoint(0, 0, 0);
        var up = Tuple.NewVector(0, 1, 0);
        c.Transform = Matrix.ViewTransform(from, to, up);

        var image = c.Render(w, 5);

        Assert.True(image[5, 5] == new Color(0.38054f, 0.47568f, 0.28541f));
    }

    [Fact]
    public void NoShadow() 
    {
        var w = CreateDefaultTestWorld();
        var p = Tuple.NewPoint(0, 10, 0);

        Assert.False(w.IsShadowed(p));
    }

    [Fact]
    public void ShadowWhenObjectBetweenPointAndLight() 
    {
        var w = CreateDefaultTestWorld();
        var p = Tuple.NewPoint(10, -10, 10);

        Assert.True(w.IsShadowed(p));
    }

    [Fact]
    public void NoShadowWhenObjectBehindLight() 
    {
        var w = CreateDefaultTestWorld();
        var p = Tuple.NewPoint(-20, 20, -20);

        Assert.False(w.IsShadowed(p));
    }

    [Fact]
    public void NoShadowWhenObjectBehindPoint() 
    {
        var w = CreateDefaultTestWorld();
        var p = Tuple.NewPoint(-2, 2, -2);

        Assert.False(w.IsShadowed(p));
    }

    [Fact]
    public void ShadeHitGivenAnIntersectionInShadow()
    {
        var w = new World();
        w.Light = new PointLight(Tuple.NewPoint(0, 0, -10), Color.White);

        var s1 = new Sphere();
        w.Objects.Add(s1);

        var s2 = new Sphere();
        s2.Transform = Matrix.Translation(0, 0, 10);
        w.Objects.Add(s2);

        var r = new Ray(Tuple.NewPoint(0, 0, 5), Tuple.NewVector(0, 0, 1));
        var i = new Intersection(4, s2);

        var comps = i.PrepareComputations(r);
        var color = w.ShadeHit(comps, 5);

        Assert.True(color == new Color(0.1f, 0.1f, 0.1f));
    }

    [Fact]
    public void ReflectedColorForNonreflectiveSurface()
    {
        var w = CreateDefaultTestWorld();
        var r = new Ray(Tuple.NewPoint(0, 0, 0), Tuple.NewVector(0, 0, 1));
        var shape = w.Objects.Last();
        shape.Material.Ambient = 1;
        var i = new Intersection(1, shape);

        var comps = i.PrepareComputations(r);
        var color = w.ReflectedColor(comps, 5);

        Assert.True(color == Color.Black);
    }

    [Fact]
    public void ReflectedColorForReflectiveMaterial()
    {
        var w = CreateDefaultTestWorld();
        var shape = new Plane();
        shape.Material.Reflectivity = 0.5f;
        shape.Transform = Matrix.Translation(0, -1, 0);
        w.Objects.Add(shape);
        var r = new Ray(Tuple.NewPoint(0, 0, -3), Tuple.NewVector(0, -MathExt.Sqrt2Over2, MathExt.Sqrt2Over2));
        var i = new Intersection((float)Math.Sqrt(2), shape);

        var comps = i.PrepareComputations(r);
        var color = w.ReflectedColor(comps, 5);

        Assert.True(color == new Color(0.19113f, 0.23892f, 0.14335f));
    }

    [Fact]
    public void ShadeHitWithReflectiveMaterial()
    {
        var w = CreateDefaultTestWorld();
        var shape = new Plane();
        shape.Material.Reflectivity = 0.5f;
        shape.Transform = Matrix.Translation(0, -1, 0);
        w.Objects.Add(shape);
        var r = new Ray(Tuple.NewPoint(0, 0, -3), Tuple.NewVector(0, -MathExt.Sqrt2Over2, MathExt.Sqrt2Over2));
        var i = new Intersection((float)Math.Sqrt(2), shape);

        var comps = i.PrepareComputations(r);
        var color = w.ShadeHit(comps, 5);

        Assert.True(color == new Color(0.87741f, 0.92519f, 0.82962f));
    }

    [Fact]
    public void ColorAtWithMutuallyReflectiveSurfaces()
    {
        var w = new World();
        w.Light = new PointLight(Tuple.NewPoint(0, 0, 0), Color.White);
        var lower = new Plane();
        lower.Material.Reflectivity = 1f;
        lower.Transform = Matrix.Translation(0, -1, 0);
        w.Objects.Add(lower);
        var upper = new Plane();
        upper.Material.Reflectivity = 1f;
        upper.Transform = Matrix.Translation(0, 1, 0);
        w.Objects.Add(upper);
        var r = new Ray(Tuple.NewPoint(0, 0, 0), Tuple.NewVector(0, 1, 0));

        var color = w.ColorAt(r, 5);

        Assert.True(true, "Recursive reflection terminates successfully.");
    }

    [Fact]
    public void ReflectedColorAtMaximumRecursiveDepth()
    {
        var w = CreateDefaultTestWorld();
        var shape = new Plane();
        shape.Material.Reflectivity = 0.5f;
        shape.Transform = Matrix.Translation(0, -1, 0);
        w.Objects.Add(shape);
        var r = new Ray(Tuple.NewPoint(0, 0, -3), Tuple.NewVector(0, -MathExt.Sqrt2Over2, MathExt.Sqrt2Over2));
        var i = new Intersection((float)Math.Sqrt(2), shape);

        var comps = i.PrepareComputations(r);
        var color = w.ReflectedColor(comps, 0);

        Assert.True(color == Color.Black);
    }


    [Fact]
    public void RefractedColorWithAnOpaqueSurface()
    {
        var w = CreateDefaultTestWorld();
        var shape = w.Objects.First();
        var r = new Ray(Tuple.NewPoint(0, 0, -5), Tuple.NewVector(0, 0, 1));
        var xs = new Intersections { new Intersection(4, shape), new Intersection(6, shape) };

        var comps = xs[0].PrepareComputations(r, xs);
        var c = w.RefractedColor(comps, 5);

        Assert.True(c == Color.Black);
    }

    [Fact]
    public void RefractedColorAtMaximumRecursiveDepth()
    {
        var w = CreateDefaultTestWorld();
        var shape = w.Objects.First();
        shape.Material.Transparency = 1f;
        shape.Material.RefractiveIndex = 1.5f;
        var r = new Ray(Tuple.NewPoint(0, 0, -5), Tuple.NewVector(0, 0, 1));
        var xs = new Intersections { new Intersection(4, shape), new Intersection(6, shape) };

        var comps = xs[0].PrepareComputations(r, xs);
        var c = w.RefractedColor(comps, 0);

        Assert.True(c == Color.Black);
    }

    [Fact]
    public void RefractedColorUnderTotalInternalReflection()
    {
        var w = CreateDefaultTestWorld();
        var shape = w.Objects.First();
        shape.Material.Transparency = 1f;
        shape.Material.RefractiveIndex = 1.5f;
        var r = new Ray(Tuple.NewPoint(0, 0, MathExt.Sqrt2Over2), Tuple.NewVector(0, 1, 0));
        var xs = new Intersections { new Intersection(-MathExt.Sqrt2Over2, shape), new Intersection(MathExt.Sqrt2Over2, shape) };

        var comps = xs[1].PrepareComputations(r, xs);
        var c = w.RefractedColor(comps, 5);

        Assert.True(c == Color.Black);
    }

    [Fact]
    public void RefractedColorWithRefractedRay()
    {
        var w = CreateDefaultTestWorld();
        var a = w.Objects.First();
        a.Material.Ambient = 1f;
        a.Material.Pattern = new TestPattern();
        var b = w.Objects.Last();
        b.Material.Transparency = 1f;
        b.Material.RefractiveIndex = 1.5f;

        var r = new Ray(Tuple.NewPoint(0, 0, 0.1f), Tuple.NewVector(0, 1, 0));
        var xs = new Intersections { new Intersection(-0.9899f, a), new Intersection(-0.4899f, b), new Intersection(0.4899f, b), new Intersection(0.9899f, a) };

        var comps = xs[2].PrepareComputations(r, xs);
        var c = w.RefractedColor(comps, 5);

        Assert.True(c == new Color(0, 0.99381f, 0.04849f));
    }

    [Fact]
    public void ShadeHitWithTransparentMaterial()
    {
        var w = CreateDefaultTestWorld();
        var floor = new Plane();
        floor.Transform = Matrix.Translation(0, -1, 0);
        floor.Material.Transparency = 0.5f;
        floor.Material.RefractiveIndex = 1.5f;
        w.Objects.Add(floor);
        var ball = new Sphere();
        ball.Material.Color = new Color(1, 0, 0);
        ball.Material.Ambient = 0.5f;
        ball.Transform = Matrix.Translation(0, -3.5f, -0.5f);
        w.Objects.Add(ball);
        var r = new Ray(Tuple.NewPoint(0, 0, -3), Tuple.NewVector(0, -MathExt.Sqrt2Over2, MathExt.Sqrt2Over2));
        var xs = new Intersections { new Intersection((float)Math.Sqrt(2), floor) };

        var comps = xs[0].PrepareComputations(r, xs);
        var color = w.ShadeHit(comps, 5);

        Assert.True(color == new Color(0.93627f, 0.68627f, 0.68627f));
    }

    [Fact]
    public void ShadeHitWithReflectiveTransparentMaterial()
    {
        var w = CreateDefaultTestWorld();
        var floor = new Plane();
        floor.Transform = Matrix.Translation(0, -1, 0);
        floor.Material.Reflectivity = 0.5f;
        floor.Material.Transparency = 0.5f;
        floor.Material.RefractiveIndex = 1.5f;
        w.Objects.Add(floor);
        var ball = new Sphere();
        ball.Material.Color = new Color(1, 0, 0);
        ball.Material.Ambient = 0.5f;
        ball.Transform = Matrix.Translation(0, -3.5f, -0.5f);
        w.Objects.Add(ball);
        var r = new Ray(Tuple.NewPoint(0, 0, -3), Tuple.NewVector(0, -MathExt.Sqrt2Over2, MathExt.Sqrt2Over2));
        var xs = new Intersections { new Intersection((float)Math.Sqrt(2), floor) };

        var comps = xs[0].PrepareComputations(r, xs);
        var color = w.ShadeHit(comps, 5);

        Assert.True(color == new Color(0.93380f, 0.69632f, 0.69230f));
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