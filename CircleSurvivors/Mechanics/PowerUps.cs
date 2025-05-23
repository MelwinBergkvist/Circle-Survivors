﻿using CircleSurvivors.UI_Helpers;
using CircleSurvivors.Core;
using Raylib_cs;

namespace CircleSurvivors.Mechanics
{
    /// <summary>
    /// hanterar alla powerups
    /// </summary>
    public class PowerUps
    {
        //alla positioner som powerupsen ska ligga på
        float posX1 = Config.WindowSizeWidth / 2 - Config.WindowSizeWidth / 6;
        float posY1 = Config.WindowSizeHeight / 4;

        float posX2 = Config.WindowSizeWidth / 2;
        float posY2 = Config.WindowSizeHeight / 3;

        float posX3 = Config.WindowSizeWidth / 2 + Config.WindowSizeWidth / 6;
        float posY3 = Config.WindowSizeHeight / 4;

        readonly int radius = 50;
        readonly Random random = new Random();
        
        public bool p1, p2, p3 = false;
        public bool isPicked = false;
        public bool hasRolledThisRound = false;
        public float despawnTime = 0.5f;

        Color Instruction = Raylib.ColorAlpha(Color.DarkGray, 0.5f);

        int powerUpStatus1, powerUpStatus2, powerUpStatus3;

        string[] powerUpsArray =
        {
                "+20 Bullet Damage",        //0
                "+2 Bullet Radius",         //1
                "+10 Bullet Speed",         //2
                "10% Faster Cooldown",      //3
                "-1 Radius",                //4
                "-10 Bullet Speed",         //5
                "-10% Enemy Cooldown",      //6
                "+20 Hitpoints",            //7
                "+30 Movement speed",       //8
                "+10% dash regain",         //9
                "+10% dash duration"        //10
        };

