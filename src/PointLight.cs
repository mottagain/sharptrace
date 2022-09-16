
namespace SharpTrace
{
    using System.Diagnostics;

    public class PointLight
    {
        public PointLight(Tuple position, Color intensity) 
        {
            Debug.Assert(position.IsPoint);
            
            _position = position;
            _intensity = intensity;
        }

        public Tuple Position
        {
            get 
            {
                return _position;                
            }
        }

        public Color Intensity
        {
            get 
            {
                return _intensity;
            }
        }

        private Tuple _position;
        private Color _intensity;
    }
}
