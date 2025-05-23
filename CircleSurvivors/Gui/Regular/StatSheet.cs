﻿using CircleSurvivors.Interfaces;
using CircleSurvivors.Core;
using Raylib_cs;

namespace CircleSurvivors.Gui.Regular
{
    /// <summary>
    /// hanterar stat sheets
    /// </summary>
    public class StatSheet : IGui //implementerar gui interface
    {
        /// <summary>
        /// displayar alla onscreen stats, inkluderar killcount och wave count
        /// </summary>
        public void Display(float deltaTime)
        {
            //corner info
            Raylib.DrawText($"Kill count: {Config.killCount}", 0, 0, 28, Color.Red);
            Raylib.DrawText($"Wave: {Config.wave}", 0, 25, 28, Color.Red);

            //Stat sheet
            Raylib.DrawText($"bullet damage stat: {Config.bulletDamage}", 0, Config.WindowSizeHeight - 20, 12, Color.DarkGray);
            Raylib.DrawText($"bullet radius stat: {Config.bulletRadius}", 0, Config.WindowSizeHeight - 30, 12, Color.DarkGray);
            Raylib.DrawText($"bullet speed stat: {Config.bulletSpeed}", 0, Config.WindowSizeHeight - 40, 12, Color.DarkGray);
            Raylib.DrawText($"bullet cooldown stat: {Config.bulletCooldown}", 0, Config.WindowSizeHeight - 50, 12, Color.DarkGray);
            Raylib.DrawText($"player radius stat: {Config.player.radius}", 0, Config.WindowSizeHeight - 60, 12, Color.DarkGray);
            Raylib.DrawText($"player hitpoints stat: {Config.player.healthpoints}", 0, Config.WindowSizeHeight - 70, 12, Color.DarkGray);
            Raylib.DrawText($"player movement speed stat: {Config.player.movementSpeed}", 0, Config.WindowSizeHeight - 80, 12, Color.DarkGray);
            Raylib.DrawText($"stat sheet:", 0, Config.WindowSizeHeight - 90, 12, Color.DarkGray);
        }
    }
}
