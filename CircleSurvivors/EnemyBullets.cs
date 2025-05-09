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
        bool hasDealtDamage = false;
        float moveX;
        float moveY;

        float dx;
        float dy;
        float despawnDistance;
        float radiusSum;

        float dxBullet;
        float dyBullet;
        float distanceBullets;

        public EnemyBullets(NPC enemy)
        {
            //Likadan som BaseAbility.cs fast reversed, kollar euclidean distance och updaterar inte det, håller samma linje
            bulletX = enemy.x;
            bulletY = enemy.y;

            dxBullet = Config.player.x - bulletX;
            dyBullet = Config.player.y - bulletY;
            distanceBullets = MathF.Sqrt(dxBullet * dxBullet + dyBullet * dyBullet);

            moveX = (dxBullet / distanceBullets) * Config.enemyBulletSpeed;
            moveY = (dyBullet / distanceBullets) * Config.enemyBulletSpeed;
        }
        public void draw()
        {
            Raylib.DrawCircle((int)bulletX, (int)bulletY, Config.enemyBulletRadius, Color.Black);
        }
        public void update(float deltaTime)
        {
            bulletX += moveX * deltaTime;
            bulletY += moveY * deltaTime;
        }
        public bool shouldDespawn()
        {
            //om bullet går utanför canvas, despawna
            if (bulletX < 0 || bulletX > Config.WindowSizeWidth || bulletY < 0 || bulletY > Config.WindowSizeHeight)
                return true;

            //om den colliderar med spelare tar bort, behövs ingen piercing
            dx = bulletX - Config.player.x;
            dy = bulletY - Config.player.y;
            despawnDistance = MathF.Sqrt(dx * dx + dy * dy);
            radiusSum = Config.bulletRadius + Config.playerRadius;
            if (despawnDistance < radiusSum)
            {
                if (!hasDealtDamage)
                {
                    hasDealtDamage = true; //Check så bullet bara skadar en gång
                    Config.playerHealthpoints -= Config.enemyBulletDamage;
                }
                return true;
            }
            return false;
        }
    }
}
