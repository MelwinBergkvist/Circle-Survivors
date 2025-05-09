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

        float radiusSum;
        float powerUpDx1;
        float powerUpDy1;
        float distancePowerUp1;

        float powerUpDx2;
        float powerUpDy2;
        float distancePowerUp2;

        float powerUpDx3;
        float powerUpDy3;
        float distancePowerUp3;

        Random random = new Random();

        Color Instruction = Raylib.ColorAlpha(Color.DarkGray, 0.5f);

        int powerUpStatus1, powerUpStatus2, powerUpStatus3;

        String[] powerUpsArray = 
        {
                "+5 Bullet Damage", "+2 Bullet Radius", "+10 Bullet Speed",
                "10% Faster Cooldown", "-1 Radius", "-10 Bullet Speed",
                "-10% Enemy Cooldown", "+20 Hitpoints", "+10 Movement speed" // 0 - 8
        };

        public PowerUps() { }
        public void Update(float deltaTime)
        {
            radiusSum = radius + Config.player.radius;

            if (Config.isPicked)
                Config.waveCooldown -= deltaTime;

            //ser till att den inte rullar en ny powerup varje wave
            if (!Config.hasRolledThisRound)
            {
                powerUpStatus1 = random.Next(0, 9);
                powerUpStatus2 = random.Next(0, 9);
                powerUpStatus3 = random.Next(0, 9);
                Config.hasRolledThisRound = true;
            }

            //gör all matta som kollar collision så vi vet vilken som tas,
            //efter någon har tagits så kör vi en return
            //i program.cs så resetar vi isPicked, och alla pX så vi får välja powerup igen efter
            if (!Config.p1)
            {
                powerUpDx1 = posX1 - Config.player.x;
                powerUpDy1 = posY1 - Config.player.y;
                distancePowerUp1 = powerUpDx1 * powerUpDx1 + powerUpDy1 * powerUpDy1;
                if (distancePowerUp1 <= radiusSum * radiusSum && Raylib.IsKeyPressed(KeyboardKey.E))
                {
                    Config.isPicked = true;
                    Config.p1 = true;

                    switch (powerUpStatus1)
                    {
                        case 0:
                            Config.bulletDamage += 5;
                            break;
                        case 1:
                            Config.bulletRadius += 2;
                            break;
                        case 2:
                            Config.bulletSpeed += 10;
                            break;
                        case 3:
                            Config.bulletCooldown = Config.bulletCooldown * 0.9f;
                            break;
                        case 4:
                            if (Config.player.radius >= 2)
                                Config.player.radius -= 1;
                            break;
                        case 5:
                            if (Config.bulletSpeed >= 10)
                                Config.bulletSpeed -= 10;
                            break;
                        case 6:
                            Config.enemyBulletCooldown = Config.enemyBulletCooldown * 1.1f;
                            break;
                        case 7:
                            Config.player.healthpoints += 20;
                            Config.player.maxHealthpoints += 20;
                            break;
                        case 8:
                            Config.tempMovementSpeedHolder += 10;
                            break;
                    }
                    return;
                }
            }

            if (!Config.p2)
            {
                powerUpDx2 = posX2 - Config.player.x;
                powerUpDy2 = posY2 - Config.player.y;
                distancePowerUp2 = powerUpDx2 * powerUpDx2 + powerUpDy2 * powerUpDy2;
                if (distancePowerUp2 <= radiusSum * radiusSum && Raylib.IsKeyPressed(KeyboardKey.E))
                {
                    Config.isPicked = true;
                    Config.p2 = true;
                    switch (powerUpStatus2)
                    {
                        case 0:
                            Config.bulletDamage += 5;
                            break;
                        case 1:
                            Config.bulletRadius += 2;
                            break;
                        case 2:
                            Config.bulletSpeed += 10;
                            break;
                        case 3:
                            Config.bulletCooldown = Config.bulletCooldown * 0.9f;
                            break;
                        case 4:
                            if (Config.player.radius >= 2)
                                Config.player.radius -= 1;
                            break;
                        case 5:
                            if (Config.bulletSpeed >= 10)
                                Config.bulletSpeed -= 10;
                            break;
                        case 6:
                            Config.enemyBulletCooldown = Config.enemyBulletCooldown * 1.1f;
                            break;
                        case 7:
                            Config.player.healthpoints += 20;
                            Config.player.maxHealthpoints += 20;
                            break;
                        case 8:
                            Config.tempMovementSpeedHolder += 10;
                            break;
                    }
                    return;
                }
            }

            if (!Config.p3)
            {
                powerUpDx3 = posX3 - Config.player.x;
                powerUpDy3 = posY3 - Config.player.y;
                distancePowerUp3 = powerUpDx3 * powerUpDx3 + powerUpDy3 * powerUpDy3;
                if (distancePowerUp3 <= radiusSum * radiusSum && Raylib.IsKeyPressed(KeyboardKey.E))
                {
                    Config.isPicked = true;
                    Config.p3 = true;
                    switch (powerUpStatus3)
                    {
                        case 0:
                            Config.bulletDamage += 5;
                            break;
                        case 1:
                            Config.bulletRadius += 2;
                            break;
                        case 2:
                            Config.bulletSpeed += 10;
                            break;
                        case 3:
                            Config.bulletCooldown = Config.bulletCooldown * 0.9f;
                            break;
                        case 4:
                            if (Config.player.radius >= 2)
                                Config.player.radius -= 1;
                            break;
                        case 5:
                            if (Config.bulletSpeed >= 10)
                                Config.bulletSpeed -= 10;
                            break;
                        case 6:
                            Config.enemyBulletCooldown = Config.enemyBulletCooldown * 1.1f;
                            break;
                        case 7:
                            Config.player.healthpoints += 20;
                            Config.player.maxHealthpoints += 20;
                            break;
                        case 8:
                            Config.tempMovementSpeedHolder += 10;
                            break;
                    }
                    return;
                }
            }
        }
        public void Draw(float deltaTime)
        {
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
                Raylib.DrawText($"Next wave in: {(int)Config.waveCooldown}", Config.WindowSizeWidth / 2 - waveTextWidth / 2, Config.WindowSizeHeight / 8, 40, Color.DarkPurple);
            }

            if (Config.isPicked)
            {
                if (Config.despawnTime <= 0)
                {
                    return; //vi kör denna så den slutar rita powerupsen efter någon har tagits, med en cooldown
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
