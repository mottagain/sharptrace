
namespace SharpTrace
{

    public class Ray
    {
        public Ray(Tuple origin, Tuple direction)
        {
            _origin = origin;
            _direction = direction;
        }

        public Tuple Origin
        {
            get
            {
                return _origin;
            }
        }

        public Tuple Direction
        {
            get
            {
                return _direction;
            }
        }

        public Tuple Position(float t)
        {
            return _origin + _direction * t;
        }

        private Tuple _origin;
        private Tuple _direction;
    }

}