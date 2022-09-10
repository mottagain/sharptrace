namespace tests;
using SharpTrace;

public class TupleTests
{
    [Fact]
    public void TupleIsPoint()
    {
        var tuple = new Tuple { x = 0.0f, y = 0.0f, z = 0.0f, w = 1.0f };

        Assert.True(tuple.IsPoint(), "A tuple with w=1.0 is a point.");
        Assert.False(tuple.IsVector(), "A tuple with w=1.0 is not a vector.");
    }

    [Fact]
    public void TupleIsVector()
    {
        var tuple = new Tuple { x = 0.0f, y = 0.0f, z = 0.0f, w = 0.0f };

        Assert.True(tuple.IsVector(), "A tuple with w=0.0 is a vector.");
        Assert.False(tuple.IsPoint(), "A tuple with w=1.0 is not a point.");
    }

    [Fact]
    public void FactoryMethodCreatesPoint()
    {
        var tuple = Tuple.NewPoint(1.0f, 2.0f, 3.0f);

        Assert.True(tuple == new Tuple { x = 1, y = 2, z = 3, w = 1 }, "Tuple.NewPoint matches raw construction.");
        Assert.True(tuple.IsPoint(), "Tuple.NewPoint creates a point.");
    }

    [Fact]
    public void FactoryMethodCreatesVector()
    {
        var tuple = Tuple.NewVector(3.0f, 2.0f, 1.0f);

        Assert.True(tuple == new Tuple { x = 3, y = 2, z = 1, w = 0 }, "Tuple.NewVector matches raw construction.");
        Assert.True(tuple.IsVector(), "Tuple.NewVector creates a vector.");
    }

    [Fact]
    public void AdditionOfVectorAndPoint()
    {
        var a1 = new Tuple { x = 3, y = -2, z = 5, w = 1 };
        var a2 = new Tuple { x = -2, y = 3, z = 1, w = 0 };

        var result = a1 + a2;

        Assert.True(result == new Tuple { x = 1, y = 1, z = 6, w = 1 }, "Addition of a vector and a point yields correct point.");
        Assert.True(result.IsPoint(), "Addition of a vector and a point is a point.");
    }

    [Fact]
    public void AdditionOfVectorAndVector()
    {
        var a1 = new Tuple { x = 3, y = -2, z = 5, w = 0 };
        var a2 = new Tuple { x = -2, y = 3, z = 1, w = 0 };

        var result = a1 + a2;

        Assert.True(result == new Tuple { x = 1, y = 1, z = 6, w = 0 }, "Addition of a vector and a vector yields correct vector.");
        Assert.True(result.IsVector(), "Addition of a vector and a vector is a vector.");
    }

    [Fact]
    public void AdditionOfPointAndPoint()
    {
        var a1 = new Tuple { x = 0, y = 0, z = 0, w = 1 };
        var a2 = new Tuple { x = 0, y = 0, z = 0, w = 1 };

        var result = a1 + a2;

        Assert.False(result.IsPoint(), "Addition of a point and point is not a point.");
        Assert.False(result.IsVector(), "Addition of a point and point is not a vector.");
    }

    [Fact]
    public void SubtractionOfPointAndPoint()
    {
        var a1 = new Tuple { x = 3, y = 2, z = 1, w = 1 };
        var a2 = new Tuple { x = 5, y = 6, z = 7, w = 1 };

        var result = a1 - a2;

        Assert.True(result == new Tuple { x = -2, y = -4, z = -6, w = 0 }, "Subtraction of a point and a point yields correct point.");
        Assert.True(result.IsVector(), "Subtraction of a point and point is a vector.");
    }

    [Fact]
    public void SubtractionOfPointAndVector() 
    {
        var p = Tuple.NewPoint(3, 2, 1);
        var v = Tuple.NewVector(5, 6, 7);
        
        Assert.True(p - v == Tuple.NewPoint(-2, -4, -6), "Subtraction of a vector from a point yields correct point.");
    }

    [Fact]
    public void SubtractionOfVectorAndPoint() 
    {
        var v = Tuple.NewVector(5, 6, 7);
        var p = Tuple.NewPoint(3, 2, 1);

        var result = v - p;
        
        Assert.False(result.IsPoint(), "Subtraction of a point from a vector isn't a point.");
        Assert.False(result.IsVector(), "Subtraction of a point from a vector isn't a vector.");
    }

