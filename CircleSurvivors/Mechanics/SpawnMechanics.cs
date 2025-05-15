using CircleSurvivors.Entities;
using CircleSurvivors.Core;

namespace CircleSurvivors.Mechanics
{
    /// <summary>
    /// köter alla spawn mechanics för wavsen
    /// </summary>
    public class SpawnMechanics
    {
        bool noMoreTempSpeed = false;
        float spawnTime = 1f;
        /// <summary>
        /// köter alla spawn mechanics, för waves och powerups
        /// </summary>
        /// <param name="deltaTime">tiden</param>
        public void Spawn(float deltaTime)
        {
            //när alla enemies är döda, ny wave och +1 wave count
            if (Config.enemiesList.Count <= 0 && Config.enemieSpawnCount <= 0 && Config.firstWaveAfterRestart != true)
            {
                Config.bulletsList.Clear();

                //stoppar timern mellan waves
                Config.countTime = false;

                if (!noMoreTempSpeed)
                    Config.tempMovementSpeedHolder = Config.player.movementSpeed;
                noMoreTempSpeed = true;

                Config.player.movementSpeed = 300; //temporärt gör movementspeed högre, simple qol

                Config.powerUps.Draw(deltaTime);
                Config.powerUps.Update(deltaTime);

                if (Config.waveCooldown < 1)
                {
                    Config.enemieSpawnCount = 10 + Config.wave * Config.wave; //scaling
                    Config.waveCooldown = 3.99f; //nära 4 men inte 4, annars flashar en 4 pförsta framen
                    Config.wave++;
                    //resettar alla states, annars så får vi inte välja om powerups waven efter
                    Config.powerUps.isPicked = false;
                    Config.powerUps.p1 = false;
                    Config.powerUps.p2 = false;
                    Config.powerUps.p3 = false;
                    Config.shouldBossSpawn = true;
                    Config.powerUps.hasRolledThisRound = false;
                    Config.powerUps.despawnTime = 0.5f;
                    Config.player.movementSpeed = Config.tempMovementSpeedHolder; //tillbaka till segis
                    noMoreTempSpeed = false;
                }
            }

            //Om man dör och respawnar så ser denna till att ingen powerup spawnas första rundan
            Config.firstWaveAfterRestart = false;

            if (Config.enemieSpawnCount > 0)
            {
                //börjar den pausade timern
                Config.countTime = true;

                //ser till att inta alla enemies spawnar direkt och samtidigt
                spawnTime -= deltaTime;
                if (spawnTime <= 0)
                {
                    //eftersom NPCn är mer än en så deklrareras den mer än en gång
                    NPC npc = new NPC();
                    Config.enemiesList.Add(npc);
                    Config.drawableList.Add(npc);
                    Config.shouldBossSpawn = false;

                    //scaling för hur snabbt saker ska spawna så det inte tar 30 min per runda,
                    //också ser till att det aldrig blir instant
                    if (Config.enemieSpawnCount < 100)
                        spawnTime = 1f - Config.enemieSpawnCount / 100f;
                    else 
                        spawnTime = 0.1f;
                    Config.enemieSpawnCount--;
                }
            }
        }
    }
}
