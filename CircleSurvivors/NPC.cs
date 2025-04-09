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
            Random randomSide = new Random();
            Random randomX = new Random();
            Random randomY = new Random();
            int side = randomSide.Next(1, 5); //1 till 5 för att få mellan 1 och 4, random shenanegins
            // 1 = vänster, 2 = höger, 3 = up, 4 = nere
            if (side == 1)
            {
                this.x = 0;
                this.y = randomY.Next(0, Config.WindowSizeHeight);
            }
            else if (side == 2)
            {
                this.x = Config.WindowSizeWidth;
                this.y = randomY.Next(0, Config.WindowSizeHeight);
            }
            else if (side == 3)
            {
                this.y = 0;
                this.x = randomX.Next(0, Config.WindowSizeWidth);
            }
            else
            {
                this.y = Config.WindowSizeHeight;
                this.x = randomX.Next(0, Config.WindowSizeWidth);
            }

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
