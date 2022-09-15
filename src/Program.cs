
namespace SharpTrace 
{

    public static class Program 
    {
        public static void Main() 
        {
            // DrawClockHourMarkers();
            // Trajectory();
            RayCastToSphere();
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
            s.Material.Color = new Color(1f, 0.2f, 1f);

            var lightPosition = Tuple.NewPoint(-10, 10, -10);
            var lightColor = new Color(1, 1, 1);
            var light = new PointLight(lightPosition, lightColor);

            for (int y = 0; y < canvasPixels; y++) 
            {
                var worldY = half - pixelSize * y;

                for (int x = 0; x < canvasPixels; x++) 
                {
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

                        var color = hit.Object.Material.Lighting(light, point, eye, normal);
                        canvas[x, y] = color;
                    }
                }
            }

            canvas.SaveAsJpeg("sphere.jpg");
        }

        private static void DrawClockHourMarkers()
        {
            var canvas = new Canvas(1000, 1000);
            var canvasCenter = Tuple.NewPoint(500, 500, 0);

            var clockPosition = Tuple.NewPoint(0, 400, 0);
            var rotation = Matrix.RotationZ(Math.PI / 6);

            var white = new Color(1, 1, 1);

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
                canvas[(int)projectile.Position.x, (int)projectile.Position.y] = new Color(200, 0, 0);
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

//     public static class Program 
//     {
//         public static void Main() 
//         {
//             var canvas = new Canvas(1000, 1000);

//             var projectile = new Projectile();
//             projectile.Velocity = Tuple.NewVector(1.5f, 2f, 0);
            
//             while (projectile.Position.y >= 0.0f)
//             {
//                 Tick(projectile);
//                 canvas[(int)projectile.Position.x, (int)projectile.Position.y] = new Color(200, 0, 0);
//                 Console.WriteLine("Sample at ({0}, {1})", (int)projectile.Position.x, (int)projectile.Position.y);
//             }

//             canvas.SaveAsJpeg("output.jpg");
//         }

//         private static void Tick(Projectile projectile) 
//         {
//             var newPosition = projectile.Position + projectile.Velocity;
//             var newVelocity = projectile.Velocity + _gravity + _wind;

//             projectile.Position = newPosition;
//             projectile.Velocity = newVelocity;
//         }

//         private static readonly Tuple _gravity = Tuple.NewVector(0, -0.02f, 0);
//         private static readonly Tuple _wind = Tuple.NewVector(-0.001f, 0, 0);
//     }
    
}
