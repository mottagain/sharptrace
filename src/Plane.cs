
namespace SharpTrace
{
    using System.Diagnostics;


    public class Plane : Shape
    {
        public Plane(Material? material = null) : base(material)
        {
        }

        public override Intersections LocalIntersects(Ray r)
        {
            if (MathExt.Near(r.Direction.y, 0f))
            {
                return new Intersections();
            }

            var t = -r.Origin.y / r.Direction.y;

            var result = new Intersections();
            result.Add(new Intersection(t, this));
            return result;

        }

        public override Tuple LocalNormalAt(Tuple localPoint)
        {
            Debug.Assert(localPoint.IsPoint);

            return Tuple.NewVector(0, 1, 0);
        }
    }
}
