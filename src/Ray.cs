
namespace SharpTrace
{
    using System.Diagnostics;

    public class Ray
    {
        public Ray(Tuple origin, Tuple direction)
        {
            Debug.Assert(origin.IsPoint);
            Debug.Assert(direction.IsVector);

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
            var result = _origin + _direction * t;
            Debug.Assert(result.IsPoint);
            return result;
        }

        public Ray Transform(Matrix m)
        {
            return new Ray(m * _origin, m * _direction);
        }

        private Tuple _origin;
        private Tuple _direction;
    }

}