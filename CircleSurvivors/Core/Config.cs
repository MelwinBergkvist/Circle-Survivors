using CircleSurvivors.Entities;
using CircleSurvivors.Interfaces;
using CircleSurvivors.Mechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircleSurvivors.Core
{
    /// <summary>
    /// Config klass, tar hand om datatyper som inte behöver vara lokala men behövs i många olika klasser
    /// </summary>
    public static class Config
    {
        //program
        public const int WindowSizeWidth = 1600;
        public const int WindowSizeHeight = 800;
        public static int killCount = 0;
        public static bool countTime = true;
        public static bool firstWaveAfterRestart = false;
        public static bool startScreen = false;
        public static float waveCooldown = 3.99f; //3.99 så den inte flashar en 4 vid början av cooldownen

        //GUI
        public static bool isPaused = false;
        public static bool isAlreadyPaused = false;
        public static float timeAliveSeconds = 0f;
        public static float timeAliveMinutes = 0f;


        //npc,enemy
        public static float enemyBulletSpeed = 300f;
        public static float enemyBulletCooldown = 1.5f;
        public static int enemyBulletDamage = 5;
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

        //Listor
        public static List<IDrawable> drawableList = new List<IDrawable>();  //En list som kan draw, update och kolla om något ska despawna, NPC's bullets osv.
        public static List<BaseAbility> bulletsList = new List<BaseAbility>();
        public static List<NPC> enemiesList = new List<NPC>();
        public static List<EnemyBullets> enemyBulletList = new List<EnemyBullets>();

        public static void ResetGameState()
        {
            wave = 1;
            bulletDamage = 50;
            bulletRadius = 5;
            bulletSpeed = 300f;
            player.healthpoints = 100;
            player.maxHealthpoints = 100;
            killCount = 0;
            enemiesList.Clear();
            drawableList.Clear();
            bulletsList.Clear();
            drawableList.Add(player);
            firstWaveAfterRestart = true;
            enemieSpawnCount = 10 + wave * wave;
            player.x = WindowSizeWidth / 2;
            player.y = WindowSizeHeight / 2;
            shouldBossSpawn = true;
        }
    }
}
