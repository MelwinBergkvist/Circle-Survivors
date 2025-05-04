using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CircleSurvivors
{
    public class NPC : Drawable
    {
        public float x;
        public float y;
        public float spawnImmunity = 0.5f;
        public float sinceSpawn = 0;
        public int enemyCollisionDamage = Config.enemyCollisionDamage;
        float movementSpeed = 80;
        float hitCooldown = 0f;
        float radius = Config.npcRadius;
        int hitpoints = 100;
        int maxHitpoints;
        public bool shouldShoot = false;
        bool isBoss = false;
        bool isBossTurn = false;
        string bossName;
        string[] bossNames =
        {
            //Tanky - 0,5
            "The Verdant Juggernaut, Bramblethrone", "Chlorobane, Devourer of Damage", "Photosynthesaur, King of Endurance", "Lord Bulkwood, Unmovable Wall of Doom", "The Great Moss Engine",
            
            //Speedy - 5,10
            "Blurmageddon, Scourge of Speed", "Sonic Calamity, Vortexus Prime", "Skittershade, Swift End of Days", "Purple Menace, Prince of Panic", "Wraithflicker, Dancer of Death",

            //Shooter - 10,15
            "Gunsaint Oblivion, Apostle of Annihilation", "Project Omega: The White Eclipse", "Triggergrin, The Last Whisper", "Obsidian Herald of Lead", "The Hollow Sniper, Voidspike",

            //default - 15, 19
            "Abyssion the Forgotten", "Malgrath, Uncrowned Tyrant", "Vorharn, Bane of Balance", "Azgalor the Timeless Hunger", "The Crownless Dread"
        }; //namnen är från chatgpt
        Color enemyColor = Color.Red;
        Color enemyHealthColor = Color.Orange;

        public NPC() //constructor
        {
            Random random = new Random();
            if (Config.wave % 1 == 0)
            {
                isBossTurn = true;
            }
            //Boss
            if (!Config.hasBossSpawned && isBossTurn)
            {
                hitpoints += 500;
                radius += 50;
                movementSpeed -= 40;
                enemyCollisionDamage += 25;
                isBoss = true;
                Config.hasBossSpawned = true;
            }

            //Special enemies
            if (random.Next(0,101) > 1 && ((Config.hasBossSpawned && isBossTurn) || (!Config.hasBossSpawned && !isBossTurn))) // 9%
            {
                hitpoints += 100;
                radius += 5f;
                movementSpeed -= 30;
                enemyColor = Color.DarkGreen;
                enemyHealthColor = Color.Red;
                enemyCollisionDamage += 10;
                if (isBoss)
                {
                    bossName = bossNames[random.Next(0,5)];
                }
            }
            else if (random.Next(0,101) > 80 && ((Config.hasBossSpawned && isBossTurn) || (!Config.hasBossSpawned && !isBossTurn))) // 18%
            {
                hitpoints -= 50;
                radius -= 5;
                movementSpeed += 60;
                enemyColor = Color.Purple;
                enemyHealthColor = Color.Magenta;
                enemyCollisionDamage -= 4;
                if (isBoss)
                {
                    bossName = bossNames[random.Next(5, 10)];
                }
            }
            else if (random.Next(0,101) > 90 && ((Config.hasBossSpawned && isBossTurn) || (!Config.hasBossSpawned && !isBossTurn))) // 7.2%
            {
                enemyColor = Color.Black;
                enemyHealthColor = Color.White;
                shouldShoot = true;
                if (isBoss)
                {
                    bossName = bossNames[random.Next(10, 15)];
                }
            }

            if (string.IsNullOrEmpty(bossName) && Config.hasBossSpawned)
            {
                bossName = bossNames[random.Next(15, 19)];
            }

            maxHitpoints = hitpoints;
            int side = random.Next(1, 5); //1 till 5 för att få mellan 1 och 4, random shenanegins
            // 1 = vänster, 2 = höger, 3 = up, 4 = nere
            if (side == 1)
            {
                this.x = -radius;
                this.y = random.Next(0, Config.WindowSizeHeight);
            }
            else if (side == 2)
            {
                this.x = Config.WindowSizeWidth + radius;
                this.y = random.Next(0, Config.WindowSizeHeight);
            }
            else if (side == 3)
            {
                this.y = -radius;
                this.x = random.Next(0, Config.WindowSizeWidth);
            }
            else
            {
                this.y = Config.WindowSizeHeight + radius;
                this.x = random.Next(0, Config.WindowSizeWidth);
            }
        }
        public void draw()
        {
            Raylib.DrawCircle((int)x, (int)y, radius, enemyColor); //tar x & y som ints istället för floats

            //2nd radius som en healthmeter typ
            float healthRadius = radius - (radius * hitpoints / maxHitpoints);
            Raylib.DrawCircle((int)x, (int)y, healthRadius, enemyHealthColor);

            if (isBoss)
            {
                int measureBossName = Raylib.MeasureText(bossName, 16);
                Raylib.DrawText($"{bossName}", (int)x - measureBossName / 2, (int)y+100, 16, enemyColor);
            }
        }

        public bool shouldDespawn()
        {
            return hitpoints <= 0;
        }

        public void update(float deltaTime)
        {
            sinceSpawn += deltaTime;

            //Så de går mot spelaren
            float dx = Config.player.x - x;
            float dy = Config.player.y - y;
            float distance = MathF.Sqrt(dx * dx + dy * dy);

            if (!shouldShoot)
            {
                if (distance > 0)
                {
                    float moveX = (dx / distance) * movementSpeed * deltaTime;
                    float moveY = (dy / distance) * movementSpeed * deltaTime;

                    float newX = x + moveX;
                    float newY = y + moveY;

                    x = newX;
                    y = newY;
                }
            }
            else if (shouldShoot) //kan bara ha "else" här, behövs ingen if (shouldShoot) men det är mest bara ifall jag lägger till något mer som har liknande condition
            {
                if (distance > 400) //numret 400 var mest trail and error, inget specielt.
                {
                    float moveX = (dx / distance) * movementSpeed * deltaTime;
                    float moveY = (dy / distance) * movementSpeed * deltaTime;

                    float newX = x + moveX;
                    float newY = y + moveY;

                    x = newX;
                    y = newY;
                }
            }

            if (hitCooldown >= 0)
                hitCooldown -= deltaTime;
        }
        public void bulletCollision(BaseAbility bullets, float deltaTime)
        {
            if (sinceSpawn < spawnImmunity) return;
            
            //Kollar om bullet och enemies overlappar
            float bulletEnemyDx = bullets.bulletX - x;
            float bulletEnemyDy = bullets.bulletY - y;
            float distanceBulletEnemy = bulletEnemyDx * bulletEnemyDx + bulletEnemyDy * bulletEnemyDy;
            float radiusSum = Config.bulletRadius + radius;
            if (distanceBulletEnemy <= radiusSum * radiusSum)
            {
                if (hitCooldown <= 0)
                {
                    hitpoints -= Config.bulletDamage;
                    hitCooldown = 0.5f;
                }
            }
        }
        public void playerCollision(NPC enemies, float deltaTime)
        {
            //Hit collision för spelare, basically en kopia av den som finns för bullets i NPC.cs, matte vis
            float playerEnemyDx = Config.player.x - enemies.x;
            float playerEnemyDy = Config.player.y - enemies.y;
            float distancePlayerEnemy = playerEnemyDx * playerEnemyDx + playerEnemyDy * playerEnemyDy;
            float radiusSum = Config.playerRadius + enemies.radius;

            if (distancePlayerEnemy <= radiusSum * radiusSum)
                Config.playerHealthpoints -= enemyCollisionDamage;
        }
    }
}
