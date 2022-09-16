namespace SharpTrace
{

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

        public PointLight? Light { get; set; }

        private List<Sphere> _objects = new List<Sphere>();
    }
}
