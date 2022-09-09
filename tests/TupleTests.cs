namespace tests;
using SharpTrace;

public class TupleTests
{
    [Fact]
    public void TupleIsPoint()
    {
        Tuple tuple = new Tuple { x = 0.0f, y = 0.0f, z = 0.0f, w = 1.0f };

        Assert.True(tuple.IsPoint(), "A tuple with w=1.0 is a point.");
        Assert.False(tuple.IsVector(), "A tuple with w=1.0 is not a vector.");
    }

    [Fact]
    public void TupleIsVector()
    {
        Tuple tuple = new Tuple { x = 0.0f, y = 0.0f, z = 0.0f, w = 0.0f };

        Assert.True(tuple.IsVector(), "A tuple with w=0.0 is a vector.");
        Assert.False(tuple.IsPoint(), "A tuple with w=1.0 is not a point.");
    }

    [Fact]
    public void FactoryMethodCreatesPoint()
    {
        Tuple tuple = Tuple.NewPoint(1.0f, 2.0f, 3.0f);

        Assert.True(tuple == new Tuple { x = 1, y = 2, z = 3, w = 1 }, "Tuple.NewPoint matches raw construction.");
        Assert.True(tuple.IsPoint(), "Tuple.NewPoint creates a point.");
    }

    [Fact]
    public void FactoryMethodCreatesVector()
    {
        Tuple tuple = Tuple.NewVector(3.0f, 2.0f, 1.0f);

        Assert.True(tuple == new Tuple { x = 3, y = 2, z = 1, w = 0 }, "Tuple.NewVector matches raw construction.");
        Assert.True(tuple.IsVector(), "Tuple.NewVector creates a vector.");
    }

    [Fact]
    public void AdditionOfVectorAndPoint()
    {
        Tuple a1 = new Tuple { x = 3, y = -2, z = 5, w = 1 };
        Tuple a2 = new Tuple { x = -2, y = 3, z = 1, w = 0 };

        Tuple result = a1 + a2;

        Assert.True(result == new Tuple { x = 1, y = 1, z = 6, w = 1 }, "Addition of a vector and a point yields correct point.");
        Assert.True(result.IsPoint(), "Addition of a vector and a point is a point.");
    }

    [Fact]
    public void AdditionOfVectorAndVector()
    {
        Tuple a1 = new Tuple { x = 3, y = -2, z = 5, w = 0 };
        Tuple a2 = new Tuple { x = -2, y = 3, z = 1, w = 0 };

        Tuple result = a1 + a2;

        Assert.True(result == new Tuple { x = 1, y = 1, z = 6, w = 0 }, "Addition of a vector and a vector yields correct vector.");
        Assert.True(result.IsVector(), "Addition of a vector and a vector is a vector.");
    }

    [Fact]
    public void AdditionOfPointAndPoint()
    {
        Tuple a1 = new Tuple { x = 0, y = 0, z = 0, w = 1 };
        Tuple a2 = new Tuple { x = 0, y = 0, z = 0, w = 1 };

        Tuple result = a1 + a2;

        Assert.False(result.IsPoint(), "Addition of a point and point is not a point.");
        Assert.False(result.IsVector(), "Addition of a point and point is not a vector.");
    }

    [Fact]
    public void SubtractionOfPointAndPoint()
    {
        Tuple a1 = new Tuple { x = 3, y = 2, z = 1, w = 1 };
        Tuple a2 = new Tuple { x = 5, y = 6, z = 7, w = 1 };

        Tuple result = a1 - a2;

        Assert.True(result == new Tuple { x = -2, y = -4, z = -6, w = 0 }, "Subtraction of a point and a point yields correct point.");
        Assert.True(result.IsVector(), "Subtraction of a point and point is a vector.");
    }

    [Fact]
    public void SubtractionOfPointAndVector() 
    {
        Tuple p = Tuple.NewPoint(3, 2, 1);
        Tuple v = Tuple.NewVector(5, 6, 7);
        
        Assert.True(p - v == Tuple.NewPoint(-2, -4, -6), "Subtraction of a vector from a point yields correct point.");
    }

    [Fact]
    public void SubtractionOfVectorAndPoint() 
    {
        Tuple v = Tuple.NewVector(5, 6, 7);
        Tuple p = Tuple.NewPoint(3, 2, 1);

        Tuple result = v - p;
        
        Assert.False(result.IsPoint(), "Subtraction of a point from a vector isn't a point.");
        Assert.False(result.IsVector(), "Subtraction of a point from a vector isn't a vector.");
    }

    [Fact]
    public void SubtractionOfVectorAndVector()
    {
        Tuple v1 = Tuple.NewVector(3, 2, 1);
        Tuple v2 = Tuple.NewVector(5, 6, 7);

        Tuple result = v1 - v2;

        Assert.True(result == Tuple.NewVector(-2, -4, -6), "Subtraction of a vector and a vector yields correct point.");
        Assert.True(result.IsVector(), "Subtraction of a vector and vector is a vector.");
    }

    [Fact]
    public void NegatingATuple() 
    {
        Tuple a = new Tuple { x = 1, y = -2, z = 3, w = -4 };
        
        Assert.True(-a == new Tuple { x = -1, y = 2, z = -3, w = 4 }, "Negation of a tuple yields the correct tuple.");
    }

}