using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CircleSurvivors
{
    public class BaseAbility : Drawable
    {
        public float bulletX;
        public float bulletY;
        float moveX;
        float moveY;

        float dx;
        float dy;
        float distance;
        public BaseAbility(NPC closestEnemy) //constructor
        {
            bulletX = Config.player.x;
            bulletY = Config.player.y;

            dx = closestEnemy.x - this.bulletX;
            dy = closestEnemy.y - this.bulletY;
            distance = MathF.Sqrt(dx * dx + dy * dy);

            moveX = (dx / distance) * Config.bulletSpeed;
            moveY = (dy / distance) * Config.bulletSpeed;
            //vi gör calculations i constructorn så den inte blir en homing bullet
            //nästan like som NPC movements bara på lite olika platser
        }
        public void draw()
        {
            Raylib.DrawCircle((int)bulletX, (int)bulletY, Config.bulletRadius, Color.Blue);
        }
        public void update(float deltaTime)
        {           
            bulletX += moveX * deltaTime;
            bulletY += moveY * deltaTime;
        }
        public bool shouldDespawn()
        {
            //returnar true eller false beroände på dess position
            return (bulletX < 0 || bulletX > Config.WindowSizeWidth || bulletY < 0 || bulletY > Config.WindowSizeHeight);
        }
    }
}
