using CircleSurvivors.Mechanics;
using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CircleSurvivors.Graphics
{
    public class GUI
    {
        public float gameOverAlpha = 0f;
        public float fadeInSpeed = 50f;
        readonly float fadeInSpeedStart = 100f;
        Color fadeRed, fadeGray, fadeSkyBlue;
        float timeAliveSeconds = 0f;
        float timeAliveMinutes = 0f;
        float startScreenAlpha = 0f;

        bool startFadeIn = false;
        bool hoveredStart;
        bool clickedStart;
        bool isTutorialHovered;

        public GUI() { }
        /// <summary>
        /// displayar death screenen vid spelarens död
        /// ger också resart option
        /// </summary>
        /// <param name="deltaTime"></param>
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
        /// <summary>
        /// displayar timern som visas vid toppen av skärmen
        /// </summary>
        /// <param name="deltaTime">tid</param>
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
        /// <summary>
        /// displayar start screenen vid börjar av spelet
        /// </summary>
        /// <param name="deltaTime">tid</param>
        public void StartScreen(float deltaTime)
        {
            if (!Config.startScreen)
            {
                Raylib.ClearBackground(Color.Black);
                Raylib.BeginDrawing();

                int startScreenText = Raylib.MeasureText("Welcome to Circle survivors!", 64);
                Raylib.DrawText("Welcome to Circle survivors!", Config.WindowSizeWidth / 2 - startScreenText / 2, Config.WindowSizeHeight / 8, 64, Color.White);

                Raylib.DrawRectangle(Config.WindowSizeWidth / 2 - 150, Config.WindowSizeHeight / 2 + Config.WindowSizeHeight / 4 - 38, 300, 100, Color.SkyBlue);
                Rectangle startButton = new Rectangle(Config.WindowSizeWidth / 2 - 150, Config.WindowSizeHeight / 2 + Config.WindowSizeHeight / 4 - 38, 300, 100);

                int startScreenBeginText = Raylib.MeasureText("Click here to begin!", 24);
                Raylib.DrawText("Click here to begin!", Config.WindowSizeWidth / 2 - startScreenBeginText / 2, Config.WindowSizeHeight / 2 + Config.WindowSizeHeight / 4, 24, Color.DarkBlue);

                Rectangle tutorialButton = new Rectangle(0, Config.WindowSizeHeight / 2, 120, 50);
                Raylib.DrawRectangle(0, Config.WindowSizeHeight / 2, 120, 50, Color.DarkGray);
                Raylib.DrawText("How to play?", 10, Config.WindowSizeHeight / 2 + 14, 16, Color.Black);

                isTutorialHovered = Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), tutorialButton);
                if (isTutorialHovered)
                {
                    //ritar om rectangeln i annan färg så man ser att den är hovered
                    Raylib.DrawRectangle(0, Config.WindowSizeHeight / 2, 120, 50, Color.Gray);
                    Raylib.DrawText("How to play?", 10, Config.WindowSizeHeight / 2 + 14, 16, Color.Black);

                    //tutorial så man vet vad man ska göra
                    Raylib.DrawRectangle(Config.WindowSizeWidth / 6 - 10, Config.WindowSizeHeight / 3 - 10, 320, 220, Color.DarkGray);
                    Raylib.DrawText("Welcome to Circle Survivors!", Config.WindowSizeWidth / 6, Config.WindowSizeHeight / 3, 16, Color.White);
                    Raylib.DrawText("You play as the green circle,", Config.WindowSizeWidth / 6, Config.WindowSizeHeight / 3 + 40, 16, Color.White);
                    Raylib.DrawText("you move using WASD or direction keys.", Config.WindowSizeWidth / 6, Config.WindowSizeHeight / 3 + 60, 16, Color.White);
                    Raylib.DrawText("The game automatically shoots for you.", Config.WindowSizeWidth / 6, Config.WindowSizeHeight / 3 + 100, 16, Color.White);
                    Raylib.DrawText("So you just need to stay alive.", Config.WindowSizeWidth / 6, Config.WindowSizeHeight / 3 + 120, 16, Color.White);
                    Raylib.DrawText("Enemies spawn in waves,", Config.WindowSizeWidth / 6, Config.WindowSizeHeight / 3 + 160, 16, Color.White);
                    Raylib.DrawText("At the end a wave you get a powerup.", Config.WindowSizeWidth / 6, Config.WindowSizeHeight / 3 + 180, 16, Color.White);
                }

                //checkcollisionpointrec kollar om vektoren getmouseposition är inom rektangel startButton vilken är en kopia av drawrectangle vi gjorde innan
                //om den är sann och left click är också nedklickad samtidigt så körs inte denna if satsen nåmer, om den inte är sann så skippar allt annat för "continue;"
                hoveredStart = Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), startButton);
                clickedStart = Raylib.IsMouseButtonPressed(MouseButton.Left);

                if (hoveredStart) //så man ser att den är hovered
                {
                    Raylib.DrawRectangle(Config.WindowSizeWidth / 2 - 150, Config.WindowSizeHeight / 2 + Config.WindowSizeHeight / 4 - 38, 300, 100, Color.Blue);
                    Raylib.DrawText("Click here to begin!", Config.WindowSizeWidth / 2 - startScreenBeginText / 2, Config.WindowSizeHeight / 2 + Config.WindowSizeHeight / 4, 24, Color.Black);
                    if (clickedStart)
                    {
                        startFadeIn = true; //initierar fade-in
                    }
                }

                if (startFadeIn)
                {
                    if (startScreenAlpha < 255)
                    {
                        startScreenAlpha += fadeInSpeedStart * deltaTime;
                        if (startScreenAlpha > 255) startScreenAlpha = 255;
                    }
                    Color fadeIn = Raylib.Fade(Color.White, startScreenAlpha / 255f);
                    Raylib.DrawRectangle(0, 0, Config.WindowSizeWidth, Config.WindowSizeHeight, fadeIn);
                    if (startScreenAlpha == 255) //när fade-in är klar (opaciteten är 100%) så säger vi att startscreenen är klar
                        Config.startScreen = true;
                }
                Raylib.EndDrawing();
            }
        }
        /// <summary>
        /// displayar alla onscreen stats, inkluderar killcount och wave count
        /// </summary>
        public void StatSheet()
        {
            //corner info
            Raylib.DrawText($"Kill count: {Config.killCount}", 0, 0, 28, Color.Red);
            Raylib.DrawText($"Wave: {Config.wave}", 0, 25, 28, Color.Red);

            //Stat sheet
            Raylib.DrawText($"bullet damage stat: {Config.bulletDamage}", 0, Config.WindowSizeHeight - 20, 12, Color.DarkGray);
            Raylib.DrawText($"bullet radius stat: {Config.bulletRadius}", 0, Config.WindowSizeHeight - 30, 12, Color.DarkGray);
            Raylib.DrawText($"bullet speed stat: {Config.bulletSpeed}", 0, Config.WindowSizeHeight - 40, 12, Color.DarkGray);
            Raylib.DrawText($"bullet cooldown stat: {Config.bulletCooldown}", 0, Config.WindowSizeHeight - 50, 12, Color.DarkGray);
            Raylib.DrawText($"player radius stat: {Config.player.radius}", 0, Config.WindowSizeHeight - 60, 12, Color.DarkGray);
            Raylib.DrawText($"player hitpoints stat: {Config.player.healthpoints}", 0, Config.WindowSizeHeight - 70, 12, Color.DarkGray);
            Raylib.DrawText($"player movement speed stat: {Config.player.movementSpeed}", 0, Config.WindowSizeHeight - 80, 12, Color.DarkGray);
            Raylib.DrawText($"stat sheet:", 0, Config.WindowSizeHeight - 90, 12, Color.DarkGray);
        }
    }
}
