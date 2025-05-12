/* Notes:
 * Kommentarerna kommer vara lite svengelska
 * 
 * Allt som ska ritas as raylibs måste vara inom Begin och End drawing
 * 
 * alla raylib.measuretext måste deklareras där de används, jag kunde ha deklararet det som tex "int Text;" och sedan ändrat värdet men jag ansåg att det ser fult och klunky ut
 * och inte i början av main, jag vet inte varför men det fungerade
 * inte när jag testade det
 * 
 * Min standard:
 * Alla interfaces startar med I och sen Stor bokstav
 * Alla classes Stor bokstav
 * Alla functioner Stor bokstav
 * Alla datatyper camelCase, första bokstaven liten
 * Alla listor ska ha List i slutet av namnet
*/
using CircleSurvivors.Entities;
using CircleSurvivors.Graphics;
using CircleSurvivors.Interfaces;
using CircleSurvivors.Mechanics;
using Raylib_cs; //Initierar Raylibs library, måste göras på alla .cs

namespace CircleSurvivors.Core
{
    /// <summary>
    /// Main klassen som körs när programmet öppnas
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// initialiserar spelet och kör allt som behövs för det
        /// </summary>
        /// <param name="args"></param>
        static void Main()
        {
            float deltaTime;
            Raylib.SetTargetFPS(60); //nästan som Thread.sleep(16); men bättre

            SpawnMechanics spawnMechs = new SpawnMechanics();
            ObjectHandler objectHandler = Config.objectHandler;
            GUI gui = new GUI();

            Config.ResetGameState();

            Raylib.InitWindow(Config.WindowSizeWidth, Config.WindowSizeHeight, "Circle Survivors - " + GUI.GetSplashText());
            while (!Raylib.WindowShouldClose()) //Game loop
            {
                deltaTime = Raylib.GetFrameTime();

                if (Config.player.ShouldDespawn())
                {
                    gui.DeathScreen(deltaTime); //visar death screen
                    continue;
                }

                if (!Config.startScreen)
                {
                    gui.StartScreen(deltaTime); //visar start screen
                    continue;
                }

                Raylib.ClearBackground(Color.White);
                Raylib.BeginDrawing(); // <- drawing confines beginning

                spawnMechs.Spawn(deltaTime); //hanterar alla spawn mechanics
                objectHandler.CheckDespawnAndCollision(deltaTime); //hanterar all collision och despawn checks
                GUI.StatSheet(); //displayar alla player stats
                gui.Timer(deltaTime); //displayar timern

                Raylib.EndDrawing(); // <- drawing confines ending
            }
        }
    }
}
