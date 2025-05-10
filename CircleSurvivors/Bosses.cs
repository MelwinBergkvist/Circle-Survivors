using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircleSurvivors
{
    public class Bosses : Drawable
    {
        public float x, y;
        float radius;
        int hitpoints;
        int maxHitpoints;
        int movementSpeed;
        int bossCollisionDamage;
        public bool shouldShoot;
        Color bossColor;
        Color bossHealthColor;

        /// <summary>
        /// initializerar Bossar och bestämmer vilken typ det är
        /// </summary>
        public Bosses() 
        {
            Random random = new Random();
            int bossType = random.Next(0,4);
            switch(bossType)
            {
                case 0: //Normal
                    hitpoints += 500;
                    maxHitpoints += 500;
                    radius += 50;
                    movementSpeed += 40;
                    bossCollisionDamage += 30;
                    bossColor = new Color(156, 6, 6);
                    bossHealthColor = new Color(77, 8, 8);
                    break;
                case 1: //Tanky
                    hitpoints += 750;
                    maxHitpoints += 750;
                    radius += 55;
                    movementSpeed += 30;
                    bossCollisionDamage += 45;
                    bossColor = new Color(19, 138, 11);
                    bossHealthColor = new Color(8, 77, 3);
                    break;
                case 2: //Speedy
                    hitpoints += 250;
                    maxHitpoints += 250;
                    radius += 35;
                    movementSpeed += 70;
                    bossCollisionDamage += 20;
                    bossColor = new Color(120, 6, 191);
                    bossHealthColor = new Color(85, 5, 135);
                    break;
                case 3: //Shooter
                    hitpoints += 400;
                    maxHitpoints += 500;
                    radius += 50;
                    movementSpeed += 40;
                    bossCollisionDamage += 30;
                    shouldShoot = true;
                    bossColor = new Color(0, 0, 0);
                    bossHealthColor = new Color(65, 65, 65);
                    break;
            }

            //lika spawn mechanics som i NPC.cs
            int side = random.Next(1, 5);
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
        /// ritar bossarna
        /// </summary>
        public void Draw()
        {
            Raylib.DrawCircle((int)x, (int)y, radius, bossColor);

            //2nd radius som en healthmeter typ
            float healthRadius = radius - (radius * hitpoints / maxHitpoints);
            Raylib.DrawCircle((int)x, (int)y, healthRadius, bossHealthColor);
        }
        /// <summary>
        /// updaterar bossarnas position
        /// </summary>
        /// <param name="deltaTime">tid</param>
        public void Update(float deltaTime)
        {
            //Direkt kopia från NPC.cs, eauclidean distance, shooters går inte nära
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
        }
        /// <summary>
        /// returnar om det ska despawna
        /// </summary>
        /// <returns>true om den ska despawna, annars false</returns>
        public bool ShouldDespawn()
        {
            return hitpoints <= 0;
        }
    }
}
