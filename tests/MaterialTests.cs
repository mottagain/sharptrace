namespace tests;
using SharpTrace;

public class MaterialTests
{
    [Fact]
    public void DefaultMaterial()
    {
        var m = new Material();
        
        Assert.True(m.Color == Color.White, "Default color.");
        Assert.True(m.Ambient == 0.1f, "Default color.");
        Assert.True(m.Diffuse == 0.9f, "Default diffuse.");
        Assert.True(m.Specular == 0.9f, "Default specular.");
        Assert.True(m.Shininess == 200f, "Default shininess.");
    }

    [Fact]
    public void LightingWithEyeBetweenLightAndSurface()
    {
        var obj = new Sphere();
        var m = new Material();
        var position = Tuple.NewPoint(0, 0, 0);
        var eyev = Tuple.NewVector(0, 0, -1);
        var normalv = Tuple.NewVector(0, 0, -1);
        var light = new PointLight(Tuple.NewPoint(0, 0, -10), Color.White);

        var result = m.Lighting(obj, light, position, eyev, normalv, false);

        Assert.True(result == new Color(1.9f, 1.9f, 1.9f), "Lighting is at full strength.");
    }

    [Fact]
    public void LightingWithEyeBetweenLightAndSurfaceOffset45()
    {
        var obj = new Sphere();
        var m = new Material();
        var position = Tuple.NewPoint(0, 0, 0);
        var eyev = Tuple.NewVector(0, MathExt.Sqrt2Over2, -MathExt.Sqrt2Over2);
        var normalv = Tuple.NewVector(0, 0, -1);
        var light = new PointLight(Tuple.NewPoint(0, 0, -10), Color.White);

        var result = m.Lighting(obj, light, position, eyev, normalv, false);

        Assert.True(result == Color.White, "Lighting is at full strength minus specular.");
    }

    [Fact]
    public void LightingWithEyeOppositeSurfaceOffset45()
    {
        var obj = new Sphere();
        var m = new Material();
        var position = Tuple.NewPoint(0, 0, 0);
        var eyev = Tuple.NewVector(0, 0, -1);
        var normalv = Tuple.NewVector(0, 0, -1);
        var light = new PointLight(Tuple.NewPoint(0, 10, -10), Color.White);

        var result = m.Lighting(obj, light, position, eyev, normalv, false);

        Assert.True(result == new Color(0.7364f, 0.7364f, 0.7364f), "Lighting is at full strength minus specular.");
    }

    [Fact]
    public void LightingWithEyeInPathOfReflectionVector()
    {
        var obj = new Sphere();
        var m = new Material();
        var position = Tuple.NewPoint(0, 0, 0);
        var eyev = Tuple.NewVector(0, -MathExt.Sqrt2Over2, -MathExt.Sqrt2Over2);
        var normalv = Tuple.NewVector(0, 0, -1);
        var light = new PointLight(Tuple.NewPoint(0, 10, -10), Color.White);

        var result = m.Lighting(obj, light, position, eyev, normalv, false);

        Assert.True(result == new Color(1.6364f, 1.6364f, 1.6364f), "Lighting where eye is in the path of the relfection vector.");
    }

    [Fact]
    public void LightingWithLightBehindSurface()
    {
        var obj = new Sphere();
        var m = new Material();
        var position = Tuple.NewPoint(0, 0, 0);
        var eyev = Tuple.NewVector(0, 0, -1);
        var normalv = Tuple.NewVector(0, 0, -1);
        var light = new PointLight(Tuple.NewPoint(0, 0, 10), Color.White);

        var result = m.Lighting(obj, light, position, eyev, normalv, false);

        Assert.True(result == new Color(0.1f, 0.1f, 0.1f), "Lighting with light behind surface.");
    }

    [Fact]
    public void LightingWithTheSurfaceInShadow()
    {
        var obj = new Sphere();
        var m = new Material();
        var position = Tuple.NewPoint(0, 0, 0);
        var eyev = Tuple.NewVector(0, 0, -1);
        var normalv = Tuple.NewVector(0, 0, -1);
        var light = new PointLight(Tuple.NewPoint(0, 0, -10), Color.White);

        var result = m.Lighting(obj, light, position, eyev, normalv, true);

        Assert.True(result == new Color(0.1f, 0.1f, 0.1f), "Lighting is only ambient in shadow.");
    }

    [Fact]
    public void LightingWithAPatternApplied()
    {
        var obj = new Sphere();
        var m = new Material();
        m.Pattern = new StripePattern(Color.White, Color.Black);
        m.Ambient = 1;
        m.Diffuse = 0;
        m.Specular = 0;
        var eyev = Tuple.NewVector(0, 0, -1);
        var normalv = Tuple.NewVector(0, 0, -1);
        var light = new PointLight(Tuple.NewPoint(0, 0, -10), Color.White);

        var c1 = m.Lighting(obj, light, Tuple.NewPoint(0.9f, 0, 0), eyev, normalv, false);
        var c2 = m.Lighting(obj, light, Tuple.NewPoint(1.1f, 0, 0), eyev, normalv, false);

        Assert.True(c1 == Color.White);
        Assert.True(c2 == Color.Black);
    }

}
