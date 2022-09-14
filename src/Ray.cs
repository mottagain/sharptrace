
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

        public Ray Transform(Matrix m) 
        {
            return new Ray(m * _origin, m * _direction);
        }

        private Tuple _origin;
        private Tuple _direction;
    }

}