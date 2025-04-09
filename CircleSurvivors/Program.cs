using Raylib_cs; //Initierar Raylibs library

namespace CircleSurvivors
{
    internal class Program
    {
        static void Main(string[] args) //Allt som ska ritas as raylibs måste vara inom Begin och End drawing
        {
            Raylib.InitWindow(Config.WindowSizeWidth, Config.WindowSizeHeight, "Circle Survivors");
            while (!Raylib.WindowShouldClose()) //Game loop
            {
                Raylib.ClearBackground(Color.White);
                Raylib.BeginDrawing();
                //confines



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
