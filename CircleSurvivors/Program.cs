using Raylib_cs; //Initierar Raylibs library

namespace CircleSurvivors
{
    internal class Program
    {
        static void Main(string[] args) //Allt som ska ritas as raylibs måste vara inom Begin och End drawing
        {
            //class initieringar
            Player player = new Player(Config.WindowSizeWidth/2, Config.WindowSizeHeight/2);

            //class initieringar
            Raylib.InitWindow(Config.WindowSizeWidth, Config.WindowSizeHeight, "Circle Survivors");
            while (!Raylib.WindowShouldClose()) //Game loop
            {
                Raylib.ClearBackground(Color.White);
                Raylib.BeginDrawing();
                //confines
                player.draw();


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
