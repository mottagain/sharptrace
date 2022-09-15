
namespace SharpTrace
{

    public class Sphere
    {
        public Sphere()
        {
            Origin = Tuple.NewPoint(0, 0, 0);
            Radius = 1;
            Transform = Matrix.Identity(4);
            Material = new Material();
        }

        public Tuple Origin { get; private set; }

        public float Radius { get; private set; }

        public Matrix Transform { get; set; }

        public Material Material { get; set; }

        public Intersections Intersects(Ray r)
        {
            Ray transformedRay = r.Transform(this.Transform.Inverse());

            var sphereToRayVector = transformedRay.Origin - Tuple.NewPoint(0, 0, 0);

            var a = Tuple.Dot(transformedRay.Direction, transformedRay.Direction);
            var b = 2 * Tuple.Dot(transformedRay.Direction, sphereToRayVector);
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
            var objectPoint = Transform.Inverse() * worldPoint;
            var objectNormal = objectPoint - Tuple.NewPoint(0, 0, 0);
            var worldNormal = Transform.Inverse().Transpose() * objectNormal;
            worldNormal.w = 0;

            return worldNormal.Normalize();
        }
    }
}