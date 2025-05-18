using CircleSurvivors.Mechanics;
using CircleSurvivors.UI;
using Raylib_cs; //Initierar Raylibs library, måste göras på alla .cs (som använder raylib functioner)
/* Notes:
 * Kommentarerna kommer vara lite svengelska
 * 
 * Allt som ska ritas as raylibs måste vara inom Begin och End drawing
 * 
 * All Raylib. var hittade antingen genom att jag sökte på dem eller från https://www.raylib.com/cheatsheet/cheatsheet.html
 * All Raymath var från https://www.raylib.com/cheatsheet/raymath_cheatsheet.html 
 * 
 * Min standard:
 * Alla interfaces startar med I och sen Stor bokstav
 * Alla classes Stor bokstav
 * Alla functioner Stor bokstav
 * Alla datatyper camelCase, första bokstaven liten
 * Alla listor ska ha List i slutet av namnet
*/

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
            float deltaTime; //Tiden mellan frames, delta tiden
            Raylib.SetTargetFPS(60); //nästan som Thread.sleep(16); men bättre
            Image icon = Raylib.LoadImage("Images\\CSI.png");

            //object instantiering
            SpawnMechanics spawnMechs = new SpawnMechanics();
            ObjectHandler objectHandler = Config.objectHandler;
            GuiHandler guiHandler = new GuiHandler();
            SplashText splashText = new SplashText();

            Config.ResetGameState();

            Raylib.InitWindow(Config.WindowSizeWidth, Config.WindowSizeHeight, "Circle Survivors - " + SplashText.GetSplashText());
            Raylib.SetWindowIcon(icon);
            while (!Raylib.WindowShouldClose()) //Game loop
            {
                deltaTime = Raylib.GetFrameTime();
                Raylib.ClearBackground(Color.White);

                Raylib.BeginDrawing(); // <- drawing confines beginning

                guiHandler.Display(deltaTime); //hanterar alla gui's
                if (Config.isStartScreen || Config.player.ShouldDespawn()) continue; //om startScreen eller deathScreen, kör inte det andra
                objectHandler.CheckDespawn(deltaTime); //hanterar alla despawn checks
                spawnMechs.Spawn(deltaTime); //hanterar alla spawn mechanics

                Raylib.EndDrawing(); // <- drawing confines ending
            }
        }
    }
}