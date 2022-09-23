
namespace SharpTrace
{
    using System.Diagnostics;


    public abstract class Pattern 
    {
        public Pattern()
        {
            this.Transform = Matrix.Identity(4);
        }

        public abstract Color StripeAt(Tuple point);

        public abstract Color StripeAtObject(Shape obj, Tuple point);

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

        public override Color StripeAt(Tuple point)
        {
            Debug.Assert(point.IsPoint);

            var abs = (int)Math.Abs(Math.Floor(point.x));
            if (abs % 2 == 0)
            {
                return A;
            }
            return B;
        }

        public override Color StripeAtObject(Shape obj, Tuple point)
        {
            var localPoint = obj.Transform.Inverse() * point;
            localPoint = this.Transform.Inverse() * localPoint;
            return StripeAt(localPoint);
        }

    }
}
