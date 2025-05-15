using CircleSurvivors.Interfaces;
using CircleSurvivors.UI_Helpers;
using CircleSurvivors.Core;
using Raylib_cs;

namespace CircleSurvivors.Gui
{
    /// <summary>
    /// hanterar deathscreenen
    /// </summary>
    public class DeathScreen : IGui //implementerar gui interface
    {
        readonly float fadeInSpeed = 50f;
        float gameOverAlpha = 0f;
        Color fadeRed, fadeGray, fadeSkyBlue;

        /// <summary>
        /// displayar death screenen vid spelarens död
        /// ger också resart option
        /// </summary>
        /// <param name="deltaTime">tid</param>
        public void Display(float deltaTime)
        {
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
            Helper.DrawCenteredText("Game over, You lost!", Config.WindowSizeWidth / 2, Config.WindowSizeHeight / 4, 64, fadeRed);
            Helper.DrawCenteredText($"You stayed alive for {(int)Config.timeAliveMinutes} minutes and {(int)Config.timeAliveSeconds} seconds!", Config.WindowSizeWidth / 2, Config.WindowSizeHeight / 4 + 100, 16, fadeSkyBlue);
            Helper.DrawCenteredText($"You killed {Config.killCount} enemies during your run!", Config.WindowSizeWidth / 2, Config.WindowSizeHeight / 4 + 120, 16, fadeSkyBlue);
            Helper.DrawCenteredText($"You survived for {Config.wave - 1} waves!", Config.WindowSizeWidth / 2, Config.WindowSizeHeight / 4 + 140, 16, fadeSkyBlue);
            Helper.DrawCenteredText("Press R to restart", Config.WindowSizeWidth / 2, Config.WindowSizeHeight / 2 + Config.WindowSizeHeight / 4, 24, fadeGray);

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
