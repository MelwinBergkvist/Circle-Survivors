using System.Numerics;
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
            Raylib.DrawText(text, x - measureText, y, fontSize, color);
        } 
        /// <summary>
        /// tar fram Euclidean distansen och retunerar det som en vector2
        /// </summary>
        /// <param name="x">x1</param>
        /// <param name="x2">x2</param>
        /// <param name="y"><y1/param>
        /// <param name="y2">y2</param>
        /// <returns>Euclidean distansen som en vector 2</returns>
        public static Vector2 EuclideanVector2(ref float x, ref float x2, ref float y, ref float y2)
        {
            float dx = x2 - x;
            float dy = y2 - y;
            float distanceVectorEuc = MathF.Sqrt(dx * dx + dy * dy);
            return new Vector2(dx / distanceVectorEuc, dy / distanceVectorEuc);
        }
        /// <summary>
        /// tar fram Euclidean distansen och returnerar den som en float
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="x2">x2</param>
        /// <param name="y"><y/param>
        /// <param name="y2">y2</param>
        /// <returns>en tuple av dx/distance, dy/distance, och distansen</returns>
        public static (float dxDistance, float dyDistance, float distance) EuclideanFloat(ref float x, ref float x2, ref float y, ref float y2)
        {
            float dx = x2 - x;
            float dy = y2 - y;
            float distance = MathF.Sqrt(dx * dx + dy * dy);
            float dxDistance = dx / distance;
            float dyDistance = dy / distance;

            return (dxDistance, dyDistance, distance);
        }
    }
}
