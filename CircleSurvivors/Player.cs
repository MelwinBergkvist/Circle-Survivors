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
        public float x, y;
        public int movementSpeed = 100;
        public int radius = 15;
        public int healthpoints = 100;
        public int maxHealthpoints = 100;
        public Player(int x, int y) //constructor
        {
            //specificerar att det är x och y från parametrarna
            this.x = x;
            this.y = y;
        }
        public void Draw()
        {
            //spelaren
            Raylib.DrawCircle((int)x, (int)y, Config.player.radius, Color.Green);

            //health bar
            Raylib.DrawRectangle((int)x-15, (int)y+25, 30,8, Color.Green);
            float healthWidth = 30 - (30 * healthpoints / maxHealthpoints);
            Raylib.DrawRectangle((int)x-15, (int)y+25, (int)healthWidth,8, Color.Red);
        }

        public bool ShouldDespawn()
        {
            return healthpoints <= 0;
        }

        public void Update(float deltaTime) //kallar över deltatime som ett argument
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
            if (x - radius <= 0) x = radius;
            if (x + radius >= Config.WindowSizeWidth) x = Config.WindowSizeWidth - radius;
            if (y - radius <= 0) y = radius;
            if (y + radius >= Config.WindowSizeHeight) y = Config.WindowSizeHeight - radius;
        }
    }
}
