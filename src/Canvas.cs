namespace SharpTrace
{
    public class Canvas
    {
        public Canvas(int width, int height)
        {
            _width = width;
            _height = height;
            _pixels = new Color[width, height];
        }

        public int Width
        {
            get { return _width; }
        }

        public int Height
        {
            get { return _height; }
        }

        public Color this[int x, int y]
        {
            get
            {
                if (x >= 0 && x < _width && y >= 0 && y < _height)
                {
                    return _pixels[x, y];
                }
                return Color.Black;
            }
            set
            {
                if (x >= 0 && x < _width && y >= 0 && y < _height)
                {
                    _pixels[x, y] = value;
                }
            }
        }

#pragma warning disable CA1416 // This call site is reachable on all platforms.

        public void SaveAsJpeg(string fileName)
        {
            var bitmap = new System.Drawing.Bitmap(_width, _height);

            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    Color sourceColor = _pixels[x, y];
                    var pixel = System.Drawing.Color.FromArgb(ToByte(sourceColor.r), ToByte(sourceColor.g), ToByte(sourceColor.b));
                    bitmap.SetPixel(x, y, pixel);
                }
            }

            bitmap.Save(fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
        }

#pragma warning restore CA1416 // This call site is reachable on all platforms.

        private byte ToByte(float value)
        {
            if (value <= 0.0f) return 0;
            if (value >= 1.0f) return 255;
            return Convert.ToByte(value * 255f);
        }

        private Color[,] _pixels;

        private int _width;
        private int _height;
    }
}