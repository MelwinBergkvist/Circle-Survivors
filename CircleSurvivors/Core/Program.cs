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
using CircleSurvivors.UI;
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
        
        static void Main()
        {
            float deltaTime;
            Raylib.SetTargetFPS(60); //nästan som Thread.sleep(16); men bättre

            SpawnMechanics spawnMechs = new SpawnMechanics();
            ObjectHandler objectHandler = Config.objectHandler;
            GuiHandler guiHandler = new GuiHandler();
            SplashText splashText = new SplashText();

            Config.ResetGameState();

            Raylib.InitWindow(Config.WindowSizeWidth, Config.WindowSizeHeight, "Circle Survivors - " + SplashText.GetSplashText());
            while (!Raylib.WindowShouldClose()) //Game loop
            {
                deltaTime = Raylib.GetFrameTime();

                Raylib.ClearBackground(Color.White);
                Raylib.BeginDrawing(); // <- drawing confines beginning

                guiHandler.Display(deltaTime);
                if (Config.isStartScreen || Config.player.ShouldDespawn()) continue;
                objectHandler.CheckDespawn(deltaTime); //hanterar all collision och despawn checks
                spawnMechs.Spawn(deltaTime); //hanterar alla spawn mechanics

                Raylib.EndDrawing(); // <- drawing confines ending
            }
        }
    }
}
