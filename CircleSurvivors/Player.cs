using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircleSurvivors
{
    public class Player
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
        public void update(float deltaTime) //kallar över deltatime som ett argument
        {
            //else satser på pilarna för att du inte ska kunna dubbla din speed genom att hålla ner båda
            //bara if och inte if else för att vi vill att Playern ska kunna gå diagonalt
            //One liners så behövs inga {}
            if (Raylib.IsKeyDown(KeyboardKey.W)) y -= movementSpeed * deltaTime;
            else if (Raylib.IsKeyDown(KeyboardKey.Up)) y -= movementSpeed * deltaTime;
            if (Raylib.IsKeyDown(KeyboardKey.S)) y += movementSpeed * deltaTime;
            else if (Raylib.IsKeyDown(KeyboardKey.Down)) y += movementSpeed * deltaTime;
            if (Raylib.IsKeyDown(KeyboardKey.A)) x -= movementSpeed * deltaTime;
            else if (Raylib.IsKeyDown(KeyboardKey.Left)) x -= movementSpeed * deltaTime;
            if (Raylib.IsKeyDown(KeyboardKey.D)) x += movementSpeed * deltaTime;
            else if (Raylib.IsKeyDown(KeyboardKey.Right)) x += movementSpeed * deltaTime;

            //Player kan inte lämna Canvas
            if (x-radius <= 0) x = radius;
            if (x+radius >= Config.WindowSizeWidth) x = Config.WindowSizeWidth - radius;
            if (y - radius <= 0) y = radius;
            if (y + radius >= Config.WindowSizeHeight) y = Config.WindowSizeHeight - radius;
        }
    }
}
