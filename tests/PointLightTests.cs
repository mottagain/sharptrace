namespace tests;
using SharpTrace;

public class PointLightTests
{
    [Fact]
    public void PointLightConstruction()
    {
        var intensity = Color.White;
        var position = Tuple.NewPoint(0, 0, 0);

        var light = new PointLight(position, intensity);

        Assert.True(light.Position == position, "Point light is correctly constructed with a position.");
        Assert.True(light.Intensity == intensity, "Point light is correctly constructed with an intensity.");
    }
}
