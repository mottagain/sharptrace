namespace SharpTrace
{
    using System.Diagnostics;

    public class World
    {
        public List<Shape> Objects
        {
            get
            {
                return _objects;
            }
        }

        public Intersections Intersects(Ray r)
        {
            var result = new Intersections();

            foreach (var obj in _objects) 
            {
                var localIntersections = obj.Intersects(r);
                result.AddRange(localIntersections);
            }

            result.Sort();
            return result;
        }

        public Color ColorAt(Ray r, int remainingReflections)
        {
            var xs = this.Intersects(r);
            var hit = xs.Hit();
            if (hit != null)
            {
                var comps = hit.PrepareComputations(r);

                return this.ShadeHit(comps, remainingReflections);
            }

            return Color.Black;
        }

        public Color ShadeHit(Computations comps, int remainingReflections)
        {
            if (Light != null)
            {
                bool isShadowed = this.IsShadowed(comps.OverPoint);
                var surface = comps.Object!.Material.Lighting(comps.Object, Light, comps.Point, comps.EyeVector, comps.NormalVector, isShadowed);
                var reflected = this.ReflectedColor(comps, remainingReflections);

                return surface + reflected;
            }

            return Color.Black;
        }

        public bool IsShadowed(Tuple point) 
        {
            Debug.Assert(point.IsPoint);

            if (this.Light != null)
            {
                var v = this.Light.Position - point;

                var distance = v.Magnitude();
                var direction = v.Normalize();

                Ray r = new Ray(point, direction);
                var xs = this.Intersects(r);
                var hit = xs.Hit();
                if (hit != null && hit.Time < distance)
                {
                    return true;                    
                }
            }

            return false;
        }

        public Color ReflectedColor(Computations comps, int remainingReflections)
        {
            if (remainingReflections <= 0 || MathExt.Near(comps.Object!.Material.Reflectivity, 0)) 
            {
                return Color.Black;
            }

            var reflectedRay = new Ray(comps.OverPoint, comps.ReflectVector);
            var color = this.ColorAt(reflectedRay, remainingReflections - 1);

            return color * comps.Object!.Material.Reflectivity;
        }

        public PointLight? Light { get; set; }

        private List<Shape> _objects = new List<Shape>();
    }
}
