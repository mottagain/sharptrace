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
        var testData = new float[,] { { 1f, 2f, 3f, 4f }, { 5.5f, 6.5f, 7.5f, 8.5f }, { 9f, 10f, 11f, 12f }, { 13.5f, 14.5f, 15.5f, 16.5f } };

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
        var testData = new float[,] { { -3f, 5f, 0f }, { 1f, -2f, -7f }, { 0f, 1f, 1f } };

        var matrix = new Matrix(testData);

        Assert.True(matrix.Rows == 3 && matrix.Columns == 3, "Construct a 3x3 matrix from test data.");
        Assert.True(MathExt.Near(matrix[0, 0], -3f), "[0,0] has the value it was constructed with.");
        Assert.True(MathExt.Near(matrix[1, 1], -2f), "[1,1] has the value it was constructed with.");
        Assert.True(MathExt.Near(matrix[2, 2], 1f), "[2,2] has the value it was constructed with.");
    }

    [Fact]
    public void Sample2x2MatrixConstruction()
    {
        var testData = new float[,] { { -3f, 5f }, { 1f, -2f } };

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
        var testData = new float[,] { { 1f, 2f, 3f, 4f }, { 5.5f, 6.5f, 7.5f, 8.5f }, { 9f, 10f, 11f, 12f }, { 13.5f, 14.5f, 15.5f, 16.5f } };

        var m1 = new Matrix(testData);
        var m2 = new Matrix(testData);

        Assert.True(m1 == m2, "Two matricies withe same data are equal.");
    }

    [Fact]
    public void MatrixEqualityDifferentDimensions()
    {
        var testData3x3 = new float[,] { { 1f, 2f, 3f }, { 5.5f, 6.5f, 7.5f }, { 9f, 10f, 11f } };
        var testData4x4 = new float[,] { { 1f, 2f, 3f, 4f }, { 5.5f, 6.5f, 7.5f, 8.5f }, { 9f, 10f, 11f, 12f }, { 13.5f, 14.5f, 15.5f, 16.5f } };

        var m1 = new Matrix(testData3x3);
        var m2 = new Matrix(testData4x4);

        Assert.True(m1 != m2, "A 4x4 matrix that matches that data of a 3x3 matrix are nonetheless not equal.");
    }

    [Fact]
    public void MatrixEqualityDifferentData()
    {
        var testData1 = new float[,] { { 1f, 2f, 3f }, { 5.5f, 6.5f, 7.5f }, { 9f, 10f, 11f } };
        var testData2 = new float[,] { { 1f, 2f, 3f }, { 5.5f, 6.5f, 7.5f }, { 9f, 10f, 12f } };

        var m1 = new Matrix(testData1);
        var m2 = new Matrix(testData2);

        Assert.True(m1 != m2, "Matricies with different data are not equal.");
    }

    [Fact]
    public void MultiplyMatricies()
    {
        var testData1 = new float[,] { { 1f, 2f, 3f, 4f }, { 5f, 6f, 7f, 8f }, { 9f, 8f, 7f, 6f }, { 5f, 4f, 3f, 2f } };
        var testData2 = new float[,] { { -2f, 1f, 2f, 3f }, { 3f, 2f, 1f, -1f }, { 4f, 3f, 6f, 5f }, { 1f, 2f, 7f, 8f } };
        var resultData = new float[,] { { 20f, 22f, 50f, 48f }, { 44f, 54f, 114f, 108f }, { 40f, 58f, 110f, 102f }, { 16f, 26f, 46f, 42f } };

        var m1 = new Matrix(testData1);
        var m2 = new Matrix(testData2);
        var result = new Matrix(resultData);

        Assert.True(m1 * m2 == result, "Multiplying 2 matricies results in the expected value.");
    }

    [Fact]
    public void MultiplyMatrixAndTuple()
    {
        var testData = new float[,] { { 1f, 2f, 3f, 4f }, { 2f, 4f, 4f, 2f }, { 8f, 6f, 4f, 1f }, { 0f, 0f, 0f, 1f } };

        var m = new Matrix(testData);
        var t = new Tuple { x = 1f, y = 2f, z = 3f, w = 1f };

        Assert.True(m * t == new Tuple { x = 18f, y = 24f, z = 33f, w = 1f }, "Multiplying a matrix by a Tuple results in the expected value.");
    }

    [Fact]
    public void MultiplyMatrixByIdentity()
    {
        var testData = new float[,] { { 1f, 2f, 3f, 4f }, { 2f, 4f, 4f, 2f }, { 8f, 6f, 4f, 1f }, { 0f, 0f, 0f, 1f } };

        var m1 = new Matrix(testData);
        var m2 = Matrix.Identity(4);

        Assert.True(m1 * m2 == m1, "Multiplying a matrix by the identity matrix yields itself.");
    }

    [Fact]
    public void MultiplyTranspose()
    {
        var testData = new float[,] { { 0f, 9f, 3f, 0f }, { 9f, 8f, 0f, 8f }, { 1f, 8f, 5f, 3f }, { 0f, 0f, 5f, 8f } };
        var resultData = new float[,] { { 0f, 9f, 1f, 0f }, { 9f, 8f, 8f, 0f }, { 3f, 0f, 5f, 5f }, { 0f, 8f, 3f, 8f } };

        var m = new Matrix(testData);
        var result = new Matrix(resultData);

        Assert.True(m.Transpose() == result, "Transposing a matrix yeilds the correct value.");
    }

    [Fact]
    public void DeterminantOf2x2Matrix()
    {
        var testData = new float[,] { { 1f, 5f }, { -3f, 2f } };
        var m = new Matrix(testData);

        var result = m.Determinant();

        Assert.True(result == 17f, "Determinant of 2x2 matrix is correct.");
    }

    [Fact]
    public void SubMatrix3x3()
    {
        var testData = new float[,] { { 1f, 5f, 0f }, { -3f, 2f, 7f }, { 0f, 6f, -3f } };
        var resultData = new float[,] { { -3f, 2f }, { 0f, 6f } };

        var m = new Matrix(testData);

        var expectedResult = new Matrix(resultData);

        Assert.True(m.SubMatrix(0, 2) == expectedResult, "SubMatrix of a 3x3 Matrix yeilds the correct matrix.");
    }

    [Fact]
    public void SubMatrix4x4()
    {
        var testData = new float[,] { { -6f, 1f, 1f, 6f }, { -8f, 5f, 8f, 6f }, { -1f, 0f, 8f, 2f }, { -7f, 1f, -1f, 1f } };
        var resultData = new float[,] { { -6f, 1f, 6f }, { -8f, 8f, 6f }, { -7f, -1f, 1f } };

        var m = new Matrix(testData);

        var expectedResult = new Matrix(resultData);

        Assert.True(m.SubMatrix(2, 1) == expectedResult, "SubMatrix of a 4x4 Matrix yeilds the correct matrix.");
    }

    [Fact]
    public void MinorOf3x3Matrix()
    {
        var testData = new float[,] { { 3f, 5f, 0f }, { 2f, -1f, -7f }, { 6f, -1f, 5f } };

        var m = new Matrix(testData);

        Assert.True(MathExt.Near(m.Minor(1, 0), 25f), "Minor of 3x3 Matrix yeilds the correct scalar.");
    }

    [Fact]
    public void CofactorOf3x3Matrix()
    {
        var testData = new float[,] { { 3f, 5f, 0f }, { 2f, -1f, -7f }, { 6f, -1f, 5f } };

        var m = new Matrix(testData);

        Assert.True(MathExt.Near(m.Minor(0, 0), -12f), "Minor of 3x3 Matrix yeilds the correct scalar.");
        Assert.True(MathExt.Near(m.Cofactor(0, 0), -12f), "Cofactor of 3x3 Matrix yeilds the correct scalar.");
        Assert.True(MathExt.Near(m.Minor(1, 0), 25f), "Minor of 3x3 Matrix yeilds the correct scalar.");
        Assert.True(MathExt.Near(m.Cofactor(1, 0), -25f), "Cofactor of 3x3 Matrix yeilds the correct scalar.");
    }

    [Fact]
    public void DeterminantOf3x3Matrix()
    {
        var testData = new float[,] { { 1f, 2f, 6f }, { -5f, 8f, -4f }, { 2f, 6f, 4f } };

        var m = new Matrix(testData);

        Assert.True(MathExt.Near(m.Cofactor(0, 0), 56f), "Cofactor of 3x3 Matrix yeilds the correct scalar.");
        Assert.True(MathExt.Near(m.Cofactor(0, 1), 12f), "Cofactor of 3x3 Matrix yeilds the correct scalar.");
        Assert.True(MathExt.Near(m.Cofactor(0, 2), -46f), "Cofactor of 3x3 Matrix yeilds the correct scalar.");
        Assert.True(MathExt.Near(m.Determinant(), -196f), "Determinant of 3x3 Matrix yeilds the correct scalar.");
    }

    [Fact]
    public void DeterminantOf4x4Matrix()
    {
        var testData = new float[,] { { -2f, -8f, 3f, 5f }, { -3f, 1f, 7f, 3f }, { 1f, 2f, -9f, 6f }, { -6f, 7f, 7f, -9f } };

        var m = new Matrix(testData);

        Assert.True(MathExt.Near(m.Cofactor(0, 0), 690f), "Cofactor of 4x4 Matrix yeilds the correct scalar.");
        Assert.True(MathExt.Near(m.Cofactor(0, 1), 447f), "Cofactor of 4x4 Matrix yeilds the correct scalar.");
        Assert.True(MathExt.Near(m.Cofactor(0, 2), 210f), "Cofactor of 4x4 Matrix yeilds the correct scalar.");
        Assert.True(MathExt.Near(m.Cofactor(0, 3), 51f), "Cofactor of 4x4 Matrix yeilds the correct scalar.");
        Assert.True(MathExt.Near(m.Determinant(), -4071f), "Determinant of 4x4 Matrix yeilds the correct scalar.");
    }

    [Fact]
    public void Invertabile4x4Matrix()
    {
        var testData = new float[,] { { 6f, 4f, 4f, 4f }, { 5f, 5f, 7f, 6f }, { 4f, -9f, 3f, -7f }, { 9f, 1f, 7f, -6f } };

        var m = new Matrix(testData);

        Assert.True(MathExt.Near(m.Determinant(), -2120f), "Determinant of 4x4 Matrix yeilds the correct scalar.");
        Assert.True(m.IsInvertable(), "A matrix with a non-zero determinant is invertable.");
    }

    [Fact]
    public void NonInvertabile4x4Matrix()
    {
        var testData = new float[,] { { -4f, 2f, -2f, -3f }, { 9f, 6f, 2f, 6f }, { 0f, -5f, 1f, -5f }, { 0f, 0f, 0f, 0f } };

        var m = new Matrix(testData);

        Assert.True(MathExt.Near(m.Determinant(), 0.0f), "Determinant of 4x4 Matrix yeilds the correct scalar.");
        Assert.False(m.IsInvertable(), "A matrix with a zero determinant is not invertable.");
    }

    [Fact]
    public void InverseOf4x4Matrix()
    {
        var testData = new float[,] { { -5f, 2f, 6f, -8f }, { 1f, -5f, 1f, 8f }, { 7f, 7f, -6f, -7f }, { 1f, -3f, 7f, 4f } };
        var resultData = new float[,] {
            {  0.21805f,  0.45113f,  0.24060f, -0.04511f },
            { -0.80827f, -1.45677f, -0.44361f,  0.52068f },
            { -0.07895f, -0.22368f, -0.05263f,  0.19737f },
            { -0.52256f, -0.81391f, -0.30075f,  0.30639f }
        };

        var a = new Matrix(testData);
        var b = a.Inverse();
        var expectedResult = new Matrix(resultData);

        Assert.True(MathExt.Near(a.Determinant(), 532f), "Determinant of 4x4 Matrix yeilds the correct scalar.");
        Assert.True(MathExt.Near(a.Cofactor(2, 3), -160f), "Cofactor of 4x4 matrix yeilds the correct result.");
        Assert.True(MathExt.Near(b[3, 2], -160f / 532f), "Cofactor of 4x4 matrix yeilds the correct result.");
        Assert.True(MathExt.Near(a.Cofactor(3, 2), 105f), "Cofactor of 4x4 matrix yeilds the correct result.");
        Assert.True(MathExt.Near(b[2, 3], 105f / 532f), "Cofactor of 4x4 matrix yeilds the correct result.");
        Assert.True(b == expectedResult, "Inverse of 4x4 matrix yeilds the correct matrix.");
    }

    [Fact]
    public void ReversingMultiplicaitonThroughInverse()
    {
        var testData1 = new float[,] { { 3f, -9f, 7f, 3f }, { 3f, -8f, 2f, -9f }, { -4f, 4f, 4f, 1f }, { 8f, 2f, 2f, 2f } };
        var testData2 = new float[,] { { 8f, 2f, 2f, 2f }, { 3f, -1f, 7f, 0f }, { 7f, 0f, 5f, 4f }, { 6f, -2f, 0f, 5f } };

        var m1 = new Matrix(testData1);
        var m2 = new Matrix(testData2);
        var m2Inverse = m2.Inverse();

        Assert.True(m1 * m2 * m2Inverse == m1, "A matrix multiplication can be reversed through multiplication by inverse.");
    }

    [Fact]
    public void TranslationMatrix() 
    {
        var transform = Matrix.Translation(5, -3, 2);
        var p = Tuple.NewPoint(-3, 4, 5);

        var expectedResult = Tuple.NewPoint(2, 1, 7);

        Assert.True(transform * p == expectedResult, "Translating a point through a translation matrix works as expected.");
    }
}