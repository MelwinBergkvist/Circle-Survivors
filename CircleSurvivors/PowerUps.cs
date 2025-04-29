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

        public PowerUps() { }

        public void update(Player player)
        {
            float radiusSum = radius + Config.playerRadius;
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
                    Config.bulletDamage += 5;
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
                    Config.bulletRadius += 2;
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
                    Config.bulletSpeed += 10;
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
            Raylib.DrawRectangle(posX1 - 100,posY1 - 15, 200, 30, Color.DarkGreen); //dark green rektangel
            Raylib.DrawRectangle(posX1 - 95,posY1 - 13, 190, 26, Color.White); //vit rektangel så den gröna ser ut som en outline
            int p1Text = Raylib.MeasureText("+5 Bullet Damage", 20); //kollar textens width för centrering
            Raylib.DrawText("+5 Bullet Damage", posX1 - p1Text/2, posY1-10, 20, Color.Black); //texten som säger vad för powerup det är

            Raylib.DrawCircleLines(posX2, posY2, radius, Color.DarkGreen);
            Raylib.DrawRectangle(posX2 - 100, posY2 - 15, 200, 30, Color.DarkGreen);
            Raylib.DrawRectangle(posX2 - 95, posY2 - 13, 190, 26, Color.White);
            int p2Text = Raylib.MeasureText("+2 Bullet Radius", 20);
            Raylib.DrawText("+2 Bullet Radius", posX2 - p2Text/2, posY2-10, 20, Color.Black);

            Raylib.DrawCircleLines(posX3, posY3, radius, Color.DarkGreen);
            Raylib.DrawRectangle(posX3 - 100, posY3 - 15, 200, 30, Color.DarkGreen);
            Raylib.DrawRectangle(posX3 - 95, posY3 - 13, 190, 26, Color.White);
            int p3Text = Raylib.MeasureText("+10 Bullet Speed", 20);
            Raylib.DrawText("+10 Bullet Speed", posX3 - p3Text/2, posY3-10, 20, Color.Black);

        }
    }
}
