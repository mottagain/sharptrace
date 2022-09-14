
namespace SharpTrace
{
    using System.Runtime.InteropServices;


    public struct Matrix : IEquatable<Matrix>
    {
        public Matrix(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            _values = new float[rows * columns];
        }

        public Matrix(float[,] values)
        {
            this.Rows = values.GetLength(0);
            this.Columns = values.GetLength(1);
            _values = new float[Rows * Columns];

            for (int row = 0; row < Rows; row++)
            {
                for (int column = 0; column < Columns; column++)
                {
                    this[row, column] = values[row, column];
                }
            }
        }

        public static Matrix Identity(int size)
        {
            var result = new Matrix(size, size);

            for (int i = 0; i < size; i++)
            {
                result[i, i] = 1;
            }

            return result;
        }

        public static Matrix Translation(float x, float y, float z)
        {
            Matrix result = Identity(4);
            result[0, 3] = x;
            result[1, 3] = y;
            result[2, 3] = z;
            return result;
        }

        public static Matrix Scaling(float x, float y, float z)
        {
            Matrix result = Identity(4);
            result[0, 0] = x;
            result[1, 1] = y;
            result[2, 2] = z;
            return result;
        }

        public static Matrix RotationX(double radians) 
        {
            Matrix result = Identity(4);
            result[1, 1] = (float)Math.Cos(radians);
            result[1, 2] = -(float)Math.Sin(radians);
            result[2, 1] = (float)Math.Sin(radians);
            result[2, 2] = (float)Math.Cos(radians);
            return result;
        }

        public static Matrix RotationY(double radians) 
        {
            Matrix result = Identity(4);
            result[0, 0] = (float)Math.Cos(radians);
            result[0, 2] = (float)Math.Sin(radians);
            result[2, 0] = -(float)Math.Sin(radians);
            result[2, 2] = (float)Math.Cos(radians);
            return result;
        }

        public static Matrix RotationZ(double radians) 
        {
            Matrix result = Identity(4);
            result[0, 0] = (float)Math.Cos(radians);
            result[0, 1] = -(float)Math.Sin(radians);
            result[1, 0] = (float)Math.Sin(radians);
            result[1, 1] = (float)Math.Cos(radians);
            return result;
        }

        public static Matrix Shearing(float xy, float xz, float yx, float yz, float zx, float zy) 
        {
            Matrix result = Identity(4);
            result[0, 1] = xy;
            result[0, 2] = xz;
            result[1, 0] = yx;
            result[1, 2] = yz;
            result[2, 0] = zx; 
            result[2, 1] = zy;
            return result;
        }

        public int Rows { get; private set; }

        public int Columns { get; private set; }

        public float this[int row, int column]
        {
            get
            {
                return _values[row * Columns + column];
            }
            set
            {
                _values[row * Columns + column] = value;
            }
        }

        public static bool operator ==(Matrix lhs, Matrix rhs)
        {
            if (lhs.Rows != rhs.Rows || lhs.Columns != rhs.Columns)
            {
                return false;
            }

            for (int i = 0; i < lhs._values.Length; i++)
            {
                if (!MathExt.Near(lhs._values[i], rhs._values[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool operator !=(Matrix lhs, Matrix rhs)
        {
            return !(lhs == rhs);
        }

        public static Matrix operator *(Matrix lhs, Matrix rhs)
        {
            if (lhs.Columns != rhs.Rows)
            {
                throw new InvalidOperationException("Expected matricies compatable for multiplication.");
            }

            Matrix result = new Matrix(lhs.Rows, lhs.Columns);

            for (int row = 0; row < lhs.Rows; row++)
            {
                for (int column = 0; column < lhs.Columns; column++)
                {
                    float accum = 0f;
                    for (int other = 0; other < lhs.Rows; other++)
                    {
                        accum += lhs[row, other] * rhs[other, column];
                    }
                    result[row, column] = accum;
                }
            }

            return result;
        }

        public static Tuple operator *(Matrix lhs, Tuple rhs)
        {
            if (lhs.Rows != 4 || lhs.Columns != 4)
            {
                throw new InvalidOperationException("Expected a 4x4 matrix for multiplication by a tuple.");
            }

            return new Tuple
            {
                x = lhs[0, 0] * rhs.x + lhs[0, 1] * rhs.y + lhs[0, 2] * rhs.z + lhs[0, 3] * rhs.w,
                y = lhs[1, 0] * rhs.x + lhs[1, 1] * rhs.y + lhs[1, 2] * rhs.z + lhs[1, 3] * rhs.w,
                z = lhs[2, 0] * rhs.x + lhs[2, 1] * rhs.y + lhs[2, 2] * rhs.z + lhs[2, 3] * rhs.w,
                w = lhs[3, 0] * rhs.x + lhs[3, 1] * rhs.y + lhs[3, 2] * rhs.z + lhs[3, 3] * rhs.w,
            };
        }

        public Matrix Transpose()
        {
            var result = new Matrix(Columns, Rows);

            for (int row = 0; row < Rows; row++)
            {
                for (int column = 0; column < Columns; column++)
                {
                    result[column, row] = this[row, column];
                }
            }

            return result;
        }

        public float Determinant()
        {
            if (Rows != Columns)
            {
                throw new InvalidOperationException("Determinant only works on n x n matricies.");
            }

            if (Rows == 1)
            {
                return this[0, 0];
            }
            else if (Rows == 2)
            {
                return this[0, 0] * this[1, 1] - this[0, 1] * this[1, 0];
            }
            else
            {
                float accum = 0;
                for (int column = 0; column < Columns; column++)
                {
                    accum += this[0, column] * Cofactor(0, column);
                }
                return accum;
            }
        }

        public Matrix SubMatrix(int rowToRemove, int columnToRemove)
        {
            var result = new Matrix(Rows - 1, Columns - 1);

            for (int row = 0; row < result.Rows; row++)
            {
                for (int column = 0; column < result.Columns; column++)
                {
                    int sourceRow = (row < rowToRemove) ? (row) : (row + 1);
                    int sourceColumn = (column < columnToRemove) ? (column) : (column + 1);

                    result[row, column] = this[sourceRow, sourceColumn];
                }
            }

            return result;
        }

        public float Minor(int rowToRemove, int columnToRemove)
        {
            var subMatrix = SubMatrix(rowToRemove, columnToRemove);

            return subMatrix.Determinant();
        }

        public float Cofactor(int rowToRemove, int columnToRemove)
        {
            float result = Minor(rowToRemove, columnToRemove);

            if ((rowToRemove + columnToRemove) % 2 != 0)
            {
                return -result;
            }

            return result;
        }

        public bool IsInvertable()
        {
            float determinant = this.Determinant();

            return !MathExt.Near(determinant, 0f);
        }

        public Matrix Inverse()
        {
            float determinant = Determinant();
            if (MathExt.Near(determinant, 0f))
            {
                throw new InvalidOperationException("Matrix is not invertable.");
            }

            var result = new Matrix(Rows, Columns);

            for (int row = 0; row < result.Rows; row++)
            {
                for (int column = 0; column < result.Columns; column++)
                {
                    result[column, row] = Cofactor(row, column) / determinant;
                }
            }

            return result;
        }

        public bool Equals(Matrix other)
        {
            return this == other;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return this == (Matrix)obj;
        }

        public override int GetHashCode()
        {
            int result = 0;
            var intArr = MemoryMarshal.Cast<float, int>(_values);
            foreach (var part in intArr)
            {
                result ^= intArr[0];
            }
            return result;
        }

        private float[] _values;
    }
}
