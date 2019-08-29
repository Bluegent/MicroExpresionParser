﻿namespace ConsoleTester
{
    using System;
    using System.Threading;

    using RPGEngine.Core;
    using RPGEngine.Discord;
    using RPGEngine.GameInterface;
    using RPGEngine.Managers;

    class Program
    {
        static void Main(string[] args)
        {
            const int SLEEP_TIME = 1000;
            GameEngine engine = new GameEngine(new DiscordLogHelper(new ConsoleLogger()));
            engine.LoadConfigFromFiles();
            CommandManager.Instance.Engine = engine;
            Command cmd = new Command()  { UserId = 1, Name = "create",Args = new string[] { "John", "shonen_protag" } };
            Command cmd2 = new Command() { UserId = 2, Name = "create", Args = new string[] { "Putza", "shonen_protag" } };
            Command cmd3 = new Command() { UserId = 1, Name = "duel", Args = new string[] { "challenge", "Putza" } };
            Command cmd4 = new Command() { UserId = 2, Name = "duel", Args = new string[] { "accept"} };
            Command cmd5 = new Command() { UserId = 2, Name = "autoattack", Args = new string[]{} };
            Command cmd6 = new Command() { UserId = 1, Name = "cast", Args = new string[] { "attack"} };
            Command meCmd = new Command() { UserId = 2, Name = "me", Args = new string[]{}};
            Command meSkills = new Command() { UserId = 2, Name = "me", Args = new string[] { "skills" } };
            engine.EnqueueCommand(cmd);
            engine.EnqueueCommand(cmd2);
            engine.EnqueueCommand(cmd3);
            engine.EnqueueCommand(cmd4);
            engine.EnqueueCommand(cmd5);
            engine.EnqueueCommand(meCmd);
            engine.EnqueueCommand(cmd6);
            engine.EnqueueCommand(meSkills);
            MockTimer timer = (MockTimer)engine.GetTimer();
            while (true)
            {
                engine.Update();
                timer.ForceTick();
                Thread.Sleep(SLEEP_TIME);
            }
        }
    }
}
