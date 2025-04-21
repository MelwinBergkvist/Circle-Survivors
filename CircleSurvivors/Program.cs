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
            Player player = Config.player;
            List<BaseAbility> bullets = new List<BaseAbility>();
            float deltaTime;
            float bulletCooldown = 1.5f;
            float bulletCooldownTimer = 0;

            //En list som kan draw, update och kolla om något ska despawna, NPC's bullets osv.
            List<Drawable> drawableList = new List<Drawable>();
            List<NPC> enemies = new List<NPC>();
            for (int i = 0; i < 10; i++)
            {
                NPC npc = new NPC();
                enemies.Add(npc);
                drawableList.Add(npc);
            }
            drawableList.Add(player);

            Raylib.InitWindow(Config.WindowSizeWidth, Config.WindowSizeHeight, "Circle Survivors");
            while (!Raylib.WindowShouldClose()) //Game loop
            {
                Raylib.ClearBackground(Color.White);
                deltaTime = Raylib.GetFrameTime();

                Raylib.BeginDrawing();
                //drawing confines;

                //make sure att det faktist finns enemies på skärmen,
                //så vi inte försöker skjuta mot något som inte finns
                if (enemies.Count > 0)
                {
                    enemies = enemies.OrderBy(enemy => enemyDistance(player, enemy)).ToList();
                    NPC closestEnemy = enemies[0];
                    //note to self: kommer behöva ta bort från flera listor i framtiden, både drawables och enemies
                    //^^sorterar alla enemies i listan baserat på dess distans till spelaren i ascending order
                    //sedan skriver över orginella listan med sorterade veriationen

                    bulletCooldownTimer += deltaTime;
                    if (bulletCooldown <= bulletCooldownTimer)
                    {
                        bulletCooldownTimer = 0;
                        BaseAbility bullet = new BaseAbility(player, closestEnemy);
                        drawableList.Add(bullet);
                        bullets.Add(bullet);
                    }
                }                

                //Kollar om jag ska despawn itemen annars så draw och update
                for (int i = drawableList.Count-1; i >= 0; i--)
                {
                    var item = drawableList[i];
                    if (item.shouldDespawn())
                    {
                        if (item is NPC enemyNpc)
                        {
                            enemies.Remove(enemyNpc);
                        }
                        drawableList.RemoveAt(i);
                        continue;
                    }
                    
                    item.update(deltaTime);
                    if (item is NPC npc)
                    {
                        foreach (var bullet in bullets)
                        {
                            npc.bulletCollision(bullet);
                        }
                    }
                    item.draw();
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
        public static int npcRadius = 10;
        public static int bulletRadius = 5;

        //global för alla, det får bara finnas en instans
        public static Player player = new Player(Config.WindowSizeWidth / 2, Config.WindowSizeHeight / 2);
    }
}
