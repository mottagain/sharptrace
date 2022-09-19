
namespace SharpTrace
{
    using System.Diagnostics;

    public abstract class Shape
    {
        public Shape() 
        {
            Transform = Matrix.Identity(4);
            Material = new Material();
        }

        public Intersections Intersects(Ray r)
        {
            Ray localRay = r.Transform(this.Transform.Inverse());
            return LocalIntersects(localRay);
        }

        public abstract Intersections LocalIntersects(Ray r);


        public Matrix Transform { get; set; }

        public Material Material { get; set; }

    }
}