using CircleSurvivors.Core;
using CircleSurvivors.Interfaces;
using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircleSurvivors.Gui
{
    /// <summary>
    /// hanterar deathscreenen
    /// </summary>
    public class DeathScreen
    {
        readonly float fadeInSpeed = 50f;
        float gameOverAlpha = 0f;
        Color fadeRed, fadeGray, fadeSkyBlue;

        /// <summary>
        /// displayar death screenen vid spelarens död
        /// ger också resart option
        /// </summary>
        /// <param name="deltaTime"></param>
        public void Display(float deltaTime)
        {
            if (Config.isStartScreen)
                return;
            
            //fade in effect för you lose screen
            if (gameOverAlpha < 255)
            {
                gameOverAlpha += fadeInSpeed * deltaTime;
                if (gameOverAlpha > 255) gameOverAlpha = 255;
            }
            fadeRed = Raylib.Fade(Color.Red, gameOverAlpha / 255f);
            fadeGray = Raylib.Fade(Color.DarkGray, gameOverAlpha / 255f);
            fadeSkyBlue = Raylib.Fade(Color.SkyBlue, gameOverAlpha / 255f);
            //raylib vill ha mellan 0-1 medan vi kör mellan 0-255 så vi kör /255 för att göra raylibs glad
            //raylibs inbyggda fade in

            Raylib.ClearBackground(Color.Gray);
            int endTextWidth = Raylib.MeasureText("Game over, You lost!", 64);
            //x post räknas från början av texten, så vi kör en measureText så vi kan centrera texten
            Raylib.DrawText("Game over, You lost!", Config.WindowSizeWidth / 2 - endTextWidth / 2, Config.WindowSizeHeight / 4, 64, fadeRed);

            int timeAliveText = Raylib.MeasureText($"You stayed alive for {(int)Config.timeAliveMinutes} minutes and {(int)Config.timeAliveSeconds} seconds!", 16);
            Raylib.DrawText($"You stayed alive for {(int)Config.timeAliveMinutes} minutes and {(int)Config.timeAliveSeconds} seconds!", Config.WindowSizeWidth / 2 - timeAliveText / 2, Config.WindowSizeHeight / 4 + 100, 16, fadeSkyBlue);
            int killCountText = Raylib.MeasureText($"You killed {Config.killCount} enemies during your run!", 16);
            Raylib.DrawText($"You killed {Config.killCount} enemies during your run!", Config.WindowSizeWidth / 2 - killCountText / 2, Config.WindowSizeHeight / 4 + 120, 16, fadeSkyBlue);
            int waveText = Raylib.MeasureText($"You survived for {Config.wave - 1} waves!", 16);
            Raylib.DrawText($"You survived for {Config.wave - 1} waves!", Config.WindowSizeWidth / 2 - waveText / 2, Config.WindowSizeHeight / 4 + 140, 16, fadeSkyBlue);


            int restartTextWidth = Raylib.MeasureText("Press R to restart", 24);
            Raylib.DrawText("Press R to restart", Config.WindowSizeWidth / 2 - restartTextWidth / 2, Config.WindowSizeHeight / 2 + Config.WindowSizeHeight / 4, 24, fadeGray);
            if (Raylib.IsKeyPressed(KeyboardKey.R)) // resettar allting
            {
                Config.ResetGameState();
                Config.timeAliveMinutes = 0;
                Config.timeAliveSeconds = 0;
            }
            else
            {
                Raylib.EndDrawing();
            }
        }
    }
}
