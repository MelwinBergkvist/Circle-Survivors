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
        readonly float moveX;
        readonly float moveY;

        public EnemyBullets(NPC enemy)
        {
            //Likadan som BaseAbility.cs fast reversed, kollar euclidean distance och updaterar inte det, håller samma linje
            bulletX = enemy.x;
            bulletY = enemy.y;

            float dxBullet = Config.player.x - bulletX;
            float dyBullet = Config.player.y - bulletY;
            float distanceBullets = MathF.Sqrt(dxBullet * dxBullet + dyBullet * dyBullet);

            moveX = (dxBullet / distanceBullets) * Config.enemyBulletSpeed;
            moveY = (dyBullet / distanceBullets) * Config.enemyBulletSpeed;
        }
        public void Draw()
        {
            Raylib.DrawCircle((int)bulletX, (int)bulletY, Config.enemyBulletRadius, Color.Black);
        }
        public void Update(float deltaTime)
        {
            bulletX += moveX * deltaTime;
            bulletY += moveY * deltaTime;
        }
        public bool ShouldDespawn()
        {
            //om bullet går utanför canvas, despawna
            if (bulletX < 0 || bulletX > Config.WindowSizeWidth || bulletY < 0 || bulletY > Config.WindowSizeHeight)
                return true;

            //om den colliderar med spelare tar bort, behövs ingen piercing
            float dx = bulletX - Config.player.x;
            float dy = bulletY - Config.player.y;
            float despawnDistance = MathF.Sqrt(dx * dx + dy * dy);
            float radiusSum = Config.bulletRadius + Config.player.radius;
            if (despawnDistance < radiusSum)
            {
                if (!hasDealtDamage)
                {
                    hasDealtDamage = true; //Check så bullet bara skadar en gång
                    Config.player.healthpoints -= Config.enemyBulletDamage;
                }
                return true;
            }
            return false;
        }
    }
}
