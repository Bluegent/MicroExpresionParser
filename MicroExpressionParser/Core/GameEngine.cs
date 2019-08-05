﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MicroExpressionParser
{
    using System.Dynamic;

    public interface IGameEngine
    {
        void AddPlayer(Entity entity);

        void AddEnemy(Entity entity);
        Entity[] GetAllPlayers();

        DamageType GeDamageType(string key);
        Entity GetEntityByKey(string key);
        StatusTemplate GetStatusByKey(string key);
    }
    public class GameEngine : IGameEngine
    {
        public Dictionary<string, Entity> Players { get; }
        public Dictionary<string, Entity> Enemies { get; }
        private Dictionary<string, DamageType> DamageTypes { get; }
        public GameEngine()
        {
            Players = new Dictionary<string, Entity>();
            Enemies = new Dictionary<string, Entity>();
            DamageTypes = new Dictionary<string, DamageType>();
            DamageTypes.Add("P", new DamageType() { Key = "P", MitigationFormula = "NON_NEG($VALUE - GET_PROP($TARGET, DEF))" });
            DamageTypes.Add("M", new DamageType() { Key = "M", MitigationFormula = "NON_NEG($VALUE - GET_PROP($TARGET, MDEF))" });
            DamageTypes.Add("T", new DamageType() { Key = "T", MitigationFormula = "$VALUE" });
        }

        public void AddPlayer(Entity entity)
        {
            Players.Add(entity.Key, entity);
        }

        public void AddEnemy(Entity entity)
        {
            Enemies.Add(entity.Key, entity);
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
    }
}
