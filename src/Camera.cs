namespace SharpTrace
{
    public class Camera
    {
        public Camera(int width, int height, float fov)
        {
            this.Width = width;
            this.Height = height;
            this.Aspect = (float)width / (float)height;
            this.FOV = fov;
            this.Transform = Matrix.Identity(4);

            var halfView = (float)Math.Tan(fov / 2.0);
            if (this.Aspect >= 1f)
            {
                this._halfWidth = halfView;
                this._halfHeight = halfView / this.Aspect;
            }
            else
            {
                this._halfWidth = halfView * this.Aspect;
                this._halfHeight = halfView;
            }

            this.PixelSize = (this._halfWidth * 2) / this.Width;
        }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public float Aspect { get; private set; }

        public float FOV { get; private set; }

        public Matrix Transform { get; set; }

        public float PixelSize { get; private set; }

        public Ray RayForPixel(int x, int y)
        {
            var xOffset = ((float)x + 0.5f) * this.PixelSize;
            var yOffset = ((float)y + 0.5f) * this.PixelSize;

            var worldX = this._halfWidth - xOffset;
            var worldY = this._halfHeight - yOffset;

            var pixel = this.Transform.Inverse() * Tuple.NewPoint(worldX, worldY, -1);
            var origin = this.Transform.Inverse() * Tuple.NewPoint(0, 0, 0);
            var direction = (pixel - origin).Normalize();

            return new Ray(origin, direction);
        }

        public Canvas Render(World w, int maxReflections)
        {
            var image = new Canvas(this.Width, this.Height);

            for (int y = 0; y < this.Height; y++)
            {
                Parallel.For(0, this.Width - 1, (int x, ParallelLoopState loopState) =>
                {
                    var r = this.RayForPixel(x, y);
                    var color = w.ColorAt(r, maxReflections);
                    image[x, y] = color;
                });
            }

            return image;
        }

        private float _halfWidth;
        private float _halfHeight;
    }
}