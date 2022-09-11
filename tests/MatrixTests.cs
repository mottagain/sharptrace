namespace tests;
using SharpTrace;

public class MatrixTests
{
    [Fact]
    public void EmptryMatrixConstruction()
    {
        var matrix = new Matrix(2, 2);

        Assert.True(matrix.Rows == 2 && matrix.Columns == 2, "Construct a 2x2 emptry matrix.");
    }

    [Fact]
    public void Sample4x4MatrixConstruction()
    {
        var testData = new float[,] { { 1f , 2f, 3f, 4f }, { 5.5f, 6.5f, 7.5f, 8.5f }, { 9f, 10f, 11f, 12f }, { 13.5f, 14.5f, 15.5f, 16.5f } };

        var matrix = new Matrix(testData);

        Assert.True(matrix.Rows == 4 && matrix.Columns == 4, "Construct a 4x4 matrix from test data.");
        Assert.True(MathExt.Near(matrix[0, 0], 1f), "[0,0] has the value it was constructed with.");
        Assert.True(MathExt.Near(matrix[0, 3], 4f), "[0,3] has the value it was constructed with.");
        Assert.True(MathExt.Near(matrix[1, 0], 5.5f), "[1,0] has the value it was constructed with.");
        Assert.True(MathExt.Near(matrix[1, 2], 7.5f), "[1,2] has the value it was constructed with.");
        Assert.True(MathExt.Near(matrix[2, 2], 11f), "[2,2] has the value it was constructed with.");
        Assert.True(MathExt.Near(matrix[3, 0], 13.5f), "[3,0] has the value it was constructed with.");
        Assert.True(MathExt.Near(matrix[3, 2], 15.5f), "[3,2] has the value it was constructed with.");
    }

    [Fact]
    public void Sample3x3MatrixConstruction()
    {
        var testData = new float[,] { { -3f , 5f, 0f }, { 1f, -2f, -7f }, { 0f, 1f, 1f } };

        var matrix = new Matrix(testData);

        Assert.True(matrix.Rows == 3 && matrix.Columns == 3, "Construct a 3x3 matrix from test data.");
        Assert.True(MathExt.Near(matrix[0, 0], -3f), "[0,0] has the value it was constructed with.");
        Assert.True(MathExt.Near(matrix[1, 1], -2f), "[1,1] has the value it was constructed with.");
        Assert.True(MathExt.Near(matrix[2, 2], 1f), "[2,2] has the value it was constructed with.");
    }

    [Fact]
    public void Sample2x2MatrixConstruction()
    {
        var testData = new float[,] { { -3f , 5f }, { 1f, -2f } };

        var matrix = new Matrix(testData);

        Assert.True(matrix.Rows == 2 && matrix.Columns == 2, "Construct a 2x2 matrix from test data.");
        Assert.True(MathExt.Near(matrix[0, 0], -3f), "[0,0] has the value it was constructed with.");
        Assert.True(MathExt.Near(matrix[0, 1], 5f), "[0,1] has the value it was constructed with.");
        Assert.True(MathExt.Near(matrix[1, 0], 1f), "[1,0] has the value it was constructed with.");
        Assert.True(MathExt.Near(matrix[1, 1], -2f), "[1,1] has the value it was constructed with.");
    }

    [Fact]
    public void MatrixEquality() 
    {
         var testData = new float[,] { { 1f , 2f, 3f, 4f }, { 5.5f, 6.5f, 7.5f, 8.5f }, { 9f, 10f, 11f, 12f }, { 13.5f, 14.5f, 15.5f, 16.5f } };

         var m1 = new Matrix(testData);
         var m2 = new Matrix(testData);

         Assert.True(m1 == m2, "Two matricies withe same data are equal.");              
    }

