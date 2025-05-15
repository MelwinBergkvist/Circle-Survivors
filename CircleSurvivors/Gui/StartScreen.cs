using CircleSurvivors.Core;
using CircleSurvivors.Interfaces;
using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CircleSurvivors.Graphics
{
    /// <summary>
    /// hanterar alla GUI changes, Timer, death screen, start screen etc
    /// </summary>
    public class StartScreen : IGui
    {
        readonly float fadeInSpeed = 50f;
        float gameOverAlpha = 0f;

        Color fadeRed, fadeGray, fadeSkyBlue;
        float timeAliveSeconds = 0f;
        float timeAliveMinutes = 0f;
        float startScreenAlpha = 0f;

        readonly float fadeInSpeedStart = 100f;
        readonly Random random = new Random();

        List<StartScreen> startScreenEffectsList = new List<StartScreen>();
        float circleSpawnCooldownTimer = 0f;
        readonly float circleSpawnCooldown = 0.2f;
        readonly float circleY;
        float circleX;

        bool startFadeIn = false;
        bool hoveredStart;
        bool clickedStart;
        bool isTutorialHovered;

        public StartScreen() 
        {
            circleY = random.Next(0, Config.WindowSizeHeight);
            circleX = 0;
        }
        /// <summary>
        /// displayar start screenen vid börjar av spelet
        /// </summary>
        /// <param name="deltaTime">tid</param>
        public void Display(float deltaTime)
        {
            Raylib.ClearBackground(Color.Black);
            Raylib.BeginDrawing();

            circleSpawnCooldownTimer += deltaTime;
            if (circleSpawnCooldownTimer >= circleSpawnCooldown)
            {
                circleSpawnCooldownTimer = 0;
                StartScreenEffects(deltaTime);
            }
            UpdateStartScreenEffects(deltaTime);
            DrawStartScreenEffects();

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
                    Config.isStartScreen = false;
            }
            Raylib.EndDrawing();
        }
        /// <summary>
        /// skapar en ny circle för start screenen
        /// </summary>
        /// <param name="deltaTime"></param>
        public void StartScreenEffects(float deltaTime)
        {
            StartScreen circle = new StartScreen();
            startScreenEffectsList.Add(circle);
        }
        /// <summary>
        /// ritar circle som flyger förbi på start screenen
        /// </summary>
        public void DrawStartScreenEffects()
        {
            foreach (StartScreen circle in startScreenEffectsList)
                Raylib.DrawCircle((int)circle.circleX, (int)circle.circleY, 5, new(155,155,155));
        }
        /// <summary>
        /// updaterar circle effekterna som flyger förbi på start screenen
        /// </summary>
        /// <param name="deltaTime"></param>
        public void UpdateStartScreenEffects(float deltaTime)
        {
            foreach (StartScreen circle in startScreenEffectsList)
            {
                circle.circleX += deltaTime * 500;
                if (circle.circleX >= Config.WindowSizeWidth)
                    startScreenEffectsList.Remove(this);
            }
        }
    }
}
