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
        float movementSpeed = 500; //temporärt hög movement speed för debugging
        public Player(int x, int y) //constructor
        {
            this.x = x;
            this.y = y;
            this.radius = 15;
        }
        public void draw()
        {
            Raylib.DrawCircle((int)x, (int)y, radius, Color.Green);
        }

        public bool shouldDespawn()
        {
            return false;
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
    }
}
