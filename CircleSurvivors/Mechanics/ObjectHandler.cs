using CircleSurvivors.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircleSurvivors.Mechanics
{
    public class ObjectHandler
    {
        float collisionCooldown = 1.5f;
        float collisionCooldownTimer = 0f;
        float bossCollisionCooldown = 1.5f;
        float bossCollisionCooldownTimer = 0f;
        /// <summary>
        /// Kollar Collision och Despawn reqs för alla object som interfacet hanterar inom drawableList
        /// </summary>
        /// <param name="deltaTime">tid</param>
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
                bossCollisionCooldownTimer += deltaTime;
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
                if (item is Bosses bosses)
                {
                    //kolla alla bullets för collision med bossar
                    foreach (var bullet in Config.bullets)
                    {
                        bosses.BulletCollisionBoss(bullet, deltaTime);
                    }
                    //cooldown och sen kollar physical collision mellan spelare och boss
                    if (bossCollisionCooldownTimer >= bossCollisionCooldown)
                    {
                        bossCollisionCooldownTimer = 0;
                        bosses.BossCollision(deltaTime);
                    }
                }
                item.Draw();
            }
        }
    }
}