        public PowerUps() { }
        /// <summary>
        /// rullar en random powerUp och kollar om spelaren har plockat upp den
        /// </summary>
        /// <param name="deltaTime">tid</param>
        public void Update(float deltaTime)
        {
            float radiusSum = radius + Config.player.radius;

            if (Config.powerUps.isPicked)
                Config.waveCooldown -= deltaTime;

            //ser till att den inte rullar en ny powerup varje wave
            if (!Config.powerUps.hasRolledThisRound)
            {
                powerUpStatus1 = random.Next(0, powerUpsArray.Count());
                powerUpStatus2 = random.Next(0, powerUpsArray.Count());
                powerUpStatus3 = random.Next(0, powerUpsArray.Count());

                //ser till att alla är olika
                while (powerUpStatus1 == powerUpStatus2)
                    powerUpStatus2 = random.Next(0, powerUpsArray.Count());
                while (powerUpStatus1 == powerUpStatus3 || powerUpStatus2 == powerUpStatus3)
                    powerUpStatus3 = random.Next(0, powerUpsArray.Count());

                Config.powerUps.hasRolledThisRound = true;
            }

            //gör all matta som kollar collision så vi vet vilken som tas,
            //efter någon har tagits så kör vi en return
            //i program.cs så resetar vi isPicked, och alla pX så vi får välja powerup igen efter
            if (!Config.powerUps.p1)
            {
                float distancePowerUp1 = Helper.EuclideanFloat(ref Config.player.x, ref posX1, ref Config.player.y, ref posY1).distance;
                if (distancePowerUp1 <= radiusSum && Raylib.IsKeyPressed(KeyboardKey.E))
                {
                    Config.powerUps.isPicked = true;
                    Config.powerUps.p1 = true;

                    switch (powerUpStatus1)
                    {
                        case 0:
                            Config.bulletDamage += 20;
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
                            Config.tempMovementSpeedHolder += 30;
                            break;
                        case 9:
                            Config.player.dashRegain = Config.player.dashRegain * 0.9f;
                            break;
                        case 10:
                            Config.player.maxDashDuration = Config.player.maxDashDuration * 1.1f;
                            break;
                    }
                    return;
                }
            }

            if (!Config.powerUps.p2)
            {
                float distancePowerUp2 = Helper.EuclideanFloat(ref Config.player.x, ref posX2, ref Config.player.y, ref posY2).distance;
                if (distancePowerUp2 <= radiusSum && Raylib.IsKeyPressed(KeyboardKey.E))
                {
                    Config.powerUps.isPicked = true;
                    Config.powerUps.p2 = true;
                    switch (powerUpStatus2)
                    {
                        case 0:
                            Config.bulletDamage += 20;
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
                            Config.tempMovementSpeedHolder += 30;
                            break;
                        case 9:
                            Config.player.dashRegain = Config.player.dashRegain * 0.9f;
                            break;
                        case 10:
                            Config.player.maxDashDuration = Config.player.maxDashDuration * 1.1f;
                            break;
                    }
                    return;
                }
            }

            if (!Config.powerUps.p3)
            {
                float distancePowerUp3 = Helper.EuclideanFloat(ref Config.player.x, ref posX3, ref Config.player.y, ref posY3).distance;
                if (distancePowerUp3 <= radiusSum && Raylib.IsKeyPressed(KeyboardKey.E))
                {
                    Config.powerUps.isPicked = true;
                    Config.powerUps.p3 = true;
                    switch (powerUpStatus3)
                    {
                        case 0:
                            Config.bulletDamage += 20;
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
                            Config.tempMovementSpeedHolder += 30;
                            break;
                        case 9:
                            Config.player.dashRegain = Config.player.dashRegain * 0.9f;
                            break;
                        case 10:
                            Config.player.maxDashDuration = Config.player.maxDashDuration * 1.1f;
                            break;
                    }
                    return;
                }
            }
        }
        /// <summary>
        /// ritar powerUpsen
        /// </summary>
        /// <param name="deltaTime">tid</param>
        public void Draw(float deltaTime)
        {
            if (!Config.powerUps.isPicked)
            {
                Helper.DrawCenteredText("Next wave after power-up has been chosen", Config.WindowSizeWidth / 2, Config.WindowSizeHeight / 8, 40, Color.DarkPurple);
                Helper.DrawCenteredText($"Power-up is picked up by walking inside the circle and pressing E", Config.WindowSizeWidth / 2, Config.WindowSizeHeight / 2 + Config.WindowSizeHeight / 4, 18, Instruction);
            }
            else
            {
                Helper.DrawCenteredText($"Next wave in: {(int)Config.waveCooldown}", Config.WindowSizeWidth / 2, Config.WindowSizeHeight / 8, 40, Color.DarkPurple);
            }

            if (Config.powerUps.isPicked)
            {
                if (Config.powerUps.despawnTime <= 0)
                {
                    return; //vi kör denna så den slutar rita powerupsen efter någon har tagits, med en cooldown
                }
                Config.powerUps.despawnTime -= deltaTime;
            }


            //väldigt confusing för allt ser så fult ut, den de skapar först cirkeln,
            //sen en grön rectangle, sen en vit rectangle för att göra det en outline
            //sedan texten efter, repeat på alla andra. alla -15 -100 och sånt är för centrering.
            Raylib.DrawCircleLines((int)posX1, (int)posY1, radius, Color.DarkGreen); //outline cirkeln
            Raylib.DrawRectangle((int)posX1 - 120, (int)posY1 - 15, 240, 30, Color.DarkGreen); //dark green rektangel
            Raylib.DrawRectangle((int)posX1 - 110, (int)posY1 - 13, 220, 26, Color.White); //vit rektangel så den gröna ser ut som en outline
            Helper.DrawCenteredText($"{powerUpsArray[powerUpStatus1]}", (int)posX1, (int)posY1 - 10, 20, Color.Black); //texten som säger vad för powerup det är

            Raylib.DrawCircleLines((int)posX2, (int)posY2, radius, Color.DarkGreen);
            Raylib.DrawRectangle((int)posX2 - 120, (int)posY2 - 15, 240, 30, Color.DarkGreen);
            Raylib.DrawRectangle((int)posX2 - 110, (int)posY2 - 13, 220, 26, Color.White);
            Helper.DrawCenteredText($"{powerUpsArray[powerUpStatus2]}", (int)posX2, (int)posY2 - 10, 20, Color.Black);

            Raylib.DrawCircleLines((int)posX3, (int)posY3, radius, Color.DarkGreen);
            Raylib.DrawRectangle((int)posX3 - 120, (int)posY3 - 15, 240, 30, Color.DarkGreen);
            Raylib.DrawRectangle((int)posX3 - 110, (int)posY3 - 13, 220, 26, Color.White);
            Helper.DrawCenteredText($"{powerUpsArray[powerUpStatus3]}", (int)posX3, (int)posY3 - 10, 20, Color.Black);

        }
    }
}
