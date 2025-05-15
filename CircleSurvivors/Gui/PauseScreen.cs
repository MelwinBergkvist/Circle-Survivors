using CircleSurvivors.Interfaces;
using CircleSurvivors.UI_Helpers;
using CircleSurvivors.Core;
using Raylib_cs;

namespace CircleSurvivors.Gui
{
    /// <summary>
    /// hanterar pause screen
    /// </summary>
    public class PauseScreen : IGui //implementerar gui interface
    {
        /// <summary>
        /// Visar en Pause screen
        /// </summary>
        public void Display(float deltaTime)
        {
            //pause och unpause switch med samma knapp
            if (Raylib.IsKeyPressed(KeyboardKey.P) && !Config.isAlreadyPaused) // pause
            {
                Config.isPaused = true;
                Config.isAlreadyPaused = true;
                Config.countTime = false;
            }
            else if (Raylib.IsKeyPressed(KeyboardKey.P) && Config.isAlreadyPaused) // unpause
            {
                Config.isPaused = false;
                Config.isAlreadyPaused = false;
                Config.countTime = true;
            }

            //om det är pausat visa en pause text
            if (Config.isPaused)
            {
                Raylib.DrawRectangle(0, 0, Config.WindowSizeWidth, Config.WindowSizeHeight, new(110, 110, 110, 135));
                Helper.DrawCenteredText("Game is now paused!", Config.WindowSizeWidth / 2, (Config.WindowSizeHeight / 2 - Config.WindowSizeHeight / 4) / 2, 46, new(100, 25, 25));
                Helper.DrawCenteredText("Press P to unpause", Config.WindowSizeWidth / 2, (Config.WindowSizeHeight / 2 - Config.WindowSizeHeight / 8) / 2, 24, new(115, 115, 115));
            }
        }
    }
}
