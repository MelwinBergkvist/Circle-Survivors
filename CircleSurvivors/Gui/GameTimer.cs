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
    public class GameTimer : IGui
    {
        /// <summary>
        /// displayar timern som visas vid toppen av skärmen
        /// </summary>
        /// <param name="deltaTime">tid</param>
        public void Display(float deltaTime)
        {
            if (Config.countTime && !Config.isPaused)
                Config.timeAliveSeconds += deltaTime;
            else
            {
                if (Config.timeAliveSeconds < 10)
                {
                    if (Config.timeAliveMinutes < 10)
                        Helper.DrawCenteredText($"0{(int)Config.timeAliveMinutes}:0{(int)Config.timeAliveSeconds}", Config.WindowSizeWidth / 2, 20, 24, Color.Red);
                    else
                        Helper.DrawCenteredText($"{(int)Config.timeAliveMinutes}:0{(int)Config.timeAliveSeconds}", Config.WindowSizeWidth / 2, 20, 24, Color.Red);
                }
                else
                {
                    if (Config.timeAliveMinutes < 10)
                        Helper.DrawCenteredText($"0{(int)Config.timeAliveMinutes}:{(int)Config.timeAliveSeconds}", Config.WindowSizeWidth / 2, 20, 24, Color.Red);
                    else
                        Helper.DrawCenteredText($"{(int)Config.timeAliveMinutes}:{(int)Config.timeAliveSeconds}", Config.WindowSizeWidth / 2, 20, 24, Color.Red);
                }
            }
            if (Config.countTime && !Config.isPaused)
            {
                if (Config.timeAliveSeconds < 10)
                {
                    if (Config.timeAliveMinutes < 10)
                        Helper.DrawCenteredText($"0{(int)Config.timeAliveMinutes}:0{(int)Config.timeAliveSeconds}", Config.WindowSizeWidth / 2, 20, 24, Color.Black);
                    else
                        Helper.DrawCenteredText($"{(int)Config.timeAliveMinutes}:0{(int)Config.timeAliveSeconds}", Config.WindowSizeWidth / 2, 20, 24, Color.Black);
                }
                else
                {
                    if (Config.timeAliveMinutes < 10)
                        Helper.DrawCenteredText($"0{(int)Config.timeAliveMinutes}:{(int)Config.timeAliveSeconds}", Config.WindowSizeWidth / 2, 20, 24, Color.Black);
                    else
                        Helper.DrawCenteredText($"{(int)Config.timeAliveMinutes}:{(int)Config.timeAliveSeconds}", Config.WindowSizeWidth / 2, 20, 24, Color.Black);

                }
            }
            if (Config.timeAliveSeconds > 60)
            {
                Config.timeAliveSeconds = 0;
                Config.timeAliveMinutes += 1;
            }
        }
    }
}
