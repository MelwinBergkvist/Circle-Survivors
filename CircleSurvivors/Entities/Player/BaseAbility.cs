using CircleSurvivors.Interfaces;
using CircleSurvivors.Core;
using Raylib_cs;
using CircleSurvivors.Entities.Enemies;

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
        /// <param name="closestEnemy"></param>
        public void ShootingTrajectory(NPC closestEnemy)
        {
            //räknar fram euclidean distance som en linje på en graf, och ändrar inte den.
            bulletX = Config.player.x;
            bulletY = Config.player.y;

            float dx = closestEnemy.x - bulletX;
            float dy = closestEnemy.y - bulletY;
            float distance = MathF.Sqrt(dx * dx + dy * dy);

            moveX = dx / distance * Config.bulletSpeed;
            moveY = dy / distance * Config.bulletSpeed;
        }
    }
}
