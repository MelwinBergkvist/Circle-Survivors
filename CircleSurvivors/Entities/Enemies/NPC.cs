using CircleSurvivors.Entities.Player;
using CircleSurvivors.Interfaces;
using CircleSurvivors.UI_Helpers;
using CircleSurvivors.Core;
using System.Numerics;
using Raylib_cs;

namespace CircleSurvivors.Entities.Enemies
{
    /// <summary>
    /// hanterar alla NPC
    /// </summary>
    public class NPC : IDrawable //implementerar drawable interface
    {
        //positioner
        public float x, y;
        Vector2 velocity = new Vector2(0, 0);
        float movementSpeed;
        float turnSpeed = 10f;
        float friction = 3f;
        float distance;

        //states
        public bool shouldShoot = false;
        bool isSpinner = false;
        public bool isBoss = false;
        readonly Random random = new Random();
        string bossName;
        bool extendRadius = true;

        //stats
        int hitpoints;
        int maxHitpoints; //behövs för damage radius
        public int enemyCollisionDamage;
        public float radius;
        int spinnerRadius = 10;

        //timers
        float hitCooldown;
        float enemyBulletCooldownTimer = 0f;
        readonly float collisionCooldown = 1.5f;
        float collisionCooldownTimer = 0f;
        public float spawnImmunity = 0.5f;
        public float sinceSpawn = 0;
        float aimerCooldown = 0.5f;

        double angle = 0;
        double radiusAimer = 40;

        //scaling
        readonly int scaling = (Config.wave-1) * 10;

        //Alla namn är tagna från en Dark Fantasy boss name generator jag hittade på google
        readonly string[] normalNames = 
        {
            "Malithrax the Crimson Maw",
            "The Scarlet Abyssal Ooze",
            "Vermithor, Harbinger of Blood",
            "Zyrgath, the Sanguine Horror",
            "The Red Maw of Despair",
            "The Ruby Blight",
            "Eldrathis, Lord of the Scarlet Sludge",
            "The Blistering Bloodspawn",
            "Khorvax, the Crimson Corruptor",
            "The Fiery Hemogoblin"
        };

        readonly string[] tankyNames =
        {
            "Grisshath, the Mirebound Juggernaut",
            "Vrothmuk, the Sludge Tyrant",
            "Zalgorath, the Blighted Ooze",
            "Malgroth, the Putrid Colossus",
            "Grothul, the Verdant Behemoth",
            "Ghulmire, the Viridian Rot",
            "The Emerald Plagueheart",
            "Morbgluth, the Bilebound Leviathan",
            "The Verdant Sludge Reborn",
            "Ograth'Zul, Lord of the Putrid Mire"
        };

        readonly string[] speedyNames = 
        {
            "Violeth Maw, the Swift Abyss",
            "Nyx'lor, the Violet Surge",
            "The Purple Maw of Sorrow",
            "Zhar'kul, the Blight Blob",
            "The Amethyst Wraith",
            "The forgotten blitzer",
            "Caragon, the uncrowned Tyrant",
            "The Indigo Wraithspawn",
            "Nyxithra, the Amethyst Phantasm",
            "Vilethrix, the Violet Reaper"
        };

        readonly string[] shooterNames = 
        {
            "Doomgrip, the Blackened Torrent",
            "Oblivion Maw, the Leaden Fury",
            "Xyrrath, the Bullet-Soaked Horror",
            "The Iron Veil of Destruction",
            "Grimrath, the Shattered Singularity",
            "Cinderbane, the Brass-Maw Tyrant",
            "Necroshard, the Blackened Barrage",
            "Vraxion, the Steel-Spitting Wraith",
            "The Darkgun Behemoth",
            "Kalthrax, the Lead-Wreathed Terror"
        };

        Color enemyColor;
        Color enemyHealthColor;

        /// <summary>
        /// spawnar en boss först och sen enemies som resten
        /// </summary>
        public NPC()
        {
            if (Config.shouldBossSpawn)
                CreateBoss();
            else
                CreateEnemy();
        }

        /// <summary>
        /// ritar enemies
        /// </summary>
        public void Draw()
        {
            //ritar enemies och displayar namn om den har det
            Raylib.DrawCircle((int)x, (int)y, radius, enemyColor);
            DisplayBossName();

            //2nd radius som en healthmeter typ
            float healthRadius = radius - radius * hitpoints / maxHitpoints;
            Raylib.DrawCircle((int)x, (int)y, healthRadius, enemyHealthColor);
        }

        /// <summary>
        /// Despawn mechanics för enemies, om hp < 0, despawna
        /// </summary>
        /// <returns></returns>
        public bool ShouldDespawn()
        {
            return hitpoints <= 0;
        }

        /// <summary>
        /// Hanterar enemies pathing och bullet skjutning för enemies som ska skjuta
        /// </summary>
        /// <param name="deltaTime">tid</param>
        public void Update(float deltaTime)
        {
            //spawn immunity
            sinceSpawn += deltaTime;

            //make sure att det faktist finns enemies på skärmen,
            //så vi inte försöker skjuta mot något som inte finns
            enemyBulletCooldownTimer += deltaTime;
            if (Config.enemiesList.Count > 0)
                ShootEnemyBullet(this);

            NpcMovements(deltaTime);

            foreach (var bullets in Config.bulletsList)
            {
                BulletCollision(bullets, deltaTime);
            }

            foreach (var enemies in Config.enemiesList)
            {
                PlayerCollision(enemies, deltaTime);
            }

            Spinner(deltaTime);

            //cooldowns
            if (hitCooldown >= 0) hitCooldown -= deltaTime;
            if (collisionCooldown >= collisionCooldownTimer) collisionCooldownTimer += deltaTime;
        }
        /// <summary>
        /// Kollar om spelarens bullet collidar med enemy
        /// </summary>
        /// <param name="bullets">spelarens bullet</param>
        /// <param name="deltaTime">tid</param>
        public void BulletCollision(BaseAbility bullets, float deltaTime)
        {
            if (sinceSpawn < spawnImmunity) return;

            float radiusSum = Config.bulletRadius + radius;
            float distanceBulletEnemy = Helper.EuclideanFloat(ref x, ref bullets.bulletX, ref y, ref bullets.bulletY).distance;

            if (distanceBulletEnemy <= radiusSum) //kollar om distansen mellan bullet och enemy är mindre en deras radius tillsammans
            {
                if (hitCooldown <= 0)
                {
                    hitpoints -= Config.bulletDamage;
                    hitCooldown = 0.5f;
                }
            }
        }
        /// <summary>
        /// kollar collision mellan spelare och enemies, bara physical contact.
        /// </summary>
        /// <param name="enemies">enemies</param>
        /// <param name="deltaTime">tid</param>
        public void PlayerCollision(NPC enemies, float deltaTime)
        {
            if (collisionCooldown <= collisionCooldownTimer) 
                collisionCooldownTimer = 0; //om cooldown är över resetta
            else if (collisionCooldown >= collisionCooldownTimer) 
                return; //om inte, skippa
            
            float radiusSum = Config.player.radius + enemies.radius;
            float distancePlayerEnemy = Helper.EuclideanFloat(ref Config.player.x, ref enemies.x, ref Config.player.y, ref enemies.y).distance;

            if (distancePlayerEnemy <= radiusSum) //kollar om distanse mellan spelare och enemy är mindre än deras radius tillsammans
                Config.player.healthpoints -= enemyCollisionDamage;
        }
        /// <summary>
        /// hanterar om enemies ska skjuta, om det gör så spawnas en bullet och läggs i drawableList och enemyBullet
        /// </summary>
        /// <param name="enemy">enemies(detta relaterar bara det som ska skjuta)</param>
        public void ShootEnemyBullet(NPC enemy)
        {
            //exact samma logik som bullet cooldownen innan men för enemies
            //här har vi mer än en enemy so vi loopar igen hela enemies listan
            //så alla enemies individiuelt har sin egen cooldown
            if (Config.enemyBulletCooldown <= enemyBulletCooldownTimer)
            {
                if (shouldShoot)
                {
                    if (isBoss)
                        enemyBulletCooldownTimer = 1.2f; //basically reducar cooldown till 0.3 för bossar
                    else
                        enemyBulletCooldownTimer = 0;
                    EnemyBullets enemyBullets = new EnemyBullets(enemy);
                    Config.drawableList.Add(enemyBullets);
                    Config.enemyBulletList.Add(enemyBullets);
                }
            }
        }
        /// <summary>
        /// skapar en boss
        /// </summary>
        public void CreateBoss()
        {
            int bossType = random.Next(0, 4);
            switch (bossType)
            {
                case 0: //Normal
                    hitpoints = 500 + scaling;
                    maxHitpoints = hitpoints;
                    radius = 50;
                    movementSpeed = 40;
                    enemyCollisionDamage = 30;
                    isSpinner = true;
                    enemyColor = new(156, 6, 6);
                    enemyHealthColor = new(77, 8, 8);
                    bossName = normalNames[random.Next(0,10)];
                    break;
                case 1: //Tanky
                    hitpoints = 750 + scaling;
                    maxHitpoints = 750;
                    radius = 55;
                    movementSpeed = 30;
                    enemyCollisionDamage = 45;
                    enemyColor = new(19, 138, 11);
                    enemyHealthColor = new(8, 77, 3);
                    bossName = tankyNames[random.Next(0,10)];
                    break;
                case 2: //Speedy
                    hitpoints = 250;
                    maxHitpoints = hitpoints;
                    radius = 35;
                    movementSpeed = 70;
                    enemyCollisionDamage = 20;
                    enemyColor = new(120, 6, 191);
                    enemyHealthColor = new(85, 5, 135);
                    bossName = speedyNames[random.Next(0,10)];
                    break;
                case 3: //Shooter
                    hitpoints = 400 + scaling;
                    maxHitpoints = hitpoints;
                    radius = 50;
                    movementSpeed = 40;
                    shouldShoot = true;
                    enemyCollisionDamage = 30;
                    enemyBulletCooldownTimer = 1.2f;
                    enemyColor = new(0, 0, 0);
                    enemyHealthColor = new(65, 65, 65);
                    bossName = shooterNames[random.Next(0,10)];
                    break;
            }
            isBoss = true;
            SpawnPosition(); //sätter vart enemy/boss ska spawna
        }
        /// <summary>
        /// Skapar en enemy
        /// </summary>
        public void CreateEnemy()
        {
            //Special enemies
            int enemyType = random.Next(0,4);
            switch(enemyType)
            {
                case 0: //Default
                    hitpoints = 100 + scaling;
                    maxHitpoints = hitpoints;
                    radius = 10;
                    movementSpeed = 80;
                    enemyCollisionDamage = 5;
                    enemyColor = new(156, 6, 6);
                    enemyHealthColor = new(77, 8, 8);
                    break;
                case 1: //Tanky
                    hitpoints = 200 + scaling;
                    maxHitpoints = hitpoints;
                    radius = 20;
                    movementSpeed = 50;
                    enemyCollisionDamage = 15;
                    enemyColor = new(19, 138, 11);
                    enemyHealthColor = new(8, 77, 3);
                    break;
                case 2: //Speedy
                    hitpoints = 20 + scaling/2;
                    maxHitpoints = hitpoints;
                    radius = 5;
                    movementSpeed = 140;
                    enemyCollisionDamage = 1;
                    enemyColor = new(120, 6, 191);
                    enemyHealthColor = new(85, 5, 135);
                    break;
                case 3: //Shooter
                    hitpoints = 100 + scaling;
                    maxHitpoints = hitpoints;
                    radius = 10;
                    movementSpeed = 80;
                    enemyCollisionDamage = 5;
                    shouldShoot = true;
                    enemyColor = new(0, 0, 0);
                    enemyHealthColor = new(65, 65, 65);
                    break;
            }
            SpawnPosition(); //sätter vart enemy/boss ska spawna
        }
        /// <summary>
        /// Väljer vart enemy/boss ska spawnas
        /// </summary>
        public void SpawnPosition()
        {
            int side = random.Next(0, 4);
            switch (side)
            {
                case 0:
                    //vänster
                    x = -radius;
                    y = random.Next(0, Config.WindowSizeHeight);
                    break;
                case 1:
                    //höger
                    x = Config.WindowSizeWidth + radius;
                    y = random.Next(0, Config.WindowSizeHeight);
                    break;
                case 2:
                    //up
                    y = -radius;
                    x = random.Next(0, Config.WindowSizeWidth);
                    break;
                case 3:
                    //nere
                    y = Config.WindowSizeHeight + radius;
                    x = random.Next(0, Config.WindowSizeWidth);
                    break;
            }
        }
        /// <summary>
        /// enemy movements updatering
        /// </summary>
        /// <param name="deltaTime"></param>
        public void NpcMovements(float deltaTime)
        {

            /*
             * Matten här är lite svårförklarad för jag fick mycket hjälp med själva matten
             * och Raylib har många inbyggda metoder för att räkna ut matten själv
             * 
             * vi kollar för euclidean distance mellan spelaren, sen skapar vi en Vector2 (2 för 2d space) //steg 1
             * (gör det nu med min helper class jag har skrivit)
             * 
             * sen så använder vi vector2 scale för att få slut direction att scalea till movement speed // steg 2
             * 
             * sen så använder vi vector 2 linear interpolation vilket använder 3 args (a, b, c) där c blendar mellan a och b // steg 3
             * 
             * sen är else samma som steg 3 fast till 0 för att stanna, och använder friction
            */

            distance = Helper.EuclideanFloat(ref x, ref Config.player.x, ref y, ref Config.player.y).distance;

            if (!shouldShoot)
            {
                if (distance > 0)
                {
                    Vector2 direction = Helper.EuclideanVector2(ref x, ref Config.player.x, ref y, ref Config.player.y); //steg 1
                    Vector2 endVelocity = Raymath.Vector2Scale(direction, movementSpeed); //steg 2
                    
                    velocity = Raymath.Vector2Lerp(velocity, endVelocity, turnSpeed * deltaTime); //steg 3
                }
                else
                {
                    velocity = Raymath.Vector2Lerp(velocity, new Vector2(0, 0), friction * deltaTime);
                }
            }
            else if (shouldShoot) //kan bara ha "else" här, behövs ingen if (shouldShoot) men det är mest bara ifall jag lägger till något mer som har liknande condition
            {
                if (distance > 400) //numret 400 var mest trail and error, inget specielt.
                {
                    Vector2 direction = Helper.EuclideanVector2(ref x, ref Config.player.x, ref y, ref Config.player.y);
                    Vector2 endVelocity = Raymath.Vector2Scale(direction, movementSpeed);

                    velocity = Raymath.Vector2Lerp(velocity, endVelocity, turnSpeed * deltaTime);
                }
                else
                {
                    velocity = Raymath.Vector2Lerp(velocity, new Vector2(0, 0), turnSpeed * deltaTime);
                }
            }
            x += velocity.X * deltaTime;
            y += velocity.Y * deltaTime;
        }
        /// <summary>
        /// ritar boss namnet
        /// </summary>
        public void DisplayBossName()
        {
            if (isBoss)
            {
                Helper.DrawCenteredText($"{bossName}", (int)x, (int)y + (int)radius + 10, 14, enemyColor);
            }
        }
        /// <summary>
        /// metoden som handler spinning balls runt default boss enemy
        /// </summary>
        /// <param name="deltaTime">tid</param>
        public void Spinner(float deltaTime)
        {
            // WIP - har inte gjort collision än
            if (isSpinner)
            {
                double pointerX = x + (radius + spinnerRadius) * Math.Cos(angle);
                double pointerY = y + (radius + spinnerRadius) * Math.Sin(angle);

                if (aimerCooldown > 0)
                {
                    aimerCooldown -= deltaTime;
                }
                if (aimerCooldown < 0)
                {
                    aimerCooldown = 0.01f;
                    angle += 0.05f;

                    if (spinnerRadius <= 10)
                        extendRadius = true;
                    else if (spinnerRadius >= 70)
                        extendRadius = false;

                    if (extendRadius)
                        spinnerRadius += 3;
                    else if (!extendRadius)
                        spinnerRadius -= 3;
                }

                Raylib.DrawCircle((int)pointerX, (int)pointerY, 10, Color.Red);
            }
        }
    }
}
