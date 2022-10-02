
namespace tests;
using SharpTrace;


public class CameraTests
{
    [Fact]
    public void CameraConstructor()
    {
        var width = 160;
        var height = 120;
        var fov = (float)(Math.PI / 2.0);

        var camera = new Camera(width, height, fov);

        Assert.True(camera.Width == width);
        Assert.True(camera.Height == height);
        Assert.True(camera.FOV == fov);
        Assert.True(camera.Transform == Matrix.Identity(4));
    }

    [Fact]
    public void PixelSizeForHorizontalCanvas()
    {
        var c = new Camera(200, 125, MathExt.PiOver2);

        Assert.True(c.PixelSize == 0.01f);
    }

    [Fact]
    public void PixelSizeForVerticalCanvas()
    {
        var c = new Camera(125, 200, MathExt.PiOver2);

        Assert.True(c.PixelSize == 0.01f);
    }

    [Fact]
    public void RayThroughCenterOfCanvas()
    {
        var c = new Camera(201, 101, MathExt.PiOver2);

        var r = c.RayForPixel(100, 50);

        Assert.True(r.Origin == Tuple.NewPoint(0, 0, 0));
        Assert.True(r.Direction == Tuple.NewVector(0, 0, -1));
    }

    [Fact]
    public void RayThroughCornerOfCanvas()
    {
        var c = new Camera(201, 101, MathExt.PiOver2);

        var r = c.RayForPixel(0, 0);

        Assert.True(r.Origin == Tuple.NewPoint(0, 0, 0));
        Assert.True(r.Direction == Tuple.NewVector(0.66519f, 0.33259f, -0.66851f));
    }

    [Fact]
    public void RayWhenCameraIsTransformed()
    {
        var c = new Camera(201, 101, MathExt.PiOver2);
        c.Transform = Matrix.RotationY(MathExt.PiOver4) * Matrix.Translation(0, -2, 5);

        var r = c.RayForPixel(100, 50);

        Assert.True(r.Origin == Tuple.NewPoint(0, 2, -5));
        Assert.True(r.Direction == Tuple.NewVector(MathExt.Sqrt2Over2, 0, -MathExt.Sqrt2Over2));
    }

}
