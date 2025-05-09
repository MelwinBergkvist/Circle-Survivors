/* Notes:
 * Kommentarerna kommer vara lite svengelska
 * 
 * alla raylib.measuretext måste deklareras där de används, jag kunde ha deklararet det som tex "int Text;" och sedan ändrat värdet men jag ansåg att det ser fult och klunky ut
 * och inte i början av main, jag vet inte varför men det fungerade
 * inte när jag testade det
 * 
 * Min standard:
 * alla functions små bokstäver förutom Main den ska alltid vara kapitaliserad (den e special)
 * alla datatyper camelCase med första alltid små bokstav
 * alla klasser med stor bokstäv i början
*/
using Raylib_cs; //Initierar Raylibs library, måste göras på alla .cs
using System;
using System.Numerics; 

namespace CircleSurvivors
{
    internal class Program
    {

        static void Main(string[] args) //Allt som ska ritas as raylibs måste vara inom Begin och End drawing
        {
            SplashTexts splashText = new SplashTexts();

            List<BaseAbility> bullets = Config.bullets;
            List<EnemyBullets> enemyBullet = Config.enemyBullet;
            List<Drawable> drawableList = Config.drawableList;
            List<NPC> enemies = Config.enemies;

            //floats
            float deltaTime;
            float waveCooldown = 3.99f; //3.99 så den inte flashar en 4 vid början av cooldownen
            float spawnTime = 1f;
            float collisionCooldown = 1.5f;
            float collisionCooldownTimer = 0f;

            //bools
            bool isPaused = false;
            bool noMoreTempSpeed = false;

            


            //Colors
            Color pauseOverlay = new Color(150, 150, 150, 200);
            Color Instruction = Raylib.ColorAlpha(Color.DarkGray, 0.5f);

            //En list som kan draw, update och kolla om något ska despawna, NPC's bullets osv.
            drawableList.Add(Config.player);
            PowerUps powerUps = new PowerUps();
            GUI gui = new GUI();


            Raylib.InitWindow(Config.WindowSizeWidth, Config.WindowSizeHeight, "Circle Survivors - " + SplashTexts.GetSplashText());
            while (!Raylib.WindowShouldClose()) //Game loop
            {
                deltaTime = Raylib.GetFrameTime();
                if (Config.player.ShouldDespawn())
                {
                    gui.DeathScreen(deltaTime);
                    continue;
                }

                if (!Config.startScreen)
                {
                    gui.StartScreen(deltaTime);
                    continue;
                }

                Raylib.ClearBackground(Color.White);
                Raylib.BeginDrawing();
                //drawing confines;

                if (Raylib.IsKeyPressed(KeyboardKey.K))
                    Config.player.healthpoints -= 25;

                //Pause meny, WIP
                if (isPaused)
                {
                    Raylib.DrawRectangle(0,0,Config.WindowSizeWidth, Config.WindowSizeHeight, pauseOverlay);

                    if (Raylib.IsKeyPressed(KeyboardKey.G))
                        isPaused = false;
                    else
                        Raylib.EndDrawing();
                        continue;
                }

                gui.Timer(deltaTime);

                //när alla enemies är döda, ny wave och +1 wave count
                if (enemies.Count <= 0 && Config.enemieSpawnCount <= 0 && Config.firstWaveAfterRestart != true)
                {
                    bullets.Clear();

                    //stoppar timern mellan waves
                    Config.countTime = false;

                    if (!noMoreTempSpeed)
                        Config.tempMovementSpeedHolder = Config.player.movementSpeed;
                    noMoreTempSpeed = true;

                    Config.player.movementSpeed = 300; //temporärt gör movementspeed högre, simple qol

                    powerUps.draw(deltaTime);
                    powerUps.update();

                    if (Config.isPicked)
                        waveCooldown -= deltaTime;


                    if (!Config.isPicked)
                    {
                        int waveTextWidthWait = Raylib.MeasureText("Next wave after power-up has been chosen", 40);
                        Raylib.DrawText($"Next wave after power-up has been chosen", Config.WindowSizeWidth / 2 - waveTextWidthWait / 2, Config.WindowSizeHeight / 8, 40, Color.DarkPurple);

                        int PickUpInstruction = Raylib.MeasureText("Power-up is picked up by walking inside the circle and pressing E", 18);
                        Raylib.DrawText($"Power-up is picked up by walking inside the circle and pressing E", Config.WindowSizeWidth / 2 - PickUpInstruction / 2, Config.WindowSizeHeight / 2 + (Config.WindowSizeHeight / 4), 18, Instruction);
                    }
                    else
                    {
                        int waveTextWidth = Raylib.MeasureText("Next wave in: 5", 40); //5 och inte {waveCooldown} för smoother centrering
                        Raylib.DrawText($"Next wave in: {(int)waveCooldown}", Config.WindowSizeWidth/2 - waveTextWidth/2, Config.WindowSizeHeight/8,40,Color.DarkPurple);
                    }

                    if (waveCooldown < 1)
                    {
                        Config.enemieSpawnCount = 10 + (Config.wave * Config.wave); //scaling
                        waveCooldown = 3.99f; //nära 4 men inte 4, annars flashar en 4 pförsta framen
                        Config.wave++;
                        //resettar alla states, annars så får vi inte välja om powerups waven efter
                        Config.isPicked = false;
                        Config.p1 = false;
                        Config.p2 = false;
                        Config.p3 = false;
                        Config.hasRolledThisRound = false;
                        Config.despawnTime = 0.5f;
                        Config.player.movementSpeed = Config.tempMovementSpeedHolder; //tillbaka till segis
                        noMoreTempSpeed = false;
                    }
                }

                //Om man dör och respawnar så ser denna till att ingen powerup spawnas första rundan
                Config.firstWaveAfterRestart = false;

                if (Config.enemieSpawnCount > 0)
                {
                    //börjar den pausade timern
                    Config.countTime = true;
                    
                    //ser till att inta alla enemies spawnar direkt och samtidigt
                    spawnTime -= deltaTime;
                    if (spawnTime <= 0)
                    {
                        //eftersom NPCn är mer än en så deklrareras den mer än en gång
                        NPC npc = new NPC();
                        enemies.Add(npc);
                        drawableList.Add(npc);
                        
                        //scaling för hur snabbt saker ska spawna så det inte tar 30 min per runda,
                        //också ser till att det aldrig blir instant
                        if (Config.enemieSpawnCount < 100)
                        {
                            spawnTime = 1f - (Config.enemieSpawnCount /100f);
                        }
                        else spawnTime = 0.1f;
                        Config.enemieSpawnCount--;
                    }
                }

                //make sure att det faktist finns enemies på skärmen,
                //så vi inte försöker skjuta mot något som inte finns
                //if (enemies.Count > 0)
                //{
                //    //exact samma logik som bullet cooldownen innan men för enemies
                //    //här har vi mer än en enemy so vi loopar igen hela enemies listan
                //    //så alla enemies individiuelt har sin egen cooldown
                //    enemyBulletCooldownTimer += deltaTime;
                //    if (Config.enemyBulletCooldown <= enemyBulletCooldownTimer)
                //    {
                //        foreach (var enemy in  enemies)
                //        {
                //            if (enemy.shouldShoot)
                //            {
                //                enemyBulletCooldownTimer = 0;
                //                EnemyBullets enemyBullets = new EnemyBullets(enemy);
                //                drawableList.Add(enemyBullets);
                //                enemyBullet.Add(enemyBullets);
                //            }
                //        }
                //    }
                //}                

                //Kollar om jag ska despawn itemen annars så draw och update
                for (int i = drawableList.Count-1; i >= 0; i--)
                {
                    var item = drawableList[i];
                    //Drawable interfacet har en shouldDespawn function,
                    //så alla .cs som implementerar interfacet har en shouldDespawn med sina egna requierments
                    if (item.ShouldDespawn())
                    {
                        if (item is NPC enemyNpc)
                        {
                            enemies.Remove(enemyNpc);
                            Config.killCount++;
                        }
                        if (item is BaseAbility bullet)
                        {
                            bullets.Remove(bullet);
                        }
                        drawableList.RemoveAt(i);

                        //om det ska despawna behövs det inte draw eller update så vi skippar till nästa
                        continue;
                    }
                    
                    collisionCooldownTimer += deltaTime;
                    item.Update(deltaTime);
                    //om itemet i listan är från NPC körs if satsen
                    if (item is NPC npc)
                    {
                        //för varje bullet i bullets, kolla för collision med npc
                        foreach (var bullet in bullets)
                        {
                            npc.BulletCollision(bullet, deltaTime);
                        }
                        //för varje enemy i listan, kolla om collision cooldown är över, isåfall skada spelaren och resetta timern
                        foreach (var enemy in enemies)
                        {
                            if (collisionCooldownTimer >= collisionCooldown)
                            {
                                collisionCooldownTimer = 0;
                                npc.PlayerCollision(enemy, deltaTime);
                            }
                        }
                        //för varje enemybullet i listan, kolla om despawn requierments har mötts
                        foreach (var enemyBullets in enemyBullet)
                        {
                            enemyBullets.ShouldDespawn();
                        }
                    }
                    item.Draw();
                }

                Dashboard.draw();

                Raylib.SetTargetFPS(60); //nästan som Thread.sleep(16); men bättre

                if (Raylib.IsKeyDown(KeyboardKey.P))
                    isPaused = true;

                //drawing confines
                Raylib.EndDrawing();
            }
        }
        //static float EnemyDistance(Player player, NPC npc)
        //{
        //    //Hur jag definerar i vilken ordning enemies ska vara sorted
        //    float dx = npc.x - player.x;
        //    float dy = npc.y - player.y;
        //    return dx * dx + dy * dy;
        //}
        
    }
    public static class Config
    {
        //program
        public static int WindowSizeWidth = 1600;
        public static int WindowSizeHeight = 800;
        public static int killCount = 0;
        public static bool countTime = true;
        public static bool firstWaveAfterRestart = false;
        public static bool startScreen = false;

