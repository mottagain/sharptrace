
namespace SharpTrace
{

    public class PointLight
    {
        public PointLight(Tuple position, Color intensity) 
        {
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
