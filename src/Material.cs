namespace SharpTrace
{

    using System.Diagnostics;

    public class Material
    {
        public Material()
        {
            Color = Color.White;
            Ambient = 0.1f;
            Diffuse = 0.9f;
            Specular = 0.9f;
            Shininess = 200f;
        }

        public Color Color { get; set; }

        public Pattern? Pattern { get; set; }

        public float Ambient { get; set; }

        public float Diffuse { get; set; }

        public float Specular { get; set; }

        public float Shininess { get; set; }

        public Color Lighting(PointLight light, Tuple point, Tuple eyev, Tuple normalv, bool inShadow)
        {
            Debug.Assert(point.IsPoint);
            Debug.Assert(eyev.IsVector);
            Debug.Assert(normalv.IsVector);

            var targetColor = this.Color;
            if (this.Pattern != null) 
            {
                targetColor = this.Pattern.StripeAt(point);
            }

            var effectiveColor = Color.HardamardProduct(targetColor, light.Intensity);

            var lightv = (light.Position - point).Normalize();

            Color ambient = effectiveColor * this.Ambient;
            Color diffuse = Color.Black;
            Color specular = Color.Black;

            if (!inShadow) 
            {
                var lightDotNormal = Tuple.Dot(lightv, normalv);
                if (lightDotNormal < 0)
                {
                    diffuse = Color.Black;
                    specular = Color.Black;
                }
                else
                {
                    diffuse = effectiveColor * this.Diffuse * lightDotNormal;

                    var reflectv = (-lightv).Reflect(normalv);
                    var reflectDotEye = Tuple.Dot(reflectv, eyev);

                    if (reflectDotEye <= 0) 
                    {
                        specular = Color.Black;
                    }
                    else
                    {
                        var factor = (float)Math.Pow(reflectDotEye, this.Shininess);
                        specular = light.Intensity * this.Specular * factor;
                    }
                }
            }

            return ambient + diffuse + specular;
        }
    }
}