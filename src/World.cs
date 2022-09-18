namespace SharpTrace
{
    using System.Diagnostics;

    public class World
    {
        public List<Sphere> Objects
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

        public Color ColorAt(Ray r)
        {
            var xs = this.Intersects(r);
            var hit = xs.Hit();
            if (hit != null)
            {
                var comps = hit.PrepareComputations(r);

                return this.ShadeHit(comps);
            }

            return Color.Black;
        }

        public Color ShadeHit(Computations comps)
        {
            if (Light != null)
            {
                bool isShadowed = this.IsShadowed(comps.OverPoint);
                return comps.Object!.Material.Lighting(Light, comps.Point, comps.EyeVector, comps.NormalVector, isShadowed);
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

        public PointLight? Light { get; set; }

        private List<Sphere> _objects = new List<Sphere>();
    }
}
