using CircleSurvivors.Interfaces;
using CircleSurvivors.Mechanics;
using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CircleSurvivors.Entities
{
    public class EnemyBullets : IDrawable
    {
        public float bulletX;
        public float bulletY;
        bool hasDealtDamage = false;
        readonly float moveX;
        readonly float moveY;

        /// <summary>
        /// bestämmer individuella graf linjen som enemy bullet ska skjutas vid
        /// </summary>
        /// <param name="enemy">enemies(som ska skjuta)</param>
        public EnemyBullets(NPC enemy)
        {
            //Likadan som BaseAbility.cs fast reversed, kollar euclidean distance och updaterar inte det, håller samma linje
            bulletX = enemy.x;
            bulletY = enemy.y;

            float dxBullet = Config.player.x - bulletX;
            float dyBullet = Config.player.y - bulletY;
            float distanceBullets = MathF.Sqrt(dxBullet * dxBullet + dyBullet * dyBullet);

            moveX = dxBullet / distanceBullets * Config.enemyBulletSpeed;
            moveY = dyBullet / distanceBullets * Config.enemyBulletSpeed;
        }
        /// <summary>
        /// ritar bullets
        /// </summary>
        public void Draw()
        {
            Raylib.DrawCircle((int)bulletX, (int)bulletY, Config.enemyBulletRadius, Color.Black);
        }
        /// <summary>
        /// updaterar bullets position
        /// </summary>
        /// <param name="deltaTime">tid</param>
        public void Update(float deltaTime)
        {
            bulletX += moveX * deltaTime;
            bulletY += moveY * deltaTime;
        }
        /// <summary>
        /// kollar om bullet möter reqs för att despawna (utanför canvas/collide med spelare)
        /// </summary>
        /// <returns>returnarn true/false beroände på bullets position</returns>
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
