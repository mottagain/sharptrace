
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

        private const float _tolerance = 0.00001f;
    }
}