        //npc,enemy
        public static int npcRadius = 10;
        public static int enemyCollisionDamage = 5;
        public static int enemyBulletDamage = 5;
        public static int enemyBulletRadius = 5;
        public static float enemyBulletSpeed = 300f;
        public static float enemyBulletCooldown = 1.5f;
        public static int enemieSpawnCount = 10 + (wave * wave);

        //player
        public static int tempMovementSpeedHolder;

        //bullets
        public static int bulletRadius = 5;
        public static int bulletDamage = 50;
        public static float bulletSpeed = 300f;
        public static float bulletCooldown = 1.5f;

        //power-ups
        public static bool p1, p2, p3 = false;
        public static bool isPicked = false;
        public static bool hasRolledThisRound = false;
        public static float despawnTime = 0.5f;

        //wave
        public static int wave = 1;

        //global för alla, det får bara finnas en instans
        public static Player player = new Player(Config.WindowSizeWidth / 2, Config.WindowSizeHeight / 2);
        public static List<Drawable> drawableList = new List<Drawable>();
        public static List<BaseAbility> bullets = new List<BaseAbility>();
        public static List<NPC> enemies = new List<NPC>();
        public static List<EnemyBullets> enemyBullet = new List<EnemyBullets>();

        public static void ResetGameState()
        {
            wave = 1;
            bulletDamage = 50;
            bulletRadius = 5;
            bulletSpeed = 300f;
            player.healthpoints = 100;
            player.maxHealthpoints = 100;
            killCount = 0;
            enemies.Clear();
            drawableList.Clear();
            bullets.Clear();
            drawableList.Add(player);
            firstWaveAfterRestart = true;
            enemieSpawnCount = 10 + wave * wave;
            player.x = Config.WindowSizeWidth / 2;
            player.y = Config.WindowSizeHeight / 2;
        }
    }
}