    [Fact]
    public void SubtractionOfVectorAndVector()
    {
        var v1 = Tuple.NewVector(3, 2, 1);
        var v2 = Tuple.NewVector(5, 6, 7);

        var result = v1 - v2;

        Assert.True(result == Tuple.NewVector(-2, -4, -6), "Subtraction of a vector and a vector yields correct point.");
        Assert.True(result.IsVector(), "Subtraction of a vector and vector is a vector.");
    }

    [Fact]
    public void NegatingATuple() 
    {
        var a = new Tuple { x = 1, y = -2, z = 3, w = -4 };
        
        Assert.True(-a == new Tuple { x = -1, y = 2, z = -3, w = 4 }, "Negation of a tuple yields the correct tuple.");
    }

    [Fact]
    public void MultiplicationOfATupleAndScalar()
    {
        var a = new Tuple { x = 1, y = -2, z = 3, w = -4 };

        var result = a * 3.5f;

        Assert.True(result == new Tuple { x = 3.5f, y = -7f, z = 10.5f, w = -14f}, "Multiplication of a tuple and a scalar yields correct tuple.");
    }

    [Fact]
    public void DivisionOfATupleAndScalar()
    {
        var a = new Tuple { x = 1, y = -2, z = 3, w = -4 };

        var result = a / 2f;

        Assert.True(result == new Tuple { x = 0.5f, y = -1f, z = 1.5f, w = -2f}, "Multiplication of a tuple and a scalar yields correct tuple.");
    }

    [Fact]
    public void MagnitudeOfSimpleUnitVector()
    {
        var v = Tuple.NewVector(0, 0, 1);

        Assert.True(Tuple.ApproximatelyEqual(v.Magnitude(), 1), "Manitude of a simple unit vector in the z direction is 1.");
    }

    [Fact]
    public void MagnitudeOfVector()
    {
        var v = Tuple.NewVector(1, 2, 3);

        Assert.True(Tuple.ApproximatelyEqual(v.Magnitude(), (float)Math.Sqrt(14)), "Manitude of a 1, 2, 3 vector yeilds the correct value.");
    }

    [Fact]
    public void MagnitudeOfANegativeVector()
    {
        var v = Tuple.NewVector(-1, -2, -3);

        Assert.True(Tuple.ApproximatelyEqual(v.Magnitude(), (float)Math.Sqrt(14)), "Manitude of a -1, -2, -3 vector yeilds the correct value.");
    }

    [Fact]
    public void NomalizeOfSimpleUnitVector()
    {
        var v = Tuple.NewVector(0, 0, 4);

        var result = v.Normalize();

        Assert.True(result == Tuple.NewVector(0, 0, 1), "Normalization of a vector yields the correct vector.");
    }

    [Fact]
    public void NomalizeOfVector()
    {
        var v = Tuple.NewVector(1, 2, 3);

        var result = v.Normalize();

        Assert.True(Tuple.ApproximatelyEqual(result.Magnitude(), 1.0f), "Magnitude of the result of normalization is 1.");
        Assert.True(result == Tuple.NewVector(0.26726f, 0.53452f, 0.80178f), "Normalization of a vector yields the correct vector.");
    }


    [Fact]
    public void MagnitudeOfNormalVector()
    {
        var v = Tuple.NewVector(1, 2, 3);

        var result = v.Normalize();

        Assert.True(Tuple.ApproximatelyEqual(result.Magnitude(), 1.0f), "Magnitude of the result of normalization is 1.");
    }

    [Fact]
    public void DotProductOfTwoVectors()
    {
        var a = Tuple.NewVector(1, 2, 3);
        var b = Tuple.NewVector(2, 3, 4);

        Assert.True(Tuple.ApproximatelyEqual(Tuple.Dot(a, b), 20), "Dot product of two vectors yields the correct scalar.");
    }

    [Fact]
    public void CrossProductOfTwoVectors()
    {
        var a = Tuple.NewVector(1, 2, 3);
        var b = Tuple.NewVector(2, 3, 4);

        Assert.True(Tuple.Cross(a, b) == Tuple.NewVector(-1, 2, -1), "Cross product of two vectors yields the correct vector.");
        Assert.True(Tuple.Cross(b, a) == Tuple.NewVector(1, -2, 1), "Cross product of two vectors yields the correct vector.");
    }

}