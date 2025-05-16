using CircleSurvivors.Entities.Enemies;
using CircleSurvivors.UI_Helpers;
using CircleSurvivors.Interfaces;
using CircleSurvivors.Core;
using Raylib_cs;

namespace CircleSurvivors.Entities.Player
{
    /// <summary>
    /// hanterar alla bullets av spelaren
    /// </summary>
    public class BaseAbility : IDrawable //implementerar drawable interface
    {
        public float bulletX, bulletY; //bullet coordinates
        float moveX, moveY; //vart bullet ska
        /// <summary>
        /// kallar på shootingTrajectory
        /// </summary>
        /// <param name="closestEnemy"></param>
        public BaseAbility(NPC closestEnemy) //constructor
        {
            ShootingTrajectory(closestEnemy);
        }
        /// <summary>
        /// ritar bullets
        /// </summary>
        public void Draw()
        {
            //ritar bulleten
            Raylib.DrawCircle((int)bulletX, (int)bulletY, Config.bulletRadius, Color.Blue);
        }
        /// <summary>
        /// updaterar bullets position längst graf linjen
        /// </summary>
        /// <param name="deltaTime">tid</param>
        public void Update(float deltaTime)
        {
            //plussar på distansen den ska åka, gånger deltaTime för frame independent movement
            bulletX += moveX * deltaTime;
            bulletY += moveY * deltaTime;
        }
        /// <summary>
        /// despawnar bullet om den är utanför canvas
        /// </summary>
        /// <returns>returnar true eller false beroände på dess position</returns>
        public bool ShouldDespawn()
        {
            //om den är utanför canvasen så returnar den true, att den ska despawna
            return bulletX < 0 || bulletX > Config.WindowSizeWidth || bulletY < 0 || bulletY > Config.WindowSizeHeight; 
        }
        /// <summary>
        /// gör shooting trajectory mellan spelare och närmaste enemy
        /// </summary>
        /// <param name="closestEnemy">närmaste enemy</param>
        public void ShootingTrajectory(NPC closestEnemy)
        {
            //räknar fram euclidean distance som en linje på en graf, och ändrar inte den.
            bulletX = Config.player.x;
            bulletY = Config.player.y;

            float distanceXvector = Helper.EuclideanVector2(ref bulletX, ref closestEnemy.x, ref bulletY, ref closestEnemy.y).X;
            float distanceYvector = Helper.EuclideanVector2(ref bulletX, ref closestEnemy.x, ref bulletY, ref closestEnemy.y).Y;

            moveX = distanceXvector * Config.bulletSpeed;
            moveY = distanceYvector * Config.bulletSpeed;
        }
        //public void ClosestEnemyPointer(NPC closestEnemy)
        //{
        //    float dx = closestEnemy.x - Config.player.x;
        //    float dy = closestEnemy.y - Config.player.y;
        //    float distance = MathF.Sqrt(dx * dx + dy * dy);

        //    float directionX = dx / distance * Config.bulletSpeed;
        //    float directionY = dy / distance * Config.bulletSpeed;

        //    Raylib.DrawCircle((int)Config.player.x + (int)directionX / 8, (int)Config.player.y + (int)directionY / 8, 2, Color.Blue);
        //}
    }
}
