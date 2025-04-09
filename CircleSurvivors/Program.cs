using Raylib_cs; //Initierar Raylibs library

namespace CircleSurvivors
{
    internal class Program
    {
        static void Main(string[] args) //Allt som ska ritas as raylibs måste vara inom Begin och End drawing
        {
            //class initieringar
            Player player = new Player(Config.WindowSizeWidth/2, Config.WindowSizeHeight/2);

            //class initieringar - Variabler
            float deltaTime;

            //Variabler
            Raylib.InitWindow(Config.WindowSizeWidth, Config.WindowSizeHeight, "Circle Survivors");
            while (!Raylib.WindowShouldClose()) //Game loop
            {
                Raylib.ClearBackground(Color.White);
                deltaTime = Raylib.GetFrameTime();
                Raylib.BeginDrawing();
                //confines
                player.update(deltaTime);
                player.draw();
                Raylib.DrawText($"{deltaTime}", 0,0, 32, Color.Black);
                Raylib.SetTargetFPS(60); //nästan som Thread.sleep(16); men bättre
                
                //confines
                Raylib.EndDrawing();
            }
        }
    }
    public static class Config
    {
        public static int WindowSizeWidth = 1600;
        public static int WindowSizeHeight = 800;
    }
}
