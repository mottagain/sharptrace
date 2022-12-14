namespace SharpTrace
{

    using System.Diagnostics;

    public class Material
    {
        public static Material Glass
        {
            get
            {
                var result = new Material();
                result.Transparency = 0.9f;
                result.Reflectivity = 0.9f;
                result.RefractiveIndex = 1.5f;
                result.Ambient = 0.01f;
                result.Diffuse = 0.09f;
                result.Specular = 1f;
                result.Shininess = 300f;
                return result;
            }
        }
        public Material()
        {
            Color = Color.White;
            Ambient = 0.1f;
            Diffuse = 0.9f;
            Specular = 0.9f;
            Shininess = 200f;
            Reflectivity = 0f;
            Transparency = 0f;
            RefractiveIndex = 1f;
        }

        public Color Color { get; set; }

        public Pattern? Pattern { get; set; }

        public float Ambient { get; set; }

        public float Diffuse { get; set; }

        public float Specular { get; set; }

        public float Shininess { get; set; }

        public float Reflectivity { get; set; }

        public float Transparency { get; set; }

        public float RefractiveIndex { get; set; }

        public Color Lighting(Shape obj, PointLight light, Tuple point, Tuple eyev, Tuple normalv, bool inShadow)
        {
            Debug.Assert(point.IsPoint);
            Debug.Assert(eyev.IsVector);
            Debug.Assert(normalv.IsVector);

            var targetColor = this.Color;
            if (this.Pattern != null)
            {
                targetColor = this.Pattern.PatternAtShape(obj, point);
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