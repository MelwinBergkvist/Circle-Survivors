using CircleSurvivors.Core;
using CircleSurvivors.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircleSurvivors.Mechanics
{
    /// <summary>
    /// hanterar alla object
    /// </summary>
    public class ObjectHandler
    {
        /// <summary>
        /// Kollar Despawn reqs för alla object som interfacet hanterar inom drawableList
        /// </summary>
        /// <param name="deltaTime">tid</param>
        public void CheckDespawn(float deltaTime)
        {
            //Kollar om jag ska despawn itemen annars så draw och update
            for (int i = Config.drawableList.Count - 1; i >= 0; i--)
            {
                var item = Config.drawableList[i];
                //Drawable interfacet har en shouldDespawn function,
                //så alla .cs som implementerar interfacet har en shouldDespawn med sina egna requierments
                if (item.ShouldDespawn())
                {
                    if (item is NPC enemyNpc)
                    {
                        Config.enemiesList.Remove(enemyNpc);
                        Config.killCount++;
                    }
                    if (item is BaseAbility bullet)
                    {
                        Config.bulletsList.Remove(bullet);
                    }
                    Config.drawableList.RemoveAt(i);
                    //om det ska despawna behövs det inte draw eller update så vi skippar till nästa
                    continue;
                }
                if (!Config.isPaused)
                    item.Update(deltaTime);
                item.Draw();
            }
        }
    }
}
