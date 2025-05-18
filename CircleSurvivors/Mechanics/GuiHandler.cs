using CircleSurvivors.Gui.Regular;
using CircleSurvivors.Gui.Special;
using CircleSurvivors.Interfaces;
using CircleSurvivors.Graphics;
using CircleSurvivors.Core;

namespace CircleSurvivors.Mechanics
{
    /// <summary>
    /// klassen som hanterar alla gui's
    /// </summary>
    public class GuiHandler
    {
        //listan
        public List<IGui> GuiList = new List<IGui>();

        //de som ska in i listan
        public GameTimer timer = new GameTimer();
        public StatSheet statsheet = new StatSheet();
        public PauseScreen pauseScreen = new PauseScreen();

        //de som inte ska in i listan men är forfarande gui som kommer användas
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
            if (Config.isStartScreen) //om startscreen inte är genomgjord visa den
            {
                startScreen.Display(deltaTime);
            }
            else if (Config.player.ShouldDespawn()) //om död visa death screen
            {
                deathScreen.Display(deltaTime);
            }
            else //annars kör alla andra gui's
            {
                foreach (var gui in GuiList)
                {
                    gui.Display(deltaTime);
                }
            }
        }
    }
}
