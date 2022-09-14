
namespace SharpTrace
{

    public class Intersection : IComparable<Intersection>
    {
        public Intersection(float t, Sphere obj) 
        {
            Time = t;
            Object = obj;
        }

        public float Time { get; private set; }

        public Sphere Object { get; private set; }

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
}