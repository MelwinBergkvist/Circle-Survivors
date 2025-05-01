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
        float movementSpeed = 80;
        float hitCooldown = 0f;
        float radius = Config.npcRadius;
        int hitpoints = 100;
        public bool shouldShoot = false;
        Color enemyColor = Color.Red;
        Color enemyHealthColor = Color.Orange;
        public NPC(Player player) //constructor
        {
            Random random = new Random();

            //Special enemies
            if (random.Next(0,100) > 90 )
            {
                hitpoints += 100;
                radius += 5f;
                movementSpeed -= 30;
                enemyColor = Color.DarkGreen;
                enemyHealthColor = Color.Red;
                Config.enemyCollisionDamage += 10;
            }
            else if (random.Next(0,101) > 80) 
            {
                hitpoints -= 50;
                radius -= 5;
                movementSpeed += 60;
                enemyColor = Color.Purple;
                enemyHealthColor = Color.Magenta;
                Config.enemyCollisionDamage -= 4;
            }
            else if (random.Next(0,101) > 90)
            {
                enemyColor = Color.Black;
                enemyHealthColor = Color.White;
                shouldShoot = true;
            }


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
            float healthRadius = radius - (radius * hitpoints / 100f);
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
            float radiusSum = Config.bulletRadius + Config.npcRadius;
            if (distanceBulletEnemy <= radiusSum * radiusSum)
            {
                if (hitCooldown <= 0)
                {
                    hitpoints -= Config.bulletDamage;
                    hitCooldown = 0.5f;
                }
            }
        }
    }
}
