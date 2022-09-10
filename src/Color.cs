namespace SharpTrace
{
    using System.Runtime.InteropServices;


    public struct Color : IEquatable<Color>
    {
        public Color(float r, float g, float b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }

        public float r;
        public float g;
        public float b;

        public static bool operator == (Color lhs, Color rhs) {
            return 
                ApproximatelyEqual(lhs.r, rhs.r) &&
                ApproximatelyEqual(lhs.g, rhs.g) &&
                ApproximatelyEqual(lhs.b, rhs.b);
        }

        public static bool operator != (Color lhs, Color rhs) {
            return 
                !ApproximatelyEqual(lhs.r, rhs.r) ||
                !ApproximatelyEqual(lhs.g, rhs.g) ||
                !ApproximatelyEqual(lhs.b, rhs.b);
        }

        public static Color operator +(Color lhs, Color rhs)
        {
            return new Color
            {
                r = lhs.r + rhs.r,
                g = lhs.g + rhs.g,
                b = lhs.b + rhs.b,
            };
        }

        public static Color operator -(Color lhs, Color rhs) 
        {
            return new Color
            {
                r = lhs.r - rhs.r,
                g = lhs.g - rhs.g,
                b = lhs.b - rhs.b,
            };
        }

        public static Color operator *(Color lhs, Color rhs) 
        {
            return new Color
            {
                r = lhs.r * rhs.r,
                g = lhs.g * rhs.g,
                b = lhs.b * rhs.b,
            };
        }

        public static Color operator *(Color tuple, float scalar)
        {
            return new Color
            {
                r = tuple.r * scalar,
                g = tuple.g * scalar,
                b = tuple.b * scalar,
            };
        }

        public bool Equals(Color other) {
            return this == other;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            
            return this == (Color)obj;
        }
        
        public override int GetHashCode()
        {
            var xIntSpan = MemoryMarshal.Cast<float, int>(new float[] { r, g, b });
            int result = 0;
            foreach (int part in xIntSpan)
            {
                result ^= part;
            }
            return result;
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
