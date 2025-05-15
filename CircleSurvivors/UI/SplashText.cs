using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircleSurvivors.UI
{
    public class SplashText
    {
        static readonly string[] splashTextsArray =
        {
            "Bravo 6, going circles.", "Circle of life... and death, probably.", "Circle up buddy", "This isn't a drill! (it's a spiral)",
            "I'm in love with the shape of you - by circle sheeran", "On edge? We don't have edges here.", "Pop a wheelie", "I'm going aRound in circles",
            "alternative title: Geometry wars", "Also try Circle Diers", "360 degrees of dangers", "Axis of evil", "No corners to hide behind",
            "Roundabout rage", "Wheel of misfortune", "esnälla ge mig ett A", "Rotating regrets", "Circle one: fight!... i mean round",
            "I need to count how many splashes made after this", "Rolling until further notice", "You're on a roll!... or not",
            "Insert better pun here.", "This text does nothing.", "in circles since 2007",
            "Tested on humans, not responsibly.", "This splash text is {Shadow Slave chapter 360}"
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
