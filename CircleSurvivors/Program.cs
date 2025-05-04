/* Notes:
 * Kommentarerna kommer vara lite svengelska
 * 
 * alla raylib.measuretext måste deklareras där de används
 * och inte i början av main, jag vet inte varför men det fungerade
 * inte när jag testade det
*/
using Raylib_cs; //Initierar Raylibs library, måste göras på alla .cs
using System.Numerics; 

namespace CircleSurvivors
{
    internal class Program
    {
        static void Main(string[] args) //Allt som ska ritas as raylibs måste vara inom Begin och End drawing
        {
            Player player = Config.player;
            List<BaseAbility> bullets = new List<BaseAbility>();
            List<EnemyBullets> enemyBullet = new List<EnemyBullets>();
            float deltaTime;
            float gameOverAlpha = 0f;
            float startScreenAlpha = 0f;
            float fadeInSpeed = 50f;
            float fadeInSpeedStart = 100f;
            float bulletCooldownTimer = 0f;
            float enemyBulletCooldownTimer = 0f;
            float waveCooldown = 3.99f; //3.99 så den inte flashar en 4 vid början av cooldownen
            float spawnTime = 1f;
            float timeAliveSeconds = 0f;
            float timeAliveMinutes = 0f;
            float collisionCooldown = 1.5f;
            float collisionCooldownTimer = 0f;
            int enemieSpawnCount = 10 + (Config.wave * Config.wave);
            int killCount = 0;
            int splashText = 0;
            bool startScreen = false;
            bool startFadeIn = false;
            bool firstWaveAfterRestart = false;
            bool countTime = true;
            bool noMoreTemps = false;
            string[] splashTextsArray = 
            {
                //common
                "Bravo 6, going circles.", "Circle of life... and death, probably.", "Circle up buddy", "This isn't a drill! (it's a spiral)",
                "I'm in love with the shape of you - by circle sheeran", "On edge? We don't have edges here.", "Pop a wheelie", "I'm going aRound in circles",
                "alternative title: Geometry wars", "Also try Circle Diers", "360 degrees of dangers", "Axis of evil", "No corners to hide behind",
                "Roundabout rage", "Wheel of misfortune", "esnälla ge mig ett A", "Rotating regrets", "Circle one: fight!... i mean round",
                "I need to count how many splashes made after this", "Rolling until further notice", "You're on a roll!... or not",

                //rare
                "Insert better pun here.", "This text does nothing.", "in circles since 1997",
                "Tested on humans, not responsibly.", "This splash text is {Shadow Slave chapter 360}"
            }; //16st, rare 5st
            Random randomSplashText = new Random();
            if (randomSplashText.Next(0,101) < 90)
            {
                splashText = randomSplashText.Next(0,16);
            }
            else
            {
                splashText = randomSplashText.Next(16,21);
            }

            //En list som kan draw, update och kolla om något ska despawna, NPC's bullets osv.
            List<Drawable> drawableList = new List<Drawable>();
            List<NPC> enemies = new List<NPC>();
            drawableList.Add(player);
            PowerUps powerUps = new PowerUps();


            Raylib.InitWindow(Config.WindowSizeWidth, Config.WindowSizeHeight, "Circle Survivors - " + splashTextsArray[splashText]);
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
                    Color fadeRed = Raylib.Fade(Color.Red, gameOverAlpha / 255f);
                    Color fadeGray = Raylib.Fade(Color.DarkGray, gameOverAlpha / 255f);
                    Color fadeSkyBlue = Raylib.Fade(Color.SkyBlue, gameOverAlpha / 255f);
                    //raylib vill ha mellan 0-1 medan vi kör mellan 0-255 så vi kör /255 för att göra raylibs glad
                    //raylibs inbyggda fade in

                    Raylib.ClearBackground(Color.Gray);
                    int endTextWidth = Raylib.MeasureText("Game over, You lost!", 64);
                    //x post räknas från början av texten, så vi kör en measureText så vi kan centrera texten
                    Raylib.DrawText("Game over, You lost!", Config.WindowSizeWidth / 2 - endTextWidth/2, Config.WindowSizeHeight / 4, 64, fadeRed);

                    int timeAliveText = Raylib.MeasureText($"You stayed alive for {(int)timeAliveMinutes} minutes and {(int)timeAliveSeconds} seconds!",16);
                    Raylib.DrawText($"You stayed alive for {(int)timeAliveMinutes} minutes and {(int)timeAliveSeconds} seconds!", Config.WindowSizeWidth / 2 - timeAliveText / 2, Config.WindowSizeHeight/ 4 + 100, 16, fadeSkyBlue);
                    int killCountText = Raylib.MeasureText($"You killed {killCount} enemies during your run!", 16);
                    Raylib.DrawText($"You killed {killCount} enemies during your run!", Config.WindowSizeWidth / 2 - killCountText / 2, Config.WindowSizeHeight / 4 + 120, 16, fadeSkyBlue);
                    int waveText = Raylib.MeasureText($"You survived for {Config.wave-1} waves!", 16);
                    Raylib.DrawText($"You survived for {Config.wave-1} waves!", Config.WindowSizeWidth / 2 - waveText / 2, Config.WindowSizeHeight / 4 + 140, 16, fadeSkyBlue);


                    int restartTextWidth = Raylib.MeasureText("Press R to restart", 24);
                    Raylib.DrawText("Press R to restart", Config.WindowSizeWidth / 2 - restartTextWidth/2, Config.WindowSizeHeight / 2 + Config.WindowSizeHeight/4, 24, fadeGray);
                    if (Raylib.IsKeyPressed(KeyboardKey.R)) // resettar allting
                    {
                        Config.wave = 1;
                        Config.bulletDamage = 50;
                        Config.bulletRadius = 5;
                        Config.bulletSpeed = 300f;
                        Config.playerHealthpoints = 100;
                        Config.hasBossSpawned = false;
                        killCount = 0;
                        enemies.Clear();
                        drawableList.Clear();
                        bullets.Clear();
                        drawableList.Add(player);
                        firstWaveAfterRestart = true;
                        enemieSpawnCount = 11;
                        timeAliveSeconds = 0;
                        timeAliveMinutes = 0;
                        player.x = Config.WindowSizeWidth / 2;
                        player.y = Config.WindowSizeHeight / 2;
                    }
                    else
                    {
                        Raylib.EndDrawing();
                        continue;
                    }
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

                    Rectangle tutorialButton = new Rectangle(0,Config.WindowSizeHeight/2, 120, 50);
                    Raylib.DrawRectangle(0, Config.WindowSizeHeight/2, 120, 50, Color.DarkGray);
                    Raylib.DrawText("How to play?", 10, Config.WindowSizeHeight/2+14, 16, Color.Black);
                    bool isTutorialHovered = Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), tutorialButton);
                    if (isTutorialHovered)
                    {
                        //ritar om rectangeln i annan färg så man ser att den är hovered
                        Raylib.DrawRectangle(0, Config.WindowSizeHeight / 2, 120, 50, Color.Gray);
                        Raylib.DrawText("How to play?", 10, Config.WindowSizeHeight / 2 + 14, 16, Color.Black);
                        
                        //tutorial så man vet vad man ska göra
                        Raylib.DrawRectangle(Config.WindowSizeWidth/6-10, Config.WindowSizeHeight/3-10, 320, 220, Color.DarkGray);
                        Raylib.DrawText("Welcome to Circle Survivors!", Config.WindowSizeWidth/6, Config.WindowSizeHeight/3,16,Color.White);
                        Raylib.DrawText("You play as the green circle,", Config.WindowSizeWidth/6, Config.WindowSizeHeight/3+40,16,Color.White);
                        Raylib.DrawText("you move using WASD or direction keys.", Config.WindowSizeWidth/6, Config.WindowSizeHeight/3+60,16,Color.White);
                        Raylib.DrawText("The game automatically shoots for you.", Config.WindowSizeWidth/6, Config.WindowSizeHeight/3+100,16,Color.White);
                        Raylib.DrawText("So you just need to stay alive.", Config.WindowSizeWidth/6, Config.WindowSizeHeight/3+120,16,Color.White);
                        Raylib.DrawText("Enemies spawn in waves,", Config.WindowSizeWidth/6, Config.WindowSizeHeight/3+160,16,Color.White);
                        Raylib.DrawText("At the end a wave you get a powerup.", Config.WindowSizeWidth/6, Config.WindowSizeHeight/3+180,16,Color.White);
                    }

                    //checkcollisionpointrec kollar om vektoren getmouseposition är inom rektangel startButton vilken är en kopia av drawrectangle vi gjorde innan
                    //om den är sann och left click är också nedklickad samtidigt så körs inte denna if satsen nåmer, om den inte är sann så skippar allt annat för "continue;"
                    bool hoveredStart = Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), startButton);
                    bool clickedStart = Raylib.IsMouseButtonPressed(MouseButton.Left);

                    //måste inte ha if satsen nestad men det ser finare ut tycker jag själv, fungerar exakt likadant att ha två ifs separate
                    if (hoveredStart) //så man ser att den är hovered
                    {
                        Raylib.DrawRectangle((Config.WindowSizeWidth / 2) - 150, (Config.WindowSizeHeight / 2 + Config.WindowSizeHeight / 4) - 38, 300, 100, Color.Blue);
                        Raylib.DrawText("Click here to begin!", Config.WindowSizeWidth / 2 - startScreenBeginText / 2, Config.WindowSizeHeight / 2 + Config.WindowSizeHeight / 4, 24, Color.Black);
                        if (clickedStart)
                        {
                            startFadeIn = true; //initierar fade-in
                        }
                    }

                    if (startFadeIn)
                    {
                        if (startScreenAlpha < 255)
                        {
                            startScreenAlpha += fadeInSpeedStart * deltaTime;
                            if (startScreenAlpha > 255) startScreenAlpha = 255;
                        }
                        Color fadeIn = Raylib.Fade(Color.White, startScreenAlpha / 255f);
                        Raylib.DrawRectangle(0, 0, Config.WindowSizeWidth, Config.WindowSizeHeight, fadeIn);
                        if (startScreenAlpha == 255) //när fade-in är klar (opaciteten är 100%) så säger vi att startscreenen är klar
                            startScreen = true;
                    }
                    Raylib.EndDrawing();
                    continue;
                }

                Raylib.ClearBackground(Color.White);
                Raylib.BeginDrawing();
                //drawing confines;
                int timeMeasureType1 = Raylib.MeasureText($"0{(int)timeAliveMinutes}:0{(int)timeAliveSeconds}", 16);
                int timeMeasureType2 = Raylib.MeasureText($"{(int)timeAliveMinutes}:0{(int)timeAliveSeconds}", 16);
                int timeMeasureType3 = Raylib.MeasureText($"0{(int)timeAliveMinutes}:{(int)timeAliveSeconds}", 16);
                int timeMeasureType4 = Raylib.MeasureText($"{(int)timeAliveMinutes}:{(int)timeAliveSeconds}", 16);
                if (countTime)
                    timeAliveSeconds += deltaTime;
                else
                {
                    if (timeAliveSeconds < 10)
                    {
                        if (timeAliveMinutes < 10)
                            Raylib.DrawText($"0{(int)timeAliveMinutes}:0{(int)timeAliveSeconds}", Config.WindowSizeWidth / 2 - timeMeasureType1/2, 20, 24, Color.Red);
                        else
                            Raylib.DrawText($"{(int)timeAliveMinutes}:0{(int)timeAliveSeconds}", Config.WindowSizeWidth / 2 - timeMeasureType2/2, 20, 24, Color.Red);
                    }
                    else
                    {
                        if (timeAliveMinutes < 10)
                            Raylib.DrawText($"0{(int)timeAliveMinutes}:{(int)timeAliveSeconds}", Config.WindowSizeWidth / 2 - timeMeasureType3/2, 20, 24, Color.Red);
                        else
                            Raylib.DrawText($"{(int)timeAliveMinutes}:{(int)timeAliveSeconds}", Config.WindowSizeWidth / 2 - timeMeasureType4/2, 20, 24, Color.Red);
                    }
                }

                if (countTime)
                {
                    if (timeAliveSeconds < 10)
                    {
                        if (timeAliveMinutes < 10)
                            Raylib.DrawText($"0{(int)timeAliveMinutes}:0{(int)timeAliveSeconds}", Config.WindowSizeWidth / 2 - timeMeasureType1/2, 20, 24, Color.Black);
                        else
                            Raylib.DrawText($"{(int)timeAliveMinutes}:0{(int)timeAliveSeconds}", Config.WindowSizeWidth / 2 - timeMeasureType2/2, 20, 24, Color.Black);
                    }
                    else
                    {
                        if (timeAliveMinutes < 10)
                            Raylib.DrawText($"0{(int)timeAliveMinutes}:{(int)timeAliveSeconds}", Config.WindowSizeWidth / 2 - timeMeasureType3/2, 20, 24, Color.Black);
                        else
                            Raylib.DrawText($"{(int)timeAliveMinutes}:{(int)timeAliveSeconds}", Config.WindowSizeWidth / 2 - timeMeasureType4/2, 20, 24, Color.Black);

                    }
                }
                //Timer ^^^, mest av koden är centrering

                //resattas till 0 så vi får minuter
                if (timeAliveSeconds > 60)
                {
                    timeAliveSeconds = 0; 
                    timeAliveMinutes += 1;
                }

                //när alla enemies är döda, ny wave och +1 wave count
                if (enemies.Count <= 0 && enemieSpawnCount <= 0 && firstWaveAfterRestart != true)
                {
                    bullets.Clear();
                    countTime = false;
                    if (!noMoreTemps)
                        Config.tempMovementSpeedHolder = Config.movementSpeed;
                    noMoreTemps = true;
                    Config.movementSpeed = 300; //temporärt gör movementspeed högre, simple qol
                    powerUps.draw(deltaTime);
                    powerUps.update();

                    if (Config.isPicked)
                        waveCooldown -= deltaTime;

                    //Ser lite dumt ut att köra om dessa varje frame men det fungerar inte att ha alla measureText i början utanför game lopp
                    int waveTextWidth = Raylib.MeasureText("Next wave in: 5", 40);
                    int waveTextWidthWait = Raylib.MeasureText("Next wave after power-up has been chosen", 40);
                    int PickUpInstruction = Raylib.MeasureText("Power-up is picked up by walking inside the circle and pressing E", 18);

                    if (!Config.isPicked)
                    {
                        Raylib.DrawText($"Next wave after power-up has been chosen", Config.WindowSizeWidth / 2 - waveTextWidthWait / 2, Config.WindowSizeHeight / 8, 40, Color.DarkPurple);
                        Color Instruction = Raylib.ColorAlpha(Color.DarkGray, 0.5f);
                        Raylib.DrawText($"Power-up is picked up by walking inside the circle and pressig E", Config.WindowSizeWidth / 2 - PickUpInstruction / 2, Config.WindowSizeHeight / 2 + (Config.WindowSizeHeight / 4), 18, Instruction);
                    }
                    else
                    {
                        Raylib.DrawText($"Next wave in: {(int)waveCooldown}", Config.WindowSizeWidth/2 - waveTextWidth/2, Config.WindowSizeHeight/8,40,Color.DarkPurple);
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
                        Config.hasRolledThisRound = false;
                        Config.despawnTime = 0.5f;
                        Config.movementSpeed = Config.tempMovementSpeedHolder; //tillbaka till segis
                        noMoreTemps = false;
                    }
                }

                firstWaveAfterRestart = false;

                if (enemieSpawnCount > 0)
                {
                    countTime = true;
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

                if (enemieSpawnCount == 0)
                    Config.hasBossSpawned = false;

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
                    if (Config.bulletCooldown <= bulletCooldownTimer)
                    {
                        bulletCooldownTimer = 0;
                        BaseAbility bullet = new BaseAbility(closestEnemy);
                        drawableList.Add(bullet);
                        bullets.Add(bullet);
                    }

                    enemyBulletCooldownTimer += deltaTime;
                    if (Config.enemyBulletCooldown <= enemyBulletCooldownTimer)
                    {
                        foreach (var enemy in  enemies)
                        {
                            if (enemy.shouldShoot)
                            {
                                enemyBulletCooldownTimer = 0;
                                EnemyBullets enemyBullets = new EnemyBullets(enemy);
                                drawableList.Add(enemyBullets);
                                enemyBullet.Add(enemyBullets);
                            }
                        }
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
                        if (item is BaseAbility bullet)
                        {
                            bullets.Remove(bullet);
                        }
                        drawableList.RemoveAt(i);
                        continue;
                    }
                    
                    collisionCooldownTimer += deltaTime;
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
                            if (collisionCooldownTimer >= collisionCooldown)
                            {
                                collisionCooldownTimer = 0f;
                                npc.playerCollision(enemy, deltaTime);
                            }
                        }
                        foreach (var enemyBullets in enemyBullet)
                        {
                            enemyBullets.shouldDespawn();
                        }
                    }
                    item.draw();
                }
                
                Raylib.DrawText($"Kill count: {killCount}", 0,0,28, Color.Red);
                Raylib.DrawText($"Wave: {Config.wave}", 0,25,28, Color.Red);
                Raylib.DrawText($"bullet damage stat: {Config.bulletDamage}",0,Config.WindowSizeHeight-20,12, Color.DarkGray);
                Raylib.DrawText($"bullet radius stat: {Config.bulletRadius}",0,Config.WindowSizeHeight-30,12, Color.DarkGray);
                Raylib.DrawText($"bullet speed stat: {Config.bulletSpeed}",0,Config.WindowSizeHeight-40,12, Color.DarkGray);
                Raylib.DrawText($"bullet cooldown stat: {Config.bulletCooldown}",0,Config.WindowSizeHeight-50,12, Color.DarkGray);
                Raylib.DrawText($"player radius stat: {Config.playerRadius}",0,Config.WindowSizeHeight-60,12, Color.DarkGray);
                Raylib.DrawText($"player hitpoints stat: {Config.playerHealthpoints}",0,Config.WindowSizeHeight-70,12, Color.DarkGray);
                Raylib.DrawText($"player movement speed stat: {Config.movementSpeed}",0,Config.WindowSizeHeight-80,12, Color.DarkGray);
                Raylib.DrawText($"stat sheet:",0,Config.WindowSizeHeight-90,12, Color.DarkGray);
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
        public static int enemyBulletDamage = 5;
        public static int enemyBulletRadius = 5;
        public static float enemyBulletSpeed = 300f;
        public static float enemyBulletCooldown = 1.5f;
        public static bool hasBossSpawned = false;

        //player
        public static int movementSpeed = 100;
        public static int playerRadius = 15;
        public static int playerHealthpoints = 100;
        public static int tempMovementSpeedHolder;

        //bullets
        public static int bulletRadius = 5;
        public static int bulletDamage = 50;
        public static float bulletSpeed = 300f;
        public static float bulletCooldown = 1.5f;

        //power-ups
        public static bool isPicked = false;
        public static bool p1 = false;
        public static bool p2 = false;
        public static bool p3 = false;
        public static bool hasRolledThisRound = false;
        public static float despawnTime = 0.5f;

        //wave
        public static int wave = 1;

        //global för alla, det får bara finnas en instans
        public static Player player = new Player(Config.WindowSizeWidth / 2, Config.WindowSizeHeight / 2);
    }
}
