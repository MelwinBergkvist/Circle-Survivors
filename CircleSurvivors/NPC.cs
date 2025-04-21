using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircleSurvivors
{
    public class NPC : Drawable
    {
        public float x;
        public float y;
        int hitpoints = 100;
        float radius;
        float movementSpeed = 80;
        public NPC() //constructor
        {
            this.radius = Config.npcRadius;
            Random randomSide = new Random();
            Random randomX = new Random();
            Random randomY = new Random();
            int side = randomSide.Next(1, 5); //1 till 5 för att få mellan 1 och 4, random shenanegins
            // 1 = vänster, 2 = höger, 3 = up, 4 = nere
            if (side == 1)
            {
                this.x = -radius;
                this.y = randomY.Next(0, Config.WindowSizeHeight);
            }
            else if (side == 2)
            {
                this.x = Config.WindowSizeWidth + radius;
                this.y = randomY.Next(0, Config.WindowSizeHeight);
            }
            else if (side == 3)
            {
                this.y = -radius;
                this.x = randomX.Next(0, Config.WindowSizeWidth);
            }
            else
            {
                this.y = Config.WindowSizeHeight + radius;
                this.x = randomX.Next(0, Config.WindowSizeWidth);
            }

        }
        public void draw()
        {
            //Color drawColor = closest ? Color.Pink : Color.Red; //if closest == true, pink, else, red
            Raylib.DrawCircle((int)x, (int)y, radius, Color.Red); //tar x & y som ints istället för floats
        }

        public bool shouldDespawn()
        {
            return hitpoints <= 0;
        }

        public void update(float deltaTime)
        {
            //Så de går mot spelaren
            float dx = Config.player.x - x;
            float dy = Config.player.y - y;
            float distance = MathF.Sqrt(dx * dx + dy * dy);

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
        public void bulletCollision(BaseAbility bullets)
        {
            float bulletEnemyDx = bullets.bulletX - x;
            float bulletEnemyDy = bullets.bulletY - y;
            float distanceBulletEnemy = bulletEnemyDx * bulletEnemyDx + bulletEnemyDy * bulletEnemyDy;
            float radiusSum = Config.bulletRadius + Config.npcRadius;
            if (distanceBulletEnemy <= radiusSum * radiusSum)
            {
                hitpoints -= 101;
            }
        }
    }
}
