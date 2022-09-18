
namespace SharpTrace
{

    using System.Diagnostics;

    public class Intersection : IComparable<Intersection>
    {
        public Intersection(float t, Sphere obj)
        {
            Time = t;
            Object = obj;
        }

        public float Time { get; private set; }

        public Sphere Object { get; private set; }

        public Computations PrepareComputations(Ray r)
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

            result.OverPoint = result.Point + result.NormalVector * 0.005f;

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

        public Sphere? Object { get; set; }

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

        public bool Inside { get; set; }

        private Tuple _point;
        private Tuple _overPoint;
        private Tuple _eyeVector;
        private Tuple _normalVector;
    }

}