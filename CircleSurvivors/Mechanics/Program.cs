﻿/* Notes:
 * Kommentarerna kommer vara lite svengelska
 * 
 * Allt som ska ritas as raylibs måste vara inom Begin och End drawing
 * 
 * alla raylib.measuretext måste deklareras där de används, jag kunde ha deklararet det som tex "int Text;" och sedan ändrat värdet men jag ansåg att det ser fult och klunky ut
 * och inte i början av main, jag vet inte varför men det fungerade
 * inte när jag testade det
 * 
 * Min standard:
 * Alla functioner Stor bokstav
 * Alla datatyper camelCase, första bokstaven liten
*/
using CircleSurvivors.Entities;
using CircleSurvivors.Graphics;
using Raylib_cs; //Initierar Raylibs library, måste göras på alla .cs
using System;
using System.Numerics;

namespace CircleSurvivors.Mechanics
{
    internal class Program
    {

        /// <summary>
        /// initialiserar spelet och kör allt som behövs för det
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            float deltaTime;
            Raylib.SetTargetFPS(60); //nästan som Thread.sleep(16); men bättre

            SplashTexts splashText = new SplashTexts();
            SpawnMechanics spawnMechs = new SpawnMechanics();
            ObjectHandler objectHandler = Config.objectHandler;
            GUI gui = new GUI();

            Config.ResetGameState();

            Raylib.InitWindow(Config.WindowSizeWidth, Config.WindowSizeHeight, "Circle Survivors - " + SplashTexts.GetSplashText());
            while (!Raylib.WindowShouldClose()) //Game loop
            {
                deltaTime = Raylib.GetFrameTime();

                if (Config.player.ShouldDespawn())
                {
                    gui.DeathScreen(deltaTime); //visar death screen
                    continue;
                }

                if (!Config.startScreen)
                {
                    gui.StartScreen(deltaTime); //visar start screen
                    continue;
                }

                Raylib.ClearBackground(Color.White);
                Raylib.BeginDrawing(); // <- drawing confines beginning

                spawnMechs.Spawn(deltaTime); //hanterar alla spawn mechanics
                objectHandler.CheckDespawnAndCollision(deltaTime); //hanterar all collision och despawn checks
                gui.StatSheet(); //displayar alla player stats
                gui.Timer(deltaTime); //displayar timern

                Raylib.EndDrawing(); // <- drawing confines ending
            }
        }
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
        public static float waveCooldown = 3.99f; //3.99 så den inte flashar en 4 vid början av cooldownen

        //npc,enemy
        public static int enemyCollisionDamage = 5;
        public static int enemyBulletDamage = 5;
        public static int enemyBulletRadius = 5;
        public static float enemyBulletSpeed = 300f;
        public static float enemyBulletCooldown = 1.5f;
        public static int enemieSpawnCount = 10 + wave * wave;
        
        //boss
        public static bool shouldBossSpawn = true;

        //player
        public static int tempMovementSpeedHolder;

        //bullets
        public static int bulletRadius = 5;
        public static int bulletDamage = 50;
        public static float bulletSpeed = 300f;
        public static float bulletCooldown = 1.5f;

        //wave
        public static int wave = 1;

        //global för alla, det får bara finnas en instans
        public static Player player = new Player(WindowSizeWidth / 2, WindowSizeHeight / 2);
        public static PowerUps powerUps = new PowerUps();
        public static ObjectHandler objectHandler = new ObjectHandler();
        public static Bosses bosses = new Bosses();
        public static List<Drawable> drawableList = new List<Drawable>();  //En list som kan draw, update och kolla om något ska despawna, NPC's bullets osv.
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
            player.x = WindowSizeWidth / 2;
            player.y = WindowSizeHeight / 2;
            shouldBossSpawn = true;
        }
    }
}
