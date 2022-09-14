
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
            var canvas = new Canvas(1000, 1000);
            var canvasCenter = Tuple.NewPoint(500, 500, 0);

            var s = new Sphere();
            s.Transform = Matrix.Scaling(300, 300, 300);

            for (int i = -500; i < 500; i++) 
            {
                for (int j = -500; j < 500; j++) 
                {
                    var r = new Ray(Tuple.NewPoint(i, j, -500), Tuple.NewVector(0, 0, 1));

                    var xs = s.Intersects(r);
                    if (xs.Count > 0) 
                    {
                        canvas[i + (int)canvasCenter.x, j + (int)canvasCenter.y] = new Color(1, 0, 0);
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
