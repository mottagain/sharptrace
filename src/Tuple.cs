namespace SharpTrace
{
    using System.Diagnostics;
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

        public static bool operator ==(Tuple lhs, Tuple rhs)
        {
            return
                MathExt.Near(lhs.x, rhs.x) &&
                MathExt.Near(lhs.y, rhs.y) &&
                MathExt.Near(lhs.z, rhs.z) &&
                MathExt.Near(lhs.w, rhs.w);
        }

        public static bool operator !=(Tuple lhs, Tuple rhs)
        {
            return
                !MathExt.Near(lhs.x, rhs.x) ||
                !MathExt.Near(lhs.y, rhs.y) ||
                !MathExt.Near(lhs.z, rhs.z) ||
                !MathExt.Near(lhs.w, rhs.w);
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
            return new Tuple
            {
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

        public static float Dot(Tuple lhs, Tuple rhs)
        {
            Debug.Assert(lhs.IsVector);
            Debug.Assert(rhs.IsVector);

            return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z + lhs.w * rhs.w;
        }

        public static Tuple Cross(Tuple lhs, Tuple rhs)
        {
            Debug.Assert(lhs.IsVector);
            Debug.Assert(rhs.IsVector);

            return NewVector(
                lhs.y * rhs.z - lhs.z * rhs.y,
                lhs.z * rhs.x - lhs.x * rhs.z,
                lhs.x * rhs.y - lhs.y * rhs.x
            );
        }

        public Tuple Reflect(Tuple normal)
        {
            Debug.Assert(normal.IsVector);

            return this - normal * 2 * Tuple.Dot(this, normal);
        }

        public bool Equals(Tuple other)
        {
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

        public bool IsPoint
        {
            get
            {
                return MathExt.Near(w, 1.0f);
            }
        }

        public bool IsVector
        {
            get
            {
                return MathExt.Near(w, 0.0f);
            }
        }
    }
}
