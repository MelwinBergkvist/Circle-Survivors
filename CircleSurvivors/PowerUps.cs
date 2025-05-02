using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircleSurvivors
{
    public class PowerUps
    {
        //alla positioner som powerupsen ska ligga på
        int posX1 = Config.WindowSizeWidth / 2 - (Config.WindowSizeWidth / 6);
        int posY1 = Config.WindowSizeHeight / 4;

        int posX2 = Config.WindowSizeWidth / 2;
        int posY2 = Config.WindowSizeHeight / 3;

        int posX3 = Config.WindowSizeWidth / 2 + (Config.WindowSizeWidth / 6);
        int posY3 = Config.WindowSizeHeight / 4;

        int radius = 50;

        Random random = new Random();

        int powerUpStatus1;
        int powerUpStatus2;
        int powerUpStatus3;

        String[] powerUpsArray = 
        {
                "+5 Bullet Damage", "+2 Bullet Radius", "+10 Bullet Speed",
                "10% Faster Cooldown", "-1 Radius", "-10 Bullet Speed",
                "-10% Enemy Bullet Cooldown", "+20 Hitpoints", "+10 Movement speed" // 0 - 8
        };

        public PowerUps() 
        { 
            
        }
        public void update(Player player)
        {
            float radiusSum = radius + Config.playerRadius;

            if (!Config.hasRolledThisRound)
            {
                powerUpStatus1 = random.Next(0, 9);
                powerUpStatus2 = random.Next(0, 9);
                powerUpStatus3 = random.Next(0, 9);
            }
            Config.hasRolledThisRound = true;
            //gör all matta som kollar collision så vi vet vilken som tas,
            //efter någon har tagits så kör vi en return
            //i program.cs så resetar vi isPicked, och alla pX så vi får välja powerup igen efter
            if (!Config.p1)
            {
                float powerUpDx1 = posX1 - player.x;
                float powerUpDy1 = posY1 - player.y;
                float distancePowerUp1 = powerUpDx1 * powerUpDx1 + powerUpDy1 * powerUpDy1;
                if (distancePowerUp1 <= radiusSum * radiusSum && Raylib.IsKeyPressed(KeyboardKey.E))
                {
                    Config.isPicked = true;
                    Config.p1 = true;
                    if (powerUpStatus1 == 0)
                    {
                        // +5 Bullet Damage
                        Config.bulletDamage += 5;
                    }
                    else if (powerUpStatus1 == 1)
                    {
                        // +2 Bullet Radius
                        Config.bulletRadius += 2;
                    }
                    else if (powerUpStatus1 == 2)
                    {
                        // +10 Bullet Speed
                        Config.bulletSpeed += 10;
                    }
                    else if (powerUpStatus1 == 3)
                    {
                        // 10% Faster Cooldown
                        Config.bulletCooldown = Config.bulletCooldown * 0.9f;
                    }
                    else if (powerUpStatus1 == 4)
                    {
                        // -1 Radius
                        if (Config.playerRadius >= 2)
                            Config.playerRadius -= 1;
                    }
                    else if (powerUpStatus1 == 5)
                    {
                        // -10 Bullet Speed
                        if (Config.bulletSpeed >= 10)
                            Config.bulletSpeed -= 10;
                    }
                    else if (powerUpStatus1 == 6)
                    {
                        // 10% Slower Enemy Bullet Cooldown
                        Config.enemyBulletCooldown = Config.enemyBulletCooldown * 1.1f;
                    }
                    else if (powerUpStatus1 == 7)
                    {
                        // +20 Hitpoints
                        Config.playerHealthpoints += 20;
                    }
                    else if (powerUpStatus1 == 8)
                    {
                        // +10 Movement speed
                        Config.tempMovementSpeedHolder += 10;
                    }
                    return;
                }
            }

            if (!Config.p2)
            {
                float powerUpDx2 = posX2 - player.x;
                float powerUpDy2 = posY2 - player.y;
                float distancePowerUp2 = powerUpDx2 * powerUpDx2 + powerUpDy2 * powerUpDy2;
                if (distancePowerUp2 <= radiusSum * radiusSum && Raylib.IsKeyPressed(KeyboardKey.E))
                {
                    Config.isPicked = true;
                    Config.p2 = true;
                    if (powerUpStatus2 == 0)
                    {
                        // +5 Bullet Damage
                        Config.bulletDamage += 5;
                    }
                    else if (powerUpStatus2 == 1)
                    {
                        // +2 Bullet Radius
                        Config.bulletRadius += 2;
                    }
                    else if (powerUpStatus2 == 2)
                    {
                        // +10 Bullet Speed
                        Config.bulletSpeed += 10;
                    }
                    else if (powerUpStatus2 == 3)
                    {
                        // 10% Faster Cooldown
                        Config.bulletCooldown = Config.bulletCooldown * 0.9f;
                    }
                    else if (powerUpStatus2 == 4)
                    {
                        // -1 Radius
                        if (Config.playerRadius >= 2)
                            Config.playerRadius -= 1;
                    }
                    else if (powerUpStatus2 == 5)
                    {
                        // -10 Bullet Speed
                        if (Config.bulletSpeed >= 10)
                            Config.bulletSpeed -= 10;
                    }
                    else if (powerUpStatus2 == 6)
                    {
                        // 10% Slower Enemy Bullet Cooldown
                        Config.enemyBulletCooldown = Config.enemyBulletCooldown * 1.1f;
                    }
                    else if (powerUpStatus2 == 7)
                    {
                        // +20 Hitpoints
                        Config.playerHealthpoints += 20;
                    }
                    else if (powerUpStatus2 == 8)
                    {
                        // +10 Movement speed
                        Config.tempMovementSpeedHolder += 10;
                    }
                    return;
                }
            }

            if (!Config.p3)
            {
                float powerUpDx3 = posX3 - player.x;
                float powerUpDy3 = posY3 - player.y;
                float distancePowerUp3 = powerUpDx3 * powerUpDx3 + powerUpDy3 * powerUpDy3;
                if (distancePowerUp3 <= radiusSum * radiusSum && Raylib.IsKeyPressed(KeyboardKey.E))
                {
                    Config.isPicked = true;
                    Config.p3 = true;
                    if (powerUpStatus3 == 0)
                    {
                        // +5 Bullet Damage
                        Config.bulletDamage += 5;
                    }
                    else if (powerUpStatus3 == 1)
                    {
                        // +2 Bullet Radius
                        Config.bulletRadius += 2;
                    }
                    else if (powerUpStatus3 == 2)
                    {
                        // +10 Bullet Speed
                        Config.bulletSpeed += 10;
                    }
                    else if (powerUpStatus3 == 3)
                    {
                        // 10% Faster Cooldown
                        Config.bulletCooldown = Config.bulletCooldown * 0.9f;
                    }
                    else if (powerUpStatus3 == 4)
                    {
                        // -1 Radius
                        if (Config.playerRadius >= 2)
                            Config.playerRadius -= 1;
                    }
                    else if (powerUpStatus3 == 5)
                    {
                        // -10 Bullet Speed
                        if (Config.bulletSpeed >= 10)
                            Config.bulletSpeed -= 10;
                    }
                    else if (powerUpStatus3 == 6)
                    {
                        // 10% Slower Enemy Bullet Cooldown
                        Config.enemyBulletCooldown = Config.enemyBulletCooldown * 1.1f;
                    }
                    else if (powerUpStatus3 == 7)
                    {
                        // +20 Hitpoints
                        Config.playerHealthpoints += 20;
                    }
                    else if (powerUpStatus3 == 8)
                    {
                        // +10 Movement speed
                        Config.tempMovementSpeedHolder += 10;
                    }
                    return;
                }
            }
        }
        public void draw(float deltaTime)
        {
            if (Config.isPicked)
            {
                if (Config.despawnTime <= 0)
                {
                    return; //vi kör denna så den slutar rita powerupsen efter någon har tagits
                }
                Config.despawnTime -= deltaTime;
            }

            //väldigt confusing för allt ser så fult ut, den de skapar först cirkeln,
            //sen en grön rectangle, sen en vit rectangle för att göra det en outline
            //sedan texten efter, repeat på alla andra. alla -15 -100 och sånt är för centrering.
            Raylib.DrawCircleLines(posX1, posY1, radius, Color.DarkGreen); //outline cirkeln
            Raylib.DrawRectangle(posX1 - 120,posY1 - 15, 240, 30, Color.DarkGreen); //dark green rektangel
            Raylib.DrawRectangle(posX1 - 110,posY1 - 13, 220, 26, Color.White); //vit rektangel så den gröna ser ut som en outline
            int p1Text = Raylib.MeasureText($"{powerUpsArray[powerUpStatus1]}", 20); //kollar textens width för centrering
            Raylib.DrawText($"{powerUpsArray[powerUpStatus1]}", posX1 - p1Text/2, posY1-10, 20, Color.Black); //texten som säger vad för powerup det är

            Raylib.DrawCircleLines(posX2, posY2, radius, Color.DarkGreen);
            Raylib.DrawRectangle(posX2 - 120, posY2 - 15, 240, 30, Color.DarkGreen);
            Raylib.DrawRectangle(posX2 - 110, posY2 - 13, 220, 26, Color.White);
            int p2Text = Raylib.MeasureText($"{powerUpsArray[powerUpStatus2]}", 20);
            Raylib.DrawText($"{powerUpsArray[powerUpStatus2]}", posX2 - p2Text/2, posY2-10, 20, Color.Black);

            Raylib.DrawCircleLines(posX3, posY3, radius, Color.DarkGreen);
            Raylib.DrawRectangle(posX3 - 120, posY3 - 15, 240, 30, Color.DarkGreen);
            Raylib.DrawRectangle(posX3 - 110, posY3 - 13, 220, 26, Color.White);
            int p3Text = Raylib.MeasureText($"{powerUpsArray[powerUpStatus3]}", 20);
            Raylib.DrawText($"{powerUpsArray[powerUpStatus3]}", posX3 - p3Text/2, posY3-10, 20, Color.Black);

        }
    }
}
