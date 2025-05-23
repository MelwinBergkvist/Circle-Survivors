﻿namespace CircleSurvivors.Interfaces
{
    /*
     * Interface comments:
     * jag har gjort ett interface vilker är som ett kontrakt, 
     * det kan spara alla datatyper i en lista som implementerar, 
     * de får en gemensam nämnare
     * 
     * den låter mig update draw och kolla om något ska despawna, 
     * utan att ha massor av logiken i program game loopen
     * den beskriver vad som måste finnas men inte hur
     * 
     * gör allt mer generelt,
     * de gör de incompatible datatyperna compatible
     * 
     * jag glömde att lägga till men jag fick mycket hjälp
     * av min broder med att skapa detta interface
     */
    /// <summary>
    /// hanterar Update, Draw, ShouldDespawn som ett interface
    /// </summary>
    public interface IDrawable
    {
        /// <summary>
        /// grupp Update metod för alla classes som implementerar Drawable interfacet, updaterar objektet
        /// </summary>
        /// <param name="deltaTime">tid</param>
        public void Update(float deltaTime);
        /// <summary>
        /// grupp Draw metod för alla classes som implementerar Drawable interfacet, ritar objektet
        /// </summary>
        public void Draw();
        /// <summary>
        /// grupp ShouldDespawn metod för alla classes som implementerar Drawable interfacet, kollar om objektet ska despawna
        /// </summary>
        /// <returns>om objektet ska despawna</returns>
        public bool ShouldDespawn();
    }
}
