
namespace SharpTrace
{
    using System.Diagnostics;
    
    public class Sphere : Shape
    {
        public Sphere() : base()
        {
            Origin = Tuple.NewPoint(0, 0, 0);
            Radius = 1;
        }

        public Tuple Origin { get; private set; }

        public float Radius { get; private set; }

        public override Intersections LocalIntersects(Ray r)
        {
            var sphereToRayVector = r.Origin - Tuple.NewPoint(0, 0, 0);

            var a = Tuple.Dot(r.Direction, r.Direction);
            var b = 2 * Tuple.Dot(r.Direction, sphereToRayVector);
            var c = Tuple.Dot(sphereToRayVector, sphereToRayVector) - 1f;

            var discriminant = b * b - 4 * a * c;

            if (discriminant < 0)
            {
                return new Intersections();
            }

            var t1 = (-b - (float)Math.Sqrt(discriminant)) / (2f * a);
            var t2 = (-b + (float)Math.Sqrt(discriminant)) / (2f * a);

            return new Intersections { new Intersection(t1, this), new Intersection(t2, this) };
        }

        public Tuple NormalAt(Tuple worldPoint)
        {
            Debug.Assert(worldPoint.IsPoint);

            var objectPoint = Transform.Inverse() * worldPoint;
            var objectNormal = objectPoint - Tuple.NewPoint(0, 0, 0);
            var worldNormal = Transform.Inverse().Transpose() * objectNormal;
            worldNormal.w = 0;

            return worldNormal.Normalize();
        }
    }
}