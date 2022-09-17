
namespace SharpTrace
{

    public static class MathExt
    {
        public static bool Near(float x, float y)
        {
            var diff = Math.Abs(x - y);
            return diff <= _tolerance ||
                diff <= Math.Max(Math.Abs(x), Math.Abs(y)) * _tolerance;
        }

        public static readonly float Sqrt2Over2 = (float)(Math.Sqrt(2) / 2.0);
        public static readonly float PiOver2 = (float)(Math.PI / 2.0);
        public static readonly float PiOver3 = (float)(Math.PI / 3.0);
        public static readonly float PiOver4 = (float)(Math.PI / 4.0);

        private const float _tolerance = 0.00001f;
    }
}