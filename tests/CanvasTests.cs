

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
}
