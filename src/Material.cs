namespace SharpTrace
{

    public class Material
    {
        public Material()
        {
            Color = new Color(1, 1, 1);
            Ambient = 0.1f;
            Diffuse = 0.9f;
            Specular = 0.9f;
            Shininess = 200f;
        }

        public Color Color { get; set; }

        public float Ambient { get; set; }

        public float Diffuse { get; set; }

        public float Specular { get; set; }

        public float Shininess { get; set; }

        public Color Lighting(PointLight light, Tuple position, Tuple eyev, Tuple normalv)
        {
            var effectiveColor = Color.HardamardProduct(this.Color, light.Intensity);

            var lightv = (light.Position - position).Normalize();

            Color ambient = effectiveColor * this.Ambient;
            Color diffuse;
            Color specular;

            var lightDotNormal = Tuple.Dot(lightv, normalv);
            if (lightDotNormal < 0)
            {
                diffuse = new Color(0, 0, 0);
                specular = new Color(0, 0, 0);
            }
            else
            {
                diffuse = effectiveColor * this.Diffuse * lightDotNormal;
            }

            var reflectv = (-lightv).Reflect(normalv);
            var reflectDotEye = Tuple.Dot(reflectv, eyev);

            if (reflectDotEye <= 0) 
            {
                specular = new Color(0, 0, 0);
            }
            else
            {
                var factor = (float)Math.Pow(reflectDotEye, this.Shininess);
                specular = light.Intensity * this.Specular * factor;
            }

            return ambient + diffuse + specular;
        }
    }
}