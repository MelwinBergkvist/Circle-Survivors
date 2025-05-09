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
        public float x, y;
        public float spawnImmunity = 0.5f;
        public float sinceSpawn = 0;
        float movementSpeed = 80;
        float hitCooldown = 0f;
        float radius = Config.npcRadius;

        int hitpoints = 100;
        int maxHitpoints;
        public int enemyCollisionDamage = Config.enemyCollisionDamage;

        public bool shouldShoot = false;

        //custom Color (Röd, Grön, Blå, Alpha)
        Color enemyColor = new Color(156, 6, 6);
        Color enemyHealthColor = new Color(77, 8, 8);

        public NPC() //constructor
        {
            Random random = new Random();
            
            maxHitpoints = hitpoints;
            int side = random.Next(1, 5); //1 till 5 för att få mellan 1 och 4, random shenanegins
            if (side == 1)
            {
                //vänster
                x = -radius;
                y = random.Next(0, Config.WindowSizeHeight);
            }
            else if (side == 2)
            {
                //höger
                x = Config.WindowSizeWidth + radius;
                y = random.Next(0, Config.WindowSizeHeight);
            }
            else if (side == 3)
            {
                //up
                y = -radius;
                x = random.Next(0, Config.WindowSizeWidth);
            }
            else
            {
                //ner
                y = Config.WindowSizeHeight + radius;
                x = random.Next(0, Config.WindowSizeWidth);
            }
        }
        public void draw()
        {
            Raylib.DrawCircle((int)x, (int)y, radius, enemyColor); //tar x & y som ints istället för floats

            //2nd radius som en healthmeter typ
            float healthRadius = radius - (radius * hitpoints / maxHitpoints);
            Raylib.DrawCircle((int)x, (int)y, healthRadius, enemyHealthColor);
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
