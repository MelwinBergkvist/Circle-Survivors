using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircleSurvivors
{
    public class Bosses : Drawable
    {
        public float x, y;
        float radius;
        int hitpoints;
        int movementSpeed;
        int bossCollisionDamage;
        public bool shouldShoot;


        public Bosses() 
        {
            Random random = new Random();
            int bossType = random.Next(0,4);
            switch(bossType)
            {
                case 0: //Normal
                    hitpoints += 500;
                    radius += 50;
                    movementSpeed += 40;
                    break;
                case 1: //Tanky

                    break;
                case 2: //Speedy

                    break;
                case 3: //Shooter

                    break;
            }

            //lika spawn mechanics som i NPC.cs
            int side = random.Next(1, 5);
            if (side == 1)
            {
                //vänster
                x = -radius;
                y = random.Next(0, Config.WindowSizeHeight);
            }
            else if (side == 2)
            {
                //höger
                x = Config.WindowSizeWidth + radius;
                y = random.Next(0, Config.WindowSizeHeight);
            }
            else if (side == 3)
            {
                //up
                y = -radius;
                x = random.Next(0, Config.WindowSizeWidth);
            }
            else
            {
                //ner
                y = Config.WindowSizeHeight + radius;
                x = random.Next(0, Config.WindowSizeWidth);
            }
        }
        public void Draw()
        {

        }
        public void Update(float deltaTime)
        {

        }
        public bool ShouldDespawn()
        {
            return hitpoints <= 0;
        }
    }
}
