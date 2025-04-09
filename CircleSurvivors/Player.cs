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
        float movementSpeed = 100;
        public Player(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.radius = 15;
        }
        public void draw()
        {
            Raylib.DrawCircle((int)x, (int)y, radius, Color.Green);
        }
        public void update(float deltaTime) //kaller över deltatime som ett argument
        {
            //bara if och inte if else för att vi vill att Playern ska kunna gå diagonalt
            if (Raylib.IsKeyDown(KeyboardKey.W)) y -= movementSpeed * deltaTime;
            if (Raylib.IsKeyDown(KeyboardKey.S)) y += movementSpeed * deltaTime;
            if (Raylib.IsKeyDown(KeyboardKey.A)) x -= movementSpeed * deltaTime;
            if (Raylib.IsKeyDown(KeyboardKey.D)) x += movementSpeed * deltaTime;
        }
    }
}
