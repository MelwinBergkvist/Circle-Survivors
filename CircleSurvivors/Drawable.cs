using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CircleSurvivors
{
    /*
     * Interface comments:
     * jag har gjort ett interface vilker är som ett kontrakt, 
     * det kan spara alla datatyper i en lista som implementerar, 
     * de får en gemensam närmnare
     * 
     * den låter mig update draw och kolla om något ska despawna, 
     * utan att ha massor av logiken i program game loopen
     * den beskriver vad som måste finnas men inte hur
     * 
     * gör allt mer generelt,
     * de gör de incompatible datatyperna compatible
     */
    public interface Drawable
    {
        public void update(float deltaTime);
        public void draw();
        public bool shouldDespawn();
    }
}
