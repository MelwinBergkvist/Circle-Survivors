using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CircleSurvivors
{
    public class EnemyBullets : Drawable
    {
        public float bulletX;
        public float bulletY;
        float moveX;
        float moveY;
        float x;
        float y;
        public EnemyBullets(Player player, NPC enemy)
        {
            bulletX = enemy.x;
            bulletY = enemy.y;

            float dxBullet = player.x - bulletX;
            float dyBullet = player.y - bulletY;
            float distance = MathF.Sqrt(dxBullet * dxBullet + dyBullet * dyBullet);

            moveX = (dxBullet / distance) * Config.bulletSpeed;
            moveY = (dyBullet / distance) * Config.bulletSpeed;
        }
        public void draw()
        {
            Raylib.DrawCircle((int)bulletX, (int)bulletY, Config.bulletRadius, Color.Black);
        }
        public void update(float deltaTime)
        {
            bulletX += moveX * deltaTime;
            bulletY += moveY * deltaTime;
        }
        public bool shouldDespawn()
        {
            return (bulletX < 0 || bulletX > Config.WindowSizeWidth || bulletY < 0 || bulletY > Config.WindowSizeHeight);
        }
    }
}
