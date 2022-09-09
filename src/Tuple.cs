namespace SharpTrace
{
    using System.Runtime.InteropServices;


    public struct Tuple : IEquatable<Tuple>
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public static Tuple NewPoint(float x, float y, float z)
        {
            return new Tuple() { x = x, y = y, z = z, w = 1.0f };
        }

        public static Tuple NewVector(float x, float y, float z)
        {
            return new Tuple() { x = x, y = y, z = z, w = 0.0f };
        }

        public static bool operator == (Tuple lhs, Tuple rhs) {
            return 
                ApproximatelyEqual(lhs.x, rhs.x) &&
                ApproximatelyEqual(lhs.y, rhs.y) &&
                ApproximatelyEqual(lhs.z, rhs.z) &&
                ApproximatelyEqual(lhs.w, rhs.w);
        }

        public static bool operator != (Tuple lhs, Tuple rhs) {
            return 
                !ApproximatelyEqual(lhs.x, rhs.x) ||
                !ApproximatelyEqual(lhs.y, rhs.y) ||
                !ApproximatelyEqual(lhs.z, rhs.z) ||
                !ApproximatelyEqual(lhs.w, rhs.w);
        }

        public static Tuple operator +(Tuple lhs, Tuple rhs)
        {
            return new Tuple
            {
                x = lhs.x + rhs.x,
                y = lhs.y + rhs.y,
                z = lhs.z + rhs.z,
                w = lhs.w + rhs.w,
            };
        }

        public static Tuple operator -(Tuple lhs, Tuple rhs) 
        {
            return new Tuple
            {
                x = lhs.x - rhs.x,
                y = lhs.y - rhs.y,
                z = lhs.z - rhs.z,
                w = lhs.w - rhs.w,
            };
        }

        public static Tuple operator -(Tuple target) 
        {
            return new Tuple {
                x = -target.x,
                y = -target.y,
                z = -target.z,
                w = -target.w,
            };       
        }

        public static Tuple operator *(Tuple tuple, float scalar)
        {
            return new Tuple
            {
                x = tuple.x * scalar,
                y = tuple.y * scalar,
                z = tuple.z * scalar,
                w = tuple.w * scalar,
            };
        }

        public static Tuple operator /(Tuple tuple, float scalar)
        {
            return new Tuple
            {
                x = tuple.x / scalar,
                y = tuple.y / scalar,
                z = tuple.z / scalar,
                w = tuple.w / scalar,
            };
        }

        public float Magnitude() 
        {
            return (float)Math.Sqrt(x * x + y * y + z * z + w * w);
        }

        public Tuple Normalize() 
        {
            return this / Magnitude();
        }

        public bool Equals(Tuple other) {
            return this == other;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            
            return this == (Tuple)obj;
        }
        
        // override object.GetHashCode
        public override int GetHashCode()
        {
            var xIntSpan = MemoryMarshal.Cast<float, int>(new float[] { x, y, z, w });
            int result = 0;
            foreach (int part in xIntSpan)
            {
                result ^= part;
            }
            return result;
        }

        public bool IsPoint()
        {
            return ApproximatelyEqual(w, 1.0f);
        }

        public bool IsVector()
        {
            return ApproximatelyEqual(w, 0.0f);
        }

        public static bool ApproximatelyEqual(float x, float y)
        {
            var diff = Math.Abs(x - y);
            return diff <= _tolerance ||
                diff <= Math.Max(Math.Abs(x), Math.Abs(y)) * _tolerance;
        }

        private const float _tolerance = 0.00001f;
    }
}
