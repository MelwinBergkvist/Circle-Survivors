using CircleSurvivors.Interfaces;
using CircleSurvivors.Core;
using Raylib_cs;
using CircleSurvivors.Entities.Enemies;

namespace CircleSurvivors.Entities
{
    /// <summary>
    /// hanterar alla bullets hos enemies
    /// </summary>
    public class EnemyBullets : IDrawable //implementerar drawable interface
    {
        Random random = new Random();
        public float bulletX, bulletY; //bullet coordinates
        float moveX, moveY; //vart den ska
        bool hasDealtDamage = false;
        int enemyBulletRadius = 5;

        /// <summary>
        /// kallar på shootingTrajectory
        /// </summary>
        /// <param name="enemy">enemies(som ska skjuta)</param>
        public EnemyBullets(NPC enemy)
        {
            ShootingTrajectory(enemy);   
        }
        /// <summary>
        /// ritar bullets
        /// </summary>
        public void Draw()
        {
            //ritar bulleten
            Raylib.DrawCircle((int)bulletX, (int)bulletY, enemyBulletRadius, Color.Black);
        }
        /// <summary>
        /// updaterar bullets position
        /// </summary>
        /// <param name="deltaTime">tid</param>
        public void Update(float deltaTime)
        {
            //plussar på distansen den ska åka, gånger deltaTime för frame independent movement
            bulletX += moveX * deltaTime;
            bulletY += moveY * deltaTime;

            ShouldDespawn();
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

        /// <summary>
        /// räknar fram bullet trajectory mellan enemy och spelare
        /// </summary>
        /// <param name="enemy">enemies</param>
        public void ShootingTrajectory(NPC enemy)
        {
            //Likadan som BaseAbility.cs fast reversed, kollar euclidean distance och updaterar inte det, håller samma linje
            bulletX = enemy.x;
            bulletY = enemy.y;

            float dxBullet = Config.player.x - bulletX;
            float dyBullet = Config.player.y - bulletY;
            if (enemy.isBoss)
            {
                //om det är bossen som skjuter ska det targetta ett random stället och inte spelaren
                dxBullet = random.Next(0, Config.WindowSizeWidth) - bulletX;
                dyBullet = random.Next(0,Config.WindowSizeHeight) - bulletY;
                enemyBulletRadius = 15;
            }
            float distanceBullets = MathF.Sqrt(dxBullet * dxBullet + dyBullet * dyBullet);

            moveX = dxBullet / distanceBullets * Config.enemyBulletSpeed;
            moveY = dyBullet / distanceBullets * Config.enemyBulletSpeed;
        }
    }
}
