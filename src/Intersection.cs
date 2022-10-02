
namespace SharpTrace
{

    using System.Diagnostics;

    public class Intersection : IComparable<Intersection>
    {
        public Intersection(float t, Shape obj)
        {
            Time = t;
            Object = obj;
        }

        public float Time { get; private set; }

        public Shape Object { get; private set; }

        public Computations PrepareComputations(Ray r, Intersections? xs = null)
        {
            var result = new Computations();

            result.Time = this.Time;
            result.Object = this.Object;
            result.Point = r.Position(this.Time);
            result.EyeVector = -r.Direction;
            result.NormalVector = this.Object.NormalAt(result.Point);

            if (Tuple.Dot(result.NormalVector, result.EyeVector) < 0)
            {
                result.Inside = true;
                result.NormalVector = -result.NormalVector;
            }
            else
            {
                result.Inside = false;
            }

            result.ReflectVector = r.Direction.Reflect(result.NormalVector);
            result.OverPoint = result.Point + result.NormalVector * 0.005f;
            result.UnderPoint = result.Point - result.NormalVector * 0.005f;

            if (xs != null)
            {
                var containers = new List<Shape>();
                foreach(var intersection in xs) {
                    if (intersection == this)
                    {
                        result.N1 = containers.Count == 0 ? 1.0f : containers.Last().Material.RefractiveIndex;
                    }

                    if (containers.Contains(intersection.Object))
                    {
                        containers.Remove(intersection.Object);
                    }
                    else
                    {
                        containers.Add(intersection.Object);
                    }
                    if (intersection == this) {
                        result.N2 = containers.Count == 0 ? 1.0f : containers.Last().Material.RefractiveIndex;
                        break;
                    }
                }
            }

            return result;
        }

        public int CompareTo(Intersection? other)
        {
            if (other == null) return 1;

            return Time.CompareTo(other.Time);
        }
    }

    public class Intersections : List<Intersection>
    {
        public Intersection? Hit()
        {
            Sort();

            foreach (var i in this)
            {
                if (i.Time >= 0f)
                {
                    return i;
                }
            }

            return null;
        }
    }

    public class Computations
    {
        public float Time { get; set; }

        public Shape? Object { get; set; }

        public Tuple Point
        {
            get
            {
                return _point;
            }

            set
            {
                Debug.Assert(value.IsPoint);
                _point = value;
            }
        }

        public Tuple OverPoint
        {
            get
            {
                return _overPoint;
            }
            set
            {
                Debug.Assert(value.IsPoint);
                _overPoint = value;
            }
        }

        public Tuple UnderPoint 
        {
            get
            {
                return _underPoint;
            }
            set
            {
                Debug.Assert(value.IsPoint);
                _underPoint = value;
            }
        }

        public Tuple EyeVector
        {
            get
            {
                return _eyeVector;
            }

            set
            {
                Debug.Assert(value.IsVector);
                _eyeVector = value;
            }
        }

        public Tuple NormalVector
        {
            get
            {
                return _normalVector;
            }

            set
            {
                Debug.Assert(value.IsVector);
                _normalVector = value;
            }
        }

        public Tuple ReflectVector
        {
            get
            {
                return _reflectVector;
            }

            set
            {
                Debug.Assert(value.IsVector);
                _reflectVector = value;
            }
        }

        public float N1 { get; set; }

        public float N2 { get; set; }

        public bool Inside { get; set; }

        public float Schlick() 
        {
            // Find the Cosine of the angle between the eye and normal vectors
            var cos = Tuple.Dot(this.EyeVector, this.NormalVector);

            // Total internal reflection can only occur if n1 > n2
            if (this.N1 > this.N2) 
            {
                var n = this.N1 / this.N2;
                var sin2_t = n * n * (1f - cos * cos);
                if (sin2_t > 1f) 
                {
                    return 1f;
                }

                // Compute the cosine of theta_t using trig identity
                var cos_t = (float)Math.Sqrt(1f - sin2_t);

                // When n1 > n2, use cos(theta_t) instead
                cos = cos_t;
            }

            var r0 = (float)Math.Pow((this.N1 - this.N2) / (this.N1 + this.N2), 2);
            return r0 + (1f - r0) * (float)Math.Pow(1f - cos, 5);
        }

        private Tuple _point;
        private Tuple _overPoint;
        private Tuple _underPoint;
        private Tuple _eyeVector;
        private Tuple _normalVector;
        private Tuple _reflectVector;
    }

}