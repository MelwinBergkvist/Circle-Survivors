using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircleSurvivors
{
    public class ObjectHandler
    {
        float collisionCooldown = 1.5f;
        float collisionCooldownTimer = 0f;
        public void CheckDespawnAndCollision(float deltaTime)
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
                        Config.enemies.Remove(enemyNpc);
                        Config.killCount++;
                    }
                    if (item is BaseAbility bullet)
                    {
                        Config.bullets.Remove(bullet);
                    }
                    Config.drawableList.RemoveAt(i);

                    //om det ska despawna behövs det inte draw eller update så vi skippar till nästa
                    continue;
                }

                collisionCooldownTimer += deltaTime;
                item.Update(deltaTime);
                //om itemet i listan är från NPC körs if satsen
                if (item is NPC npc)
                {
                    //för varje bullet i bullets, kolla för collision med npc
                    foreach (var bullet in Config.bullets)
                    {
                        npc.BulletCollision(bullet, deltaTime);
                    }
                    //för varje enemy i listan, kolla om collision cooldown är över, isåfall skada spelaren och resetta timern
                    foreach (var enemy in Config.enemies)
                    {
                        if (collisionCooldownTimer >= collisionCooldown)
                        {
                            collisionCooldownTimer = 0;
                            npc.PlayerCollision(enemy, deltaTime);
                        }
                    }
                    //för varje enemybullet i listan, kolla om despawn requierments har mötts
                    foreach (var enemyBullets in Config.enemyBullet)
                    {
                        enemyBullets.ShouldDespawn();
                    }
                }
                item.Draw();
            }
        }
    }
}
