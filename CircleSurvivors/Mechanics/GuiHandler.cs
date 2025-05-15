using CircleSurvivors.Core;
using CircleSurvivors.Graphics;
using CircleSurvivors.Gui;
using CircleSurvivors.Interfaces;

namespace CircleSurvivors.Mechanics
{
    /// <summary>
    /// klassen som hanterar alla gui's
    /// </summary>
    public class GuiHandler
    {
        public List<IGui> GuiList = new List<IGui>();

        public GameTimer timer = new GameTimer();
        public StatSheet statsheet = new StatSheet();
        public PauseScreen pauseScreen = new PauseScreen();

        public StartScreen startScreen = new StartScreen();
        public DeathScreen deathScreen = new DeathScreen();

        /// <summary>
        /// lägger till Gui's i GuiList
        /// </summary>
        public GuiHandler() 
        {
            GuiList.Add(timer);
            GuiList.Add(statsheet);
            GuiList.Add(pauseScreen);
        }
        /// <summary>
        /// Ritar alla gui's
        /// </summary>
        /// <param name="deltaTime"></param>
        public void Display(float deltaTime)
        {
            if (Config.isStartScreen)
            {
                startScreen.Display(deltaTime);
            }
            else if (Config.player.ShouldDespawn())
            {
                deathScreen.Display(deltaTime);
            }
            else
            {
                foreach (var gui in GuiList)
                {
                    gui.Display(deltaTime);
                }
            }
        }
    }
}
