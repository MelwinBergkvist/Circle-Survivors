using Raylib_cs;

namespace CircleSurvivors.UI_Helpers
{
    /// <summary>
    /// class för helper methods
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// Automatiskt centrerar texten man vill rita
        /// </summary>
        /// <param name="text">texten man vill skriva</param>
        /// <param name="x">x positionen</param>
        /// <param name="y">y positionen</param>
        /// <param name="fontSize">text storleken</param>
        /// <param name="color">färgen av texten</param>
        public static void DrawCenteredText(String text, int x, int y, int fontSize, Color color)
        {
            int measureText = (Raylib.MeasureText(text, fontSize))/2;
            
            
            (text, x - measureText, y, fontSize, color);
        } 
    }
}
