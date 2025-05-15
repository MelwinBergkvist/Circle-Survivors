using CircleSurvivors.Core;
using CircleSurvivors.Interfaces;
using CircleSurvivors.UI_Helpers;
using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircleSurvivors.Gui
{
    /// <summary>
    /// hanterar pause screen
    /// </summary>
    public class PauseScreen : IGui
    {
        /// <summary>
        /// Visar en Pause screen
        /// </summary>
        public void Display(float deltaTime)
        {
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

            if (Config.isPaused)
            {
                Raylib.DrawRectangle(0, 0, Config.WindowSizeWidth, Config.WindowSizeHeight, new(110, 110, 110, 135));
                Helper.DrawCenteredText("Game is now paused!", Config.WindowSizeWidth / 2, (Config.WindowSizeHeight / 2 - Config.WindowSizeHeight / 4) / 2, 46, new(100, 25, 25));
                Helper.DrawCenteredText("Press P to unpause", Config.WindowSizeWidth / 2, (Config.WindowSizeHeight / 2 - Config.WindowSizeHeight / 8) / 2, 24, new(115, 115, 115));
            }
        }
    }
}
