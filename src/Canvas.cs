namespace SharpTrace
{

    public class Canvas
    {
        public Canvas(int width, int height) 
        {
            _width = width;
            _height = height;
        }

        public int Width 
        {
            get { return _width; }
        }

        public int Height 
        {
            get { return _height; }
        }

        private int _width;
        private int _height;
    }
}