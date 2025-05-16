using CircleSurvivors.Entities.Enemies;
using CircleSurvivors.Interfaces;
using CircleSurvivors.Core;
using Raylib_cs;
using CircleSurvivors.UI_Helpers;

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
            float despawnDistance = Helper.EuclideanFloat(ref Config.player.x, ref bulletX, ref Config.player.y, ref bulletY).distance;
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
            //kollar euclidean distance och updaterar inte det, håller samma linje
            bulletX = enemy.x;
            bulletY = enemy.y;

            float distanceX = Helper.EuclideanVector2(ref bulletX, ref Config.player.x, ref bulletY, ref Config.player.y).X;
            float distanceY = Helper.EuclideanVector2(ref bulletX, ref Config.player.x, ref bulletY, ref Config.player.y).Y;
            if (enemy.isBoss)
            {
                //om det är bossen som skjuter ska det targetta ett random stället och inte spelaren
                float randomX = random.Next((int)enemy.x - 100, (int)enemy.x + 100);
                float randomY = random.Next((int)enemy.y - 100, (int)enemy.y + 100);
                distanceX = Helper.EuclideanVector2(ref bulletX, ref randomX, ref bulletY, ref randomY).X;
                distanceY = Helper.EuclideanVector2(ref bulletX, ref randomX, ref bulletY, ref randomY).Y;
                enemyBulletRadius = 10;
            }

            moveX = distanceX * Config.enemyBulletSpeed;
            moveY = distanceY * Config.enemyBulletSpeed;
        }
    }
}
