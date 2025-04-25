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
            float gameOverAlpha = 0f;
            float fadeInSpeed = 50f;
            float bulletCooldown = 1.5f;
            float bulletCooldownTimer = 0;
            int killCount = 0;
            int wave = 0;
            float waveCooldown = 5.99f;

            //En list som kan draw, update och kolla om något ska despawna, NPC's bullets osv.
            List<Drawable> drawableList = new List<Drawable>();
            List<NPC> enemies = new List<NPC>();
            drawableList.Add(player);


            Raylib.InitWindow(Config.WindowSizeWidth, Config.WindowSizeHeight, "Circle Survivors");
            while (!Raylib.WindowShouldClose()) //Game loop
            {
                deltaTime = Raylib.GetFrameTime();
                //om spelaren är död, sluta köra och bara visa en you lose text
                if(player.shouldDespawn())
                {
                    //fade in effect för you lose screen
                    if (gameOverAlpha < 255)
                    {
                        gameOverAlpha += fadeInSpeed * deltaTime;
                        if (gameOverAlpha > 255) gameOverAlpha = 255;
                    }                    
                    Color fade = Raylib.Fade(Color.Red, gameOverAlpha / 255f);
                    //raylib vill ha mellan 0-1 medan vi kör mellan 0-255 så vi kör /255 för att göra raylibs glad
                    //raylibs inbyggda fade in

                    int endTextWidth = Raylib.MeasureText("Game over, You lost!", 64);
                    //x post räknas från början av texten, så vi kör en measureText så vi kan centrera texten
                    Raylib.DrawText("Game over, You lost!", Config.WindowSizeWidth / 2 - endTextWidth/2, Config.WindowSizeHeight / 4, 64, fade);
                    Raylib.EndDrawing();
                    continue;
                }

                Raylib.ClearBackground(Color.White);

                Raylib.BeginDrawing();
                //drawing confines;

                //när alla enemies är döda, ny wave och +1 wave count
                if (enemies.Count <= 0)
                {
                    int waveCooldownIntParse = (int)waveCooldown;
                    waveCooldown -= deltaTime;
                    int waveTextWidth = Raylib.MeasureText("Next wave in: 5", 40);
                    Raylib.DrawText($"Next wave in: {waveCooldownIntParse}", Config.WindowSizeWidth/2 - waveTextWidth/2, Config.WindowSizeHeight/8,40,Color.DarkPurple);
                    if (waveCooldown < 1)
                    {
                        for (int i = 0; i < 10 + (wave * wave); i++)
                        {
                            NPC npc = new NPC();
                            enemies.Add(npc);
                            drawableList.Add(npc);
                        }
                        waveCooldown = 5.99f;
                        wave++;
                    }
                }

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
                            killCount++;
                        }
                        drawableList.RemoveAt(i);                        
                        continue;
                    }
                    
                    item.update(deltaTime);
                    if (item is NPC npc)
                    {
                        //kollar igenom alla bullets och enemies för collision
                        foreach (var bullet in bullets)
                        {
                            npc.bulletCollision(bullet);
                        }
                        foreach (var enemy in enemies)
                        {
                            player.playerCollision(enemy, deltaTime);
                        }
                    }
                    item.draw();
                }
                
                //Raylib.DrawText($"{deltaTime}", 0,0, 32, Color.Black);
                Raylib.DrawText($"Kill count:{killCount}", 0,0,32, Color.Red);
                Raylib.DrawText($"Wave:{wave}", 0,25,32, Color.Red);
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
