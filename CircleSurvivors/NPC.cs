using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircleSurvivors
{
    public class NPC
    {
        public float x;
        public float y;
        float radius;
        float movementSpeed = 80;
        public NPC()
        {
            this.radius = 10;
        }
        public void draw()
        {
            Raylib.DrawCircle((int)x, (int)y, radius, Color.Red);
        }
        public void update()
        {

        }
    }
}
