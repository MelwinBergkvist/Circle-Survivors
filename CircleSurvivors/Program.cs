/* Notes:
 * Kommentarerna kommer vara lite svengelska
 * 
*/
using Raylib_cs; //Initierar Raylibs library, måste göras på alla .cs

namespace CircleSurvivors
{
    internal class Program
    {
        static void Main(string[] args) //Allt som ska ritas as raylibs måste vara inom Begin och End drawing
        {
            //class initieringar
            Player player = new Player(Config.WindowSizeWidth/2, Config.WindowSizeHeight/2);
            NPC npc = new NPC();
            //class initieringar - Variabler och skit
            float deltaTime;
            List<NPC> enemies = new List<NPC>();
            for (int i = 0; i < 10; i++)
            {
                enemies.Add(new NPC());
            }
            

            //Variabler
            Raylib.InitWindow(Config.WindowSizeWidth, Config.WindowSizeHeight, "Circle Survivors");
            while (!Raylib.WindowShouldClose()) //Game loop
            {
                Raylib.ClearBackground(Color.White);
                deltaTime = Raylib.GetFrameTime();

                Raylib.BeginDrawing();
                //drawing confines
                player.update(deltaTime);
                player.draw();

                enemies = enemies.OrderBy(enemy => enemyDistance(player, enemy)).ToList();
                //^^sorterar alla enemies i listan baserat på dess distans till spelaren i ascending order
                //sedan skriver över orginella listan med sorterade veriationen

                for (int i = 0; i < enemies.Count; i++)
                {
                    bool closest = i == 0;
                    enemies[i].update(deltaTime, player);
                    enemies[i].draw(closest);
                }
                Raylib.DrawText($"{deltaTime}", 0,0, 32, Color.Black);
                Raylib.SetTargetFPS(60); //nästan som Thread.sleep(16); men bättre
                
                //drawing confines
                Raylib.EndDrawing();
            }
        }
        static float enemyDistance(Player player, NPC npc)
        {
            float dx = npc.x - player.x;
            float dy = npc.y - player.y;
            return dx * dx + dy * dy;
        }
    }
    public static class Config
    {
        public static int WindowSizeWidth = 1600;
        public static int WindowSizeHeight = 800;
    }
}
