
namespace SharpTrace
{
    using System.Diagnostics;


    public abstract class Pattern 
    {
        public abstract Color StripeAt(Tuple point);
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

    }
}
