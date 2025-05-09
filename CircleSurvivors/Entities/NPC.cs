﻿using CircleSurvivors.Mechanics;
using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CircleSurvivors.Entities
{
    public class NPC : Drawable
    {
        public float x, y;
        public float spawnImmunity = 0.5f;
        public float sinceSpawn = 0;
        float movementSpeed = 80;
        float hitCooldown = 0f;
        public float radius = 10;
        float enemyBulletCooldownTimer = 0f;

        int hitpoints = 100;
        int maxHitpoints;
        public int enemyCollisionDamage = Config.enemyCollisionDamage;

        public bool shouldShoot = false;

        Color enemyColor = new Color(156, 6, 6);
        Color enemyHealthColor = new Color(77, 8, 8);

        /// <summary>
        /// initializerar enemiesarna och gör dem till special enemies om random rollen är success
        /// </summary>
        public NPC() //constructor
        {
            Random random = new Random();

            //Special enemies
            if (random.Next(0, 101) > 90) // 10%
            {
                // Tanky
                hitpoints += 100;
                radius += 5f;
                movementSpeed -= 30;
                enemyColor = new Color(19, 138, 11);
                enemyHealthColor = new Color(8, 77, 3);
                enemyCollisionDamage += 10;
            }
            else if (random.Next(0, 101) > 80) // 18%
            {
                // Speedy                
                hitpoints -= 50;
                radius -= 5;
                movementSpeed += 60;
                enemyColor = new Color(120, 6, 191);
                enemyHealthColor = new Color(85, 5, 135);
                enemyCollisionDamage -= 4;
            }
            else if (random.Next(0, 101) > 90) // 7.2%
            {
                //Shooter
                enemyColor = new Color(0, 0, 0);
                enemyHealthColor = new Color(65, 65, 65);
                shouldShoot = true;
            }

            maxHitpoints = hitpoints;
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
        /// ritar enemies
        /// </summary>
        public void Draw()
        {
            Raylib.DrawCircle((int)x, (int)y, radius, enemyColor);

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
            sinceSpawn += deltaTime;

            //Så de går mot spelaren
            float dx = Config.player.x - x;
            float dy = Config.player.y - y;
            float distance = MathF.Sqrt(dx * dx + dy * dy);

            //make sure att det faktist finns enemies på skärmen,
            //så vi inte försöker skjuta mot något som inte finns
            enemyBulletCooldownTimer += deltaTime;
            if (Config.enemies.Count > 0)
            {
                if (Config.enemyBulletCooldown <= enemyBulletCooldownTimer)
                {
                    ShootEnemyBullet(this);
                }
            }

            if (!shouldShoot)
            {
                if (distance > 0)
                {
                    float moveX = dx / distance * movementSpeed * deltaTime;
                    float moveY = dy / distance * movementSpeed * deltaTime;

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
                    float moveX = dx / distance * movementSpeed * deltaTime;
                    float moveY = dy / distance * movementSpeed * deltaTime;

                    float newX = x + moveX;
                    float newY = y + moveY;

                    x = newX;
                    y = newY;
                }
            }


            if (hitCooldown >= 0)
                hitCooldown -= deltaTime;
        }
        /// <summary>
        /// Kollar om spelarens bullet collidar med enemy
        /// </summary>
        /// <param name="bullets">spelarens bullet</param>
        /// <param name="deltaTime">tid</param>
        public void BulletCollision(BaseAbility bullets, float deltaTime)
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
        /// <summary>
        /// kollar collision mellan spelare och enemies, bara physical contact.
        /// </summary>
        /// <param name="enemies">enemies</param>
        /// <param name="deltaTime">tid</param>
        public void PlayerCollision(NPC enemies, float deltaTime)
        {
            //Hit collision för spelare, basically en kopia av den som finns för bullets i NPC.cs, matte vis
            float playerEnemyDx = Config.player.x - enemies.x;
            float playerEnemyDy = Config.player.y - enemies.y;
            float distancePlayerEnemy = playerEnemyDx * playerEnemyDx + playerEnemyDy * playerEnemyDy;
            float radiusSum = Config.player.radius + enemies.radius;

            if (distancePlayerEnemy <= radiusSum * radiusSum)
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
            if (shouldShoot)
            {
                enemyBulletCooldownTimer = 0;
                EnemyBullets enemyBullets = new EnemyBullets(enemy);
                Config.drawableList.Add(enemyBullets);
                Config.enemyBullet.Add(enemyBullets);
            }
        }
    }
}
