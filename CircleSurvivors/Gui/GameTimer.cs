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
    public class GameTimer : IGui
    {
        /// <summary>
        /// displayar timern som visas vid toppen av skärmen
        /// </summary>
        /// <param name="deltaTime">tid</param>
        public void Display(float deltaTime)
        {
            int timeMeasureType1 = Raylib.MeasureText($"0{(int)Config.timeAliveMinutes}:0{(int)Config.timeAliveSeconds}", 16);
            int timeMeasureType2 = Raylib.MeasureText($"{(int)Config.timeAliveMinutes}:0{(int)Config.timeAliveSeconds}", 16);
            int timeMeasureType3 = Raylib.MeasureText($"0{(int)Config.timeAliveMinutes}:{(int)Config.timeAliveSeconds}", 16);
            int timeMeasureType4 = Raylib.MeasureText($"{(int)Config.timeAliveMinutes}:{(int)Config.timeAliveSeconds}", 16);
            if (Config.countTime && !Config.isPaused)
                Config.timeAliveSeconds += deltaTime;
            else
            {
                if (Config.timeAliveSeconds < 10)
                {
                    if (Config.timeAliveMinutes < 10)
                        Raylib.DrawText($"0{(int)Config.timeAliveMinutes}:0{(int)Config.timeAliveSeconds}", Config.WindowSizeWidth / 2 - timeMeasureType1 / 2, 20, 24, Color.Red);
                    else
                        Raylib.DrawText($"{(int)Config.timeAliveMinutes}:0{(int)Config.timeAliveSeconds}", Config.WindowSizeWidth / 2 - timeMeasureType2 / 2, 20, 24, Color.Red);
                }
                else
                {
                    if (Config.timeAliveMinutes < 10)
                        Raylib.DrawText($"0{(int)Config.timeAliveMinutes}:{(int)Config.timeAliveSeconds}", Config.WindowSizeWidth / 2 - timeMeasureType3 / 2, 20, 24, Color.Red);
                    else
                        Raylib.DrawText($"{(int)Config.timeAliveMinutes}:{(int)Config.timeAliveSeconds}", Config.WindowSizeWidth / 2 - timeMeasureType4 / 2, 20, 24, Color.Red);
                }
            }
            if (Config.countTime && !Config.isPaused)
            {
                if (Config.timeAliveSeconds < 10)
                {
                    if (Config.timeAliveMinutes < 10)
                        Raylib.DrawText($"0{(int)Config.timeAliveMinutes}:0{(int)Config.timeAliveSeconds}", Config.WindowSizeWidth / 2 - timeMeasureType1 / 2, 20, 24, Color.Black);
                    else
                        Raylib.DrawText($"{(int)Config.timeAliveMinutes}:0{(int)Config.timeAliveSeconds}", Config.WindowSizeWidth / 2 - timeMeasureType2 / 2, 20, 24, Color.Black);
                }
                else
                {
                    if (Config.timeAliveMinutes < 10)
                        Raylib.DrawText($"0{(int)Config.timeAliveMinutes}:{(int)Config.timeAliveSeconds}", Config.WindowSizeWidth / 2 - timeMeasureType3 / 2, 20, 24, Color.Black);
                    else
                        Raylib.DrawText($"{(int)Config.timeAliveMinutes}:{(int)Config.timeAliveSeconds}", Config.WindowSizeWidth / 2 - timeMeasureType4 / 2, 20, 24, Color.Black);

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
