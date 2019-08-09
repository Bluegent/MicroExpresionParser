﻿namespace RPGEngine.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MicroExpressionParser;
    using MicroExpressionParser.Core;

    using RPGEngine.Language;

    public interface IGameEngine
    {
        void AddPlayer(Entity entity);

        void AddEnemy(Entity entity);

        void AddDamageType(DamageType type);
        Entity[] GetAllPlayers();

        DamageType GeDamageType(string key);
        Entity GetEntityByKey(string key);
        StatusTemplate GetStatusByKey(string key);
        void AddStatus(StatusTemplate status, string key);
        ITimer GetTimer();
        Sanitizer GetSanitizer();

    }
    public class GameEngine : IGameEngine
    {
        public Dictionary<string, Entity> Players { get; }
        public Dictionary<string, Entity> Enemies { get; }
        private Dictionary<string, DamageType> DamageTypes { get; }
        private Sanitizer Sanit { get; set; }

        public ITimer Timer { get; set; }
        private Dictionary<string,StatusTemplate> statuses;
        public GameEngine()
        {
            Definer.Get().Init(this);
            Players = new Dictionary<string, Entity>();
            Enemies = new Dictionary<string, Entity>();
            DamageTypes = new Dictionary<string, DamageType>();
            Timer = new MockTimer();
            Sanit = new Sanitizer(this);
            statuses = new Dictionary<string, StatusTemplate>();
     
        }

        public void AddPlayer(Entity entity)
        {
            Console.WriteLine($"Adding player with key {entity.Key}");
            Players.Add(entity.Key, entity);
        }

        public void AddEnemy(Entity entity)
        {
            Enemies.Add(entity.Key, entity);
        }

        public void AddDamageType(DamageType type)
        {
            DamageTypes.Add(type.Key,type);
        }

        public Entity[] GetAllPlayers()
        {
            return Players.Values.ToArray();
        }

        public DamageType GeDamageType(string key)
        {
            return DamageTypes.ContainsKey(key) ? DamageTypes[key] : null;
        }

        public Entity GetEntityByKey(string key)
        {
            if(Players.ContainsKey(key)) 
                return Players[key];
            if(Enemies.ContainsKey(key))
                return Enemies[key];
            return null;
        }

        public StatusTemplate GetStatusByKey(string key)
        {
            return statuses.ContainsKey(key)?statuses[key]:null;
        }

        public ITimer GetTimer()
        {
            return Timer;
        }

        public Sanitizer GetSanitizer()
        {
            return Sanit;
        }

        public void AddStatus(StatusTemplate status, string key)
        {
            if (!statuses.ContainsKey(key))
                statuses.Add(key, status);
            else
                statuses[key] = status;
        }
    }
}