    [Fact]
    public void MatrixEqualityDifferentDimensions() 
    {
         var testData3x3 = new float[,] { { 1f , 2f, 3f }, { 5.5f, 6.5f, 7.5f }, { 9f, 10f, 11f } };
         var testData4x4 = new float[,] { { 1f , 2f, 3f, 4f }, { 5.5f, 6.5f, 7.5f, 8.5f }, { 9f, 10f, 11f, 12f }, { 13.5f, 14.5f, 15.5f, 16.5f } };

         var m1 = new Matrix(testData3x3);
         var m2 = new Matrix(testData4x4);

         Assert.True(m1 != m2, "A 4x4 matrix that matches that data of a 3x3 matrix are nonetheless not equal.");
    }

    [Fact]
    public void MatrixEqualityDifferentData() 
    {
         var testData1 = new float[,] { { 1f , 2f, 3f }, { 5.5f, 6.5f, 7.5f }, { 9f, 10f, 11f } };
         var testData2 = new float[,] { { 1f , 2f, 3f }, { 5.5f, 6.5f, 7.5f }, { 9f, 10f, 12f } };

         var m1 = new Matrix(testData1);
         var m2 = new Matrix(testData2);

         Assert.True(m1 != m2, "Matricies with different data are not equal.");
    }

    [Fact]
    public void MultiplyMatricies()
    {
        var testData1 = new float[,] { { 1f , 2f, 3f, 4f }, { 5f, 6f, 7f, 8f }, { 9f, 8f, 7f, 6f }, { 5f, 4f, 3f, 2f } };
        var testData2 = new float[,] { { -2f , 1f, 2f, 3f }, { 3f, 2f, 1f, -1f }, { 4f, 3f, 6f, 5f }, { 1f, 2f, 7f, 8f } };
        var resultData = new float[,] { { 20f , 22f, 50f, 48f }, { 44f, 54f, 114f, 108f }, { 40f, 58f, 110f, 102f }, { 16f, 26f, 46f, 42f } };

        var m1 = new Matrix(testData1);
        var m2 = new Matrix(testData2);
        var result = new Matrix(resultData);

        Assert.True(m1 * m2 == result, "Multiplying 2 matricies results in the expected value.");
    }

    [Fact]
    public void MultiplyMatrixAndTuple()
    {
        var testData = new float[,] { { 1f , 2f, 3f, 4f }, { 2f, 4f, 4f, 2f }, { 8f, 6f, 4f, 1f }, { 0f, 0f, 0f, 1f } };

        var m = new Matrix(testData);
        var t = new Tuple { x = 1f, y = 2f, z = 3f, w = 1f };

        Assert.True(m * t == new Tuple { x = 18f, y = 24f, z = 33f, w = 1f }, "Multiplying a matrix by a Tuple results in the expected value.");
    }

    [Fact]
    public void MultiplyMatrixByIdentity()
    {
        var testData = new float[,] { { 1f , 2f, 3f, 4f }, { 2f, 4f, 4f, 2f }, { 8f, 6f, 4f, 1f }, { 0f, 0f, 0f, 1f } };

        var m1 = new Matrix(testData);
        var m2 = Matrix.Identity(4);

        Assert.True(m1 * m2 == m1, "Multiplying a matrix by the identity matrix yields itself.");
    }

    [Fact]
    public void MultiplyTranspose()
    {
        var testData = new float[,] { { 0f , 9f, 3f, 0f }, { 9f, 8f, 0f, 8f }, { 1f, 8f, 5f, 3f }, { 0f, 0f, 5f, 8f } };
        var resultData = new float[,] { { 0f , 9f, 1f, 0f }, { 9f, 8f, 8f, 0f }, { 3f, 0f, 5f, 5f }, { 0f, 8f, 3f, 8f } };

        var m = new Matrix(testData);
        var result = new Matrix(resultData);

        Assert.True(m.Transpose() == result, "Transposing a matrix yeilds the correct value.");
    }

    [Fact]
    public void DeterminantOf2x2Matrix()
    {
        var testData = new float[,] { { 1f , 5f }, { -3f, 2f } };
        var m = new Matrix(testData);

        var result = Matrix.Determinant(m);

        Assert.True(result == 17f, "Determinant of 2x2 matrix is correct.");
    }
}