

namespace tests;
using SharpTrace;


public class CanvasTests
{
    [Fact]
    public void CanvasConstructor() 
    {
        var canvas = new Canvas(10, 20);

        Assert.True(canvas.Width == 10 && canvas.Height == 20, "Canvas constructor properly sets width and height.");
    }

    [Fact]
    public void WritePixelToCanvas() 
    {
        var canvas = new Canvas(10, 20);
        var red = new Color(1, 0, 0);

        canvas[2, 3] = red;

        Assert.True(canvas[2, 3] == red, "Able to set a pixel and retrieve it.");
    }
}
