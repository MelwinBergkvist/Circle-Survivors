using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircleSurvivors
{
    public class BaseAbility
    {
        public float x;
        public float y;
        float radiusH = 3f;
        float radiusV = 0.5f;
        public BaseAbility() //constructor
        {
            
        }
        public void draw()
        {
            Raylib.DrawEllipse((int)x, (int)y, radiusH, radiusV, Color.Blue);
        }
        public void update(List<NPC> enemies, Player player)
        {
            
        }
    }
}
