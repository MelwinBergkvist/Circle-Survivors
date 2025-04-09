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
        float movementSpeed = 80;
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
    }
}
