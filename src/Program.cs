
namespace SharpTrace 
{

    public static class Program 
    {
        public static void Main() 
        {
            // DrawClockHourMarkers();
            // Trajectory();
            //RayCastToSphere();
            //RenderFirstScene();
            RenderRefraction();
        }

        private static void RenderRefraction()
        {
            var wall = new Plane();
            wall.Transform =
                Matrix.Translation(0, 0, 10) *
                Matrix.RotationX(MathExt.PiOver2);
            wall.Material.Pattern = new CheckerPattern(Color.White, Color.Black);
            wall.Material.Specular = 0f;

            var sphere = new Sphere(Material.Glass);
            sphere.Transform = Matrix.Scaling(2, 2, 2);

            var hole = new Sphere();
            hole.Material.Reflectivity = 0;
            hole.Material.RefractiveIndex = 1;
            hole.Material.Transparency = 1;

            var w = new World();
            w.Objects.AddRange(new Shape[] { wall, sphere, hole});
            w.Light = new PointLight(Tuple.NewPoint(0, 10, 0), Color.White);

            var camera = new Camera(1000, 1000, MathExt.PiOver3);
            camera.Transform = Matrix.ViewTransform(Tuple.NewPoint(0f, 0f, -6f), Tuple.NewPoint(0, 0, 0), Tuple.NewVector(0, 1, 0));

            var canvas = camera.Render(w, 5);
            
            canvas.SaveAsJpeg("refraction.jpg");
        }

        private static void RenderFirstScene()
        {
            var floor = new Plane();
            floor.Material.Specular = 0f;
            floor.Material.Pattern = new CheckerPattern(Color.White, Color.Black);
            floor.Material.Reflectivity = 0.25f;
            floor.Material.Pattern.Transform = Matrix.RotationY(MathExt.PiOver4);

            var wall = new Plane();
            wall.Transform =
                Matrix.Translation(0, 0, 10) *
                Matrix.RotationX(MathExt.PiOver2);
            wall.Material.Color = new Color(0.8f, 0.8f, 1.0f);
            wall.Material.Specular = 0f;

            var middle = new Sphere();
            middle.Transform = Matrix.Translation(-0.5f, 1f, 0.5f);
            middle.Material.Color = new Color(0.3f, 0.3f, 0.3f);
            middle.Material.Diffuse = 0.7f;
            middle.Material.Specular = 0.3f;
            middle.Material.Reflectivity = 0.8f;

            var right = new Sphere();
            right.Transform =
                Matrix.Translation(1.5f, 0.5f, -0.5f) *
                Matrix.Scaling(0.5f, 0.5f, 0.5f);
            right.Material.Color = new Color(0.5f, 1f, 0.1f);
            right.Material.Diffuse = 0.7f;
            right.Material.Specular = 0.3f;

            var left = new Sphere();
            left.Transform = 
                Matrix.Translation(-1.5f, 0.33f, -0.75f) *
                Matrix.Scaling(0.33f, 0.33f, 0.33f);
            left.Material.Color = new Color(1f, 0.8f, 0.1f);
            left.Material.Diffuse = 0.7f;
            left.Material.Specular = 0.3f;

            var w = new World();
            w.Objects.AddRange(new Shape[] { floor, wall, middle, right, left });
            w.Light = new PointLight(Tuple.NewPoint(-10, 10, -10), Color.White);

            var camera = new Camera(1000, 1000, MathExt.PiOver3);
            camera.Transform = Matrix.ViewTransform(Tuple.NewPoint(0f, 1.5f, -5f), Tuple.NewPoint(0, 1, 0), Tuple.NewVector(0, 1, 0));

            var canvas = camera.Render(w, 5);
            
            canvas.SaveAsJpeg("Scene1.jpg");
        }

        private static void RayCastToSphere()
        {
            var rayOrigin = Tuple.NewPoint(0, 0, -5);
            var wallZ = 10f;

            var canvasPixels = 500;
            var wallSize = 7.0f;
            var half = wallSize / 2;
            var pixelSize = wallSize / canvasPixels;

            var canvas = new Canvas(canvasPixels, canvasPixels);

            var s = new Sphere();
            s.Transform = Matrix.RotationZ(MathExt.PiOver4);
            s.Material.Pattern = new StripePattern(new Color(1f, 0.1f, 0.1f), Color.White);
            s.Material.Pattern.Transform = Matrix.Scaling(0.25f, 0.25f, 0.25f);

            var lightPosition = Tuple.NewPoint(-10, 10, -10);
            var lightColor = Color.White;
            var light = new PointLight(lightPosition, lightColor);

            for (int y = 0; y < canvasPixels; y++) 
            {
                var worldY = half - pixelSize * y;

                Parallel.For(0, canvasPixels - 1, (x) => {
                    var worldX = -half + pixelSize * x;

                    var wallPosition = Tuple.NewPoint(worldX, worldY, wallZ);

                    var r = new Ray(rayOrigin, (wallPosition - rayOrigin).Normalize());
                    var xs = s.Intersects(r);

                    var hit = xs.Hit();
                    if (hit != null)
                    {
                        var point = r.Position(hit.Time);
                        var normal = hit.Object.NormalAt(point);
                        var eye = -r.Direction;

                        var color = hit.Object.Material.Lighting(hit.Object, light, point, eye, normal, false);
                        canvas[x, y] = color;
                    }
                });
            }

            canvas.SaveAsJpeg("sphere.jpg");
        }

        private static void DrawClockHourMarkers()
        {
            var canvas = new Canvas(1000, 1000);
            var canvasCenter = Tuple.NewPoint(500, 500, 0);

            var clockPosition = Tuple.NewPoint(0, 400, 0);
            var rotation = Matrix.RotationZ(Math.PI / 6);

            var white = Color.White;

            for (int i = 0; i < 12; i++) {
                canvas[(int)clockPosition.x + (int)canvasCenter.x, (int)clockPosition.y + (int)canvasCenter.y] = white;

                clockPosition = rotation * clockPosition;
            }

            canvas.SaveAsJpeg("clock.jpg");
        }

        private static void Trajectory() 
        {
            var canvas = new Canvas(1000, 1000);

            var projectile = new Projectile();
            projectile.Velocity = Tuple.NewVector(1.5f, 2f, 0);
            
            while (projectile.Position.y >= 0.0f)
            {
                Tick(projectile);
                canvas[(int)projectile.Position.x, (int)projectile.Position.y] = new Color(1, 0, 0);
                //Console.WriteLine("Sample at ({0}, {1})", (int)projectile.Position.x, (int)projectile.Position.y);
            }

            canvas.SaveAsJpeg("trajectory.jpg");
        }

        private static void Tick(Projectile projectile) 
        {
            var newPosition = projectile.Position + projectile.Velocity;
            var newVelocity = projectile.Velocity + _gravity + _wind;

            projectile.Position = newPosition;
            projectile.Velocity = newVelocity;
        }

        private static readonly Tuple _gravity = Tuple.NewVector(0, -0.02f, 0);
        private static readonly Tuple _wind = Tuple.NewVector(-0.001f, 0, 0);
    }


    internal class Projectile
    {
        public Tuple Position { get; set; }
        public Tuple Velocity { get; set; }        
    }    
}
