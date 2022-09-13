
namespace tests;
using SharpTrace;

public class RayTests
{
    [Fact]
    public void CreatingAndQueryingARay()
    {
        var origin = Tuple.NewPoint(1, 2, 3);
        var direction = Tuple.NewVector(4, 5, 6);

        var r = new Ray(origin, direction);
        
        Assert.True(r.Origin == origin, "Ray returns the origin it was constructed with.");
        Assert.True(r.Direction == direction, "Ray returns the direction it was constructed with.");
    }

    [Fact]
    public void CreatePointFromDistance()
    {
        var r = new Ray(Tuple.NewPoint(2, 3, 4), Tuple.NewVector(1, 0, 0));

        Assert.True(r.Position(0) == Tuple.NewPoint(2, 3, 4));
        Assert.True(r.Position(1) == Tuple.NewPoint(3, 3, 4));
        Assert.True(r.Position(-1) == Tuple.NewPoint(1, 3, 4));
        Assert.True(r.Position(2.5f) == Tuple.NewPoint(4.5f, 3, 4));
    }
}