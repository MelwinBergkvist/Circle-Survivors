/* Notes:
 * Kommentarerna kommer vara lite svengelska
 * 
*/
using Raylib_cs;
using System.Numerics; //Initierar Raylibs library, måste göras på alla .cs

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
            float waveCooldown = 3.99f; //3.99 så den inte flashar en 4 vid början av cooldownen
            float spawnTime = 1f;
            int enemieSpawnCount = 10 + (Config.wave * Config.wave);
            int killCount = 0;
            bool startScreen = false;

            //En list som kan draw, update och kolla om något ska despawna, NPC's bullets osv.
            List<Drawable> drawableList = new List<Drawable>();
            List<NPC> enemies = new List<NPC>();
            drawableList.Add(player);
            PowerUps powerUps = new PowerUps();


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

                if (!startScreen)
                {
                    Raylib.ClearBackground(Color.Black);
                    Raylib.BeginDrawing();

                    int startScreenText = Raylib.MeasureText("Welcome to Circle survivors!", 64);
                    Raylib.DrawText("Welcome to Circle survivors!", Config.WindowSizeWidth/2 - startScreenText / 2,Config.WindowSizeHeight/8, 64, Color.White);

                    Raylib.DrawRectangle((Config.WindowSizeWidth / 2)-150, (Config.WindowSizeHeight / 2 + Config.WindowSizeHeight / 4)-38, 300, 100, Color.SkyBlue);
                    Rectangle startButton = new Rectangle((Config.WindowSizeWidth / 2) - 150, (Config.WindowSizeHeight / 2 + Config.WindowSizeHeight / 4) - 38, 300, 100);

                    int startScreenBeginText = Raylib.MeasureText("Click here to begin!", 24);
                    Raylib.DrawText("Click here to begin!", Config.WindowSizeWidth / 2 - startScreenBeginText / 2, Config.WindowSizeHeight / 2 + Config.WindowSizeHeight / 4, 24, Color.DarkBlue);

                    //checkcollisionpointrec kollar om vektoren getmouseposition är inom rektangel startButton vilken är en kopia av drawrectangle vi gjorde innan
                    //om den är sann och left click är också nedklickad samtidigt så körs inte denna if satsen nåmer, om den inte är sann så skippar allt annat för "continue;"
                    bool hoveredStart = Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), startButton);
                    bool clickedStart = Raylib.IsMouseButtonPressed(MouseButton.Left);
                    if (hoveredStart && clickedStart)
                    {
                        startScreen = true;
                    }
                    Raylib.EndDrawing();
                    continue;

                }

                Raylib.ClearBackground(Color.White);
                Raylib.BeginDrawing();
                //drawing confines;

               

                //när alla enemies är döda, ny wave och +1 wave count
                if (enemies.Count <= 0 && enemieSpawnCount <= 0)
                {
                    Config.movementSpeed = 300; //temporärt gör movementspeed högre, simple qol
                    powerUps.draw(deltaTime);
                    powerUps.update(player);
                    int waveCooldownIntParse = (int)waveCooldown;

                    if (Config.isPicked)
                        waveCooldown -= deltaTime;

                    int waveTextWidth = Raylib.MeasureText("Next wave in: 5", 40);
                    int waveTextWidthWait = Raylib.MeasureText("Next wave after power-up has been chosen", 40);
                    int PickUpInstruction = Raylib.MeasureText("Power-up is picked up by walking inside the circle", 18);

                    if (!Config.isPicked)
                    {
                        Raylib.DrawText($"Next wave after power-up has been chosen", Config.WindowSizeWidth / 2 - waveTextWidthWait / 2, Config.WindowSizeHeight / 8, 40, Color.DarkPurple);
                        Color Instruction = Raylib.ColorAlpha(Color.DarkGray, 0.5f);
                        Raylib.DrawText($"Power-up is picked up by walking inside the circle", Config.WindowSizeWidth / 2 - PickUpInstruction / 2, Config.WindowSizeHeight / 2 + (Config.WindowSizeHeight / 4), 18, Instruction);
                    }
                    else
                    {
                        Raylib.DrawText($"Next wave in: {waveCooldownIntParse}", Config.WindowSizeWidth/2 - waveTextWidth/2, Config.WindowSizeHeight/8,40,Color.DarkPurple);
                    }

                    if (waveCooldown < 1)
                    {
                        enemieSpawnCount = 10 + (Config.wave * Config.wave);
                        waveCooldown = 3.99f;
                        Config.wave++;
                        //resettar alla states, annars så får vi inte välja om powerups waven efter
                        Config.isPicked = false;
                        Config.p1 = false;
                        Config.p2 = false;
                        Config.p3 = false;
                        Config.despawnTime = 0.5f;
                        Config.movementSpeed = 100; //tillbaka till segis
                    }
                }

                if (enemieSpawnCount > 0)
                {
                    spawnTime -= deltaTime;
                    if (spawnTime <= 0)
                    {
                        NPC npc = new NPC();
                        enemies.Add(npc);
                        drawableList.Add(npc);
                        if (enemieSpawnCount < 100)
                        {
                            spawnTime = 1f - (enemieSpawnCount/100f);
                        }
                        else spawnTime = 0.1f;
                        enemieSpawnCount--;
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
                            npc.bulletCollision(bullet, deltaTime);
                        }
                        foreach (var enemy in enemies)
                        {
                            player.playerCollision(enemy, deltaTime);
                        }
                    }
                    item.draw();
                }
                
                //Raylib.DrawText($"{deltaTime}", 0,0, 32, Color.Black);
                Raylib.DrawText($"Kill count: {killCount}", 0,0,32, Color.Red);
                Raylib.DrawText($"Wave: {Config.wave}", 0,25,32, Color.Red);
                Raylib.DrawText($"Stat cheet:",0,Config.WindowSizeHeight-50,12, Color.DarkGray);
                Raylib.DrawText($"bullet radius stat: {Config.bulletRadius}",0,Config.WindowSizeHeight-40,12, Color.DarkGray);
                Raylib.DrawText($"bullet damage stat: {Config.bulletDamage}",0,Config.WindowSizeHeight-30,12, Color.DarkGray);
                Raylib.DrawText($"bullet speed stat: {Config.bulletSpeed}",0,Config.WindowSizeHeight-20,12, Color.DarkGray);
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
        //program
        public static int WindowSizeWidth = 1600;
        public static int WindowSizeHeight = 800;

        //npc,enemy
        public static int npcRadius = 10;
        public static int enemyCollisionDamage = 5;

        //player
        public static int movementSpeed = 100;
        public static int playerRadius = 15;

        //bullets
        public static int bulletRadius = 5;
        public static int bulletDamage = 50;
        public static float bulletSpeed = 300f;

        //power-ups
        public static bool isPicked = false;
        public static bool p1 = false;
        public static bool p2 = false;
        public static bool p3 = false;
        public static float despawnTime = 0.5f;

        //wave
        public static int wave = 0;

        //global för alla, det får bara finnas en instans
        public static Player player = new Player(Config.WindowSizeWidth / 2, Config.WindowSizeHeight / 2);
    }
}
