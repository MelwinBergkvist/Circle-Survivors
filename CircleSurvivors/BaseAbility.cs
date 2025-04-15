using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CircleSurvivors
{
    public class BaseAbility
    {
        public float bulletX;
        public float bulletY;
        float bulletSpeed = 300f;        
        float radius = 5f;
        float moveX;
        float moveY;
        public BaseAbility(Player player, NPC closestEnemy) //constructor
        {
            bulletX = player.x;
            bulletY = player.y;

            float dx = closestEnemy.x - this.bulletX;
            float dy = closestEnemy.y - this.bulletY;
            float distance = MathF.Sqrt(dx * dx + dy * dy);

            moveX = (dx / distance) * bulletSpeed;
            moveY = (dy / distance) * bulletSpeed;
            //vi gör calculations i constructorn så den inte blir en homing bullet
            //nästan like som NPC movements bara på lite olika platser
        }
        public void draw()
        {
            Raylib.DrawCircle((int)bulletX, (int)bulletY, radius, Color.Blue);
        }
        public void update(float deltaTime)
        {           
            bulletX += moveX * deltaTime;
            bulletY += moveY * deltaTime;
        }
        public bool offScreenBullet()
        {
            if (bulletX < 0 || bulletX > Config.WindowSizeWidth || bulletY < 0 || bulletY > Config.WindowSizeHeight) return true;
            else return false;
        }
    }
}
