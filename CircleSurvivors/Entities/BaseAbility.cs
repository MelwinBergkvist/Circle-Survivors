using CircleSurvivors.Core;
using CircleSurvivors.Interfaces;
using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CircleSurvivors.Entities
{
    /// <summary>
    /// hanterar alla bullets av spelaren
    /// </summary>
    public class BaseAbility : IDrawable
    {
        public float bulletX;
        public float bulletY;
        float moveX;
        float moveY;

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
            Raylib.DrawCircle((int)bulletX, (int)bulletY, Config.bulletRadius, Color.Blue);
        }
        /// <summary>
        /// updaterar bullets position längst graf linjen
        /// </summary>
        /// <param name="deltaTime">tid</param>
        public void Update(float deltaTime)
        {
            bulletX += moveX * deltaTime;
            bulletY += moveY * deltaTime;
        }
        /// <summary>
        /// despawnar bullet om den är utanför canvas
        /// </summary>
        /// <returns>returnar true eller false beroände på dess position</returns>
        public bool ShouldDespawn()
        {
            return bulletX < 0 || bulletX > Config.WindowSizeWidth || bulletY < 0 || bulletY > Config.WindowSizeHeight;
        }
        /// <summary>
        /// gör shooting trajectory mellan spelare och närmaste enemy
        /// </summary>
        /// <param name="closestEnemy"></param>
        public void ShootingTrajectory(NPC closestEnemy)
        {
            bulletX = Config.player.x;
            bulletY = Config.player.y;

            float dx = closestEnemy.x - bulletX;
            float dy = closestEnemy.y - bulletY;
            float distance = MathF.Sqrt(dx * dx + dy * dy);

            moveX = dx / distance * Config.bulletSpeed;
            moveY = dy / distance * Config.bulletSpeed;
            //vi gör calculations i constructorn så den inte blir en homing bullet
            //nästan like som NPC movements bara på lite olika platser
            //räknar fram euclidean distance som en linje på en graf, och ändrar inte den.
        }
    }
}
