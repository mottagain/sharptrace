
namespace SharpTrace
{
    using System.Diagnostics;


    public abstract class Pattern 
    {
        public Pattern()
        {
            this.Transform = Matrix.Identity(4);
        }

        public abstract Color PatternAt(Tuple point);

        public Color PatternAtShape(Shape obj, Tuple point)
        {
            var localPoint = obj.Transform.Inverse() * point;
            localPoint = this.Transform.Inverse() * localPoint;
            return PatternAt(localPoint);
        }

        public Matrix Transform { get; set; }
    }

    public class StripePattern : Pattern
    {
        public StripePattern(Color a, Color b)
        {
            this.A = a;
            this.B = b;
        }

        public Color A { get; private set; }
        public Color B { get; private set; }

        public override Color PatternAt(Tuple point)
        {
            Debug.Assert(point.IsPoint);

            var abs = (int)Math.Abs(Math.Floor(point.x));
            if (abs % 2 == 0)
            {
                return A;
            }
            return B;
        }
    }

    public class GradientPattern : Pattern 
    {
        public GradientPattern(Color a, Color b)
        {
            this.A = a;
            this.B = b;
            this._distance = b - a;
        }

        public Color A { get; private set; }
        public Color B { get; private set; }

        public override Color PatternAt(Tuple point)
        {
            Debug.Assert(point.IsPoint);

            var result = this.A + _distance * (point.x - (float)Math.Floor(point.x));
            return result;
        }

        private Color _distance;
    }
}
