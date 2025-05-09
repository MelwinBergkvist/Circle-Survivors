using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CircleSurvivors
{
    public class GUI
    {
        public float gameOverAlpha;
        public float fadeInSpeed;
        Color fadeRed, fadeGray, fadeSkyBlue;
        float timeAliveSeconds = 0f;
        float timeAliveMinutes = 0f;

        public GUI() 
        {
            gameOverAlpha = 0f;
            fadeInSpeed = 50f;
        }
        public void DeathScreen(float deltaTime)
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
            int endTextWidth = Raylib.MeasureText("Game over, You lost!", 64);
            //x post räknas från början av texten, så vi kör en measureText så vi kan centrera texten
            Raylib.DrawText("Game over, You lost!", Config.WindowSizeWidth / 2 - endTextWidth / 2, Config.WindowSizeHeight / 4, 64, fadeRed);

            int timeAliveText = Raylib.MeasureText($"You stayed alive for {(int)timeAliveMinutes} minutes and {(int)timeAliveSeconds} seconds!", 16);
            Raylib.DrawText($"You stayed alive for {(int)timeAliveMinutes} minutes and {(int)timeAliveSeconds} seconds!", Config.WindowSizeWidth / 2 - timeAliveText / 2, Config.WindowSizeHeight / 4 + 100, 16, fadeSkyBlue);
            int killCountText = Raylib.MeasureText($"You killed {Config.killCount} enemies during your run!", 16);
            Raylib.DrawText($"You killed {Config.killCount} enemies during your run!", Config.WindowSizeWidth / 2 - killCountText / 2, Config.WindowSizeHeight / 4 + 120, 16, fadeSkyBlue);
            int waveText = Raylib.MeasureText($"You survived for {Config.wave - 1} waves!", 16);
            Raylib.DrawText($"You survived for {Config.wave - 1} waves!", Config.WindowSizeWidth / 2 - waveText / 2, Config.WindowSizeHeight / 4 + 140, 16, fadeSkyBlue);


            int restartTextWidth = Raylib.MeasureText("Press R to restart", 24);
            Raylib.DrawText("Press R to restart", Config.WindowSizeWidth / 2 - restartTextWidth / 2, Config.WindowSizeHeight / 2 + Config.WindowSizeHeight / 4, 24, fadeGray);
            if (Raylib.IsKeyPressed(KeyboardKey.R)) // resettar allting
            {
                Config.ResetGameState();
                timeAliveMinutes = 0;
                timeAliveSeconds = 0;
            }
            else
            {
                Raylib.EndDrawing();
            }
        }
        public void Timer(float deltaTime)
        {
            int timeMeasureType1 = Raylib.MeasureText($"0{(int)timeAliveMinutes}:0{(int)timeAliveSeconds}", 16);
            int timeMeasureType2 = Raylib.MeasureText($"{(int)timeAliveMinutes}:0{(int)timeAliveSeconds}", 16);
            int timeMeasureType3 = Raylib.MeasureText($"0{(int)timeAliveMinutes}:{(int)timeAliveSeconds}", 16);
            int timeMeasureType4 = Raylib.MeasureText($"{(int)timeAliveMinutes}:{(int)timeAliveSeconds}", 16);
            if (Config.countTime)
                timeAliveSeconds += deltaTime;
            else
            {
                if (timeAliveSeconds < 10)
                {
                    if (timeAliveMinutes < 10)
                        Raylib.DrawText($"0{(int)timeAliveMinutes}:0{(int)timeAliveSeconds}", Config.WindowSizeWidth / 2 - timeMeasureType1 / 2, 20, 24, Color.Red);
                    else
                        Raylib.DrawText($"{(int)timeAliveMinutes}:0{(int)timeAliveSeconds}", Config.WindowSizeWidth / 2 - timeMeasureType2 / 2, 20, 24, Color.Red);
                }
                else
                {
                    if (timeAliveMinutes < 10)
                        Raylib.DrawText($"0{(int)timeAliveMinutes}:{(int)timeAliveSeconds}", Config.WindowSizeWidth / 2 - timeMeasureType3 / 2, 20, 24, Color.Red);
                    else
                        Raylib.DrawText($"{(int)timeAliveMinutes}:{(int)timeAliveSeconds}", Config.WindowSizeWidth / 2 - timeMeasureType4 / 2, 20, 24, Color.Red);
                }
            }
            if (Config.countTime)
            {
                if (timeAliveSeconds < 10)
                {
                    if (timeAliveMinutes < 10)
                        Raylib.DrawText($"0{(int)timeAliveMinutes}:0{(int)timeAliveSeconds}", Config.WindowSizeWidth / 2 - timeMeasureType1 / 2, 20, 24, Color.Black);
                    else
                        Raylib.DrawText($"{(int)timeAliveMinutes}:0{(int)timeAliveSeconds}", Config.WindowSizeWidth / 2 - timeMeasureType2 / 2, 20, 24, Color.Black);
                }
                else
                {
                    if (timeAliveMinutes < 10)
                        Raylib.DrawText($"0{(int)timeAliveMinutes}:{(int)timeAliveSeconds}", Config.WindowSizeWidth / 2 - timeMeasureType3 / 2, 20, 24, Color.Black);
                    else
                        Raylib.DrawText($"{(int)timeAliveMinutes}:{(int)timeAliveSeconds}", Config.WindowSizeWidth / 2 - timeMeasureType4 / 2, 20, 24, Color.Black);

                }
            }
            if (timeAliveSeconds > 60)
            {
                timeAliveSeconds = 0;
                timeAliveMinutes += 1;
            }
        }
    }
}
