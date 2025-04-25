using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircleSurvivors
{
    public class Player : Drawable
    {
        public float x;
        public float y;
        float radius;
        float movementSpeed = 100; //temporärt hög movement speed för debugging
        float hitpoints = 100;
        float collisionCooldown = 0f;
        public Player(int x, int y) //constructor
        {
            this.x = x;
            this.y = y;
            this.radius = 15;
        }
        public void draw()
        {
            //spelaren
            Raylib.DrawCircle((int)x, (int)y, radius, Color.Green);

            //health bar
            Raylib.DrawRectangle((int)x-15, (int)y+25, 30,8, Color.Green);
            float healthWidth = 30 - (30 * hitpoints / 100);
            Raylib.DrawRectangle((int)x-15, (int)y+25, (int)healthWidth,8, Color.Red);
            //Raylib.DrawText($"Cooldown: {collisionCooldown:F2}", 10, 100, 20, Color.Gray);
        }

        public bool shouldDespawn()
        {
            return hitpoints <= 0;
        }

        public void update(float deltaTime) //kallar över deltatime som ett argument
        {
            //bara if och inte if else för att vi vill att Playern ska kunna gå diagonalt
            if (Raylib.IsKeyDown(KeyboardKey.W) || Raylib.IsKeyDown(KeyboardKey.Up)) 
                y -= movementSpeed * deltaTime;            
            if (Raylib.IsKeyDown(KeyboardKey.S) || Raylib.IsKeyDown(KeyboardKey.Down))
                y += movementSpeed * deltaTime;            
            if (Raylib.IsKeyDown(KeyboardKey.A) || Raylib.IsKeyDown(KeyboardKey.Left)) 
                x -= movementSpeed * deltaTime;
            if (Raylib.IsKeyDown(KeyboardKey.D) || Raylib.IsKeyDown(KeyboardKey.Right))
                x += movementSpeed * deltaTime;

            //Player kan inte lämna Canvas
            if (x-radius <= 0) x = radius;
            if (x+radius >= Config.WindowSizeWidth) x = Config.WindowSizeWidth - radius;
            if (y - radius <= 0) y = radius;
            if (y + radius >= Config.WindowSizeHeight) y = Config.WindowSizeHeight - radius;
        }
        public void playerCollision(NPC enemies, float deltaTime)
        {
            //Hit collision för spelare, basically en kopia av den som finns för bullets i NPC.cs, matte vis
            if (collisionCooldown > 0)
                collisionCooldown -= deltaTime;

            float playerEnemyDx = x - enemies.x;
            float playerEnemyDy = y - enemies.y;
            float distancePlayerEnemy = playerEnemyDx * playerEnemyDx + playerEnemyDy * playerEnemyDy;
            float radiusSum = radius + Config.npcRadius;

            if (distancePlayerEnemy <= radiusSum * radiusSum)
            {
                if (collisionCooldown <= 0f)
                {
                    hitpoints -= 5;
                    collisionCooldown = 15f;
                }
            }
        }
    }
}
