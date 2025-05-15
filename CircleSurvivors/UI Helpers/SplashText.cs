namespace CircleSurvivors.UI
{
    /// <summary>
    /// klassen som hanterar splashTexten
    /// </summary>
    public class SplashText
    {
        static readonly string[] splashTextsArray =
        {
            "I'm in love with the shape of you - by circle sheeran", 
            "I need to count how many splashes made after this", 
            "This splash text is {Shadow Slave chapter 360}",
            "Circle of life... and death, probably.", 
            "This isn't a drill! (it's a spiral)",
            "On edge? We don't have edges here.", 
            "Circle one: fight!... i mean round",
            "Tested on humans, not responsibly.", 
            "alternative title: Geometry wars", 
            "Rolling until further notice", 
            "I'm going aRound in circles",
            "You're on a roll!... or not",
            "No corners to hide behind",
            "Bravo 6, going circles.", 
            "Insert better pun here.", 
            "This text does nothing.", 
            "360 degrees of dangers", 
            "Also try Circle Diers", 
            "in circles since 2007",
            "esnälla ge mig ett A", 
            "Wheel of misfortune", 
            "Rotating regrets", 
            "Circle up buddy", 
            "Roundabout rage", 
            "Pop a wheelie", 
            "Axis of evil", 
        }; //21st

        /// <summary>
        /// hämtar en random Splash Text från en Array av strings
        /// </summary>
        /// <returns>en string som vi använder som Splash Text</returns>
        public static string GetSplashText()
        {
            Random randomSplashText = new Random();
            int splashText;
            splashText = randomSplashText.Next(0, splashTextsArray.Length);
            return splashTextsArray[splashText];
        }
    }
}
