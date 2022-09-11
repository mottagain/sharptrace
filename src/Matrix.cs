
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

        public Matrix(float[,] values) {
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

        public static bool operator == (Matrix lhs, Matrix rhs) 
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

        public static bool operator != (Matrix lhs, Matrix rhs) 
        {
            return !(lhs == rhs);
        }

        public static Matrix operator * (Matrix lhs, Matrix rhs) 
        {
            if (lhs.Columns != rhs.Rows)
            {
                throw new InvalidOperationException("Expected matricies compatable for multiplication.");
            }

            Matrix result = new Matrix(lhs.Rows, lhs.Columns);

            for (int row = 0; row < lhs.Rows; row++) {
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

        public static Tuple operator * (Matrix lhs, Tuple rhs) 
        {
            if (lhs.Rows != 4 || lhs.Columns != 4)
            {
                throw new InvalidOperationException("Expected a 4x4 matrix for multiplication by a tuple.");
            }

            return new Tuple {
                x = lhs[0, 0] * rhs.x + lhs[0, 1] * rhs.y + lhs[0, 2] * rhs.z + lhs[0, 3] * rhs.w,
                y = lhs[1, 0] * rhs.x + lhs[1, 1] * rhs.y + lhs[1, 2] * rhs.z + lhs[1, 3] * rhs.w,
                z = lhs[2, 0] * rhs.x + lhs[2, 1] * rhs.y + lhs[2, 2] * rhs.z + lhs[2, 3] * rhs.w,
                w = lhs[3, 0] * rhs.x + lhs[3, 1] * rhs.y + lhs[3, 2] * rhs.z + lhs[3, 3] * rhs.w,
            };
        }

        public bool Equals(Matrix other) {
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
