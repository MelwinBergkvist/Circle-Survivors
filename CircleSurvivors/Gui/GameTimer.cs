using CircleSurvivors.Interfaces;
using CircleSurvivors.UI_Helpers;
using CircleSurvivors.Core;
using Raylib_cs;

namespace CircleSurvivors.Gui
{
    public class GameTimer : IGui //implementerar gui interface
    {
        /// <summary>
        /// displayar timern som visas vid toppen av skärmen
        /// </summary>
        /// <param name="deltaTime">tid</param>
        public void Display(float deltaTime)
        {
            //om tiden ska ticka så räknar den på, annars ritar den timern i röd färg
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

            //om tiden ska ticka ritas timern i svart färg
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

            //konverterar 60 sec till 1 minut
            if (Config.timeAliveSeconds > 60)
            {
                Config.timeAliveSeconds = 0;
                Config.timeAliveMinutes += 1;
            }
        }
    }
}
