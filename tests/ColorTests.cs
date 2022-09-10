namespace tests;
using SharpTrace;

public class ColorTests
{
    [Fact]
    public void ColorConstructor()
    {
        var color = new Color(-0.5f, 0.4f, 1.7f);

        Assert.True(
            Color.ApproximatelyEqual(color.r, -0.5f) && 
            Color.ApproximatelyEqual(color.g, 0.4f) && 
            Color.ApproximatelyEqual(color.b, 1.7f), 
            "Color constructor forwards correct values.");
    }

    [Fact]
    public void AdditionOfColors()
    {
        var c1 = new Color(0.9f, 0.6f, 0.75f);
        var c2 = new Color(0.7f, 0.1f, 0.25f);

        var result = c1 + c2;

        Assert.True(result == new Color(1.6f, 0.7f, 1.0f), "Addition of a colors yields correct color.");
    }

    [Fact]
    public void SubtractionOfColors()
    {
        var c1 = new Color(0.9f, 0.6f, 0.75f);
        var c2 = new Color(0.7f, 0.1f, 0.25f);

        var result = c1 - c2;

        Assert.True(result == new Color(0.2f, 0.5f, 0.5f), "Subtraction of a colors yields correct color.");
    }

    [Fact]
    public void MultiplicationOfAColorAndScalar()
    {
        var c = new Color(0.2f, 0.3f, 0.4f);

        var result = c * 2f;

        Assert.True(result == new Color(0.4f, 0.6f, 0.8f), "Multiplication of a Color and a scalar yields correct Color.");
    }

    [Fact]
    public void MultiplicationOfColors()
    {
        var c1 = new Color(1.0f, 0.2f, 0.4f);
        var c2 = new Color(0.9f, 1.0f, 0.1f);

        var result = Color.HardamardProduct(c1, c2);

        Assert.True(result == new Color(0.9f, 0.2f, 0.04f), "Addition of a colors yields correct color.");
    }


}