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

        public static readonly Color Black = new Color(0, 0, 0);
        public static readonly Color White = new Color(1, 1, 1);

        public static bool operator == (Color lhs, Color rhs) {
            return 
                MathExt.Near(lhs.r, rhs.r) &&
                MathExt.Near(lhs.g, rhs.g) &&
                MathExt.Near(lhs.b, rhs.b);
        }

        public static bool operator != (Color lhs, Color rhs) {
            return 
                !MathExt.Near(lhs.r, rhs.r) ||
                !MathExt.Near(lhs.g, rhs.g) ||
                !MathExt.Near(lhs.b, rhs.b);
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

        public static Color HardamardProduct(Color lhs, Color rhs) 
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
    }
}
