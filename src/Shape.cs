
namespace SharpTrace
{
    using System.Diagnostics;

    public abstract class Shape
    {
        public Shape(Material? material = null) 
        {
            Transform = Matrix.Identity(4);
            Material = material ?? new Material();
        }

        public Intersections Intersects(Ray r)
        {
            Ray localRay = r.Transform(this.Transform.Inverse());
            return LocalIntersects(localRay);
        }

        public Tuple NormalAt(Tuple worldPoint)
        {
            Debug.Assert(worldPoint.IsPoint);

            var localPoint = Transform.Inverse() * worldPoint;
            var localNormal = this.LocalNormalAt(localPoint);
            var worldNormal = Transform.Inverse().Transpose() * localNormal;
            worldNormal.w = 0;

            return worldNormal.Normalize();
        }


        public abstract Intersections LocalIntersects(Ray r);

        public abstract Tuple LocalNormalAt(Tuple localPoint);


        public Matrix Transform { get; set; }

        public Material Material { get; set; }

    }
}