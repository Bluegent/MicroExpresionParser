﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPGEngine.Core
{
    using MicroExpressionParser;

    using RPGEngine.Parser;

    public class DamageType
    {
        public string Key { get; set; }
        public MeNode Mitigation { get; set; }

        public MeNode Dodge { get; set; }
        public MeNode CriticalChance { get; set; }
        public MeNode CriticalModifier { get; set; }

        private IGameEngine _engine;

        public DamageType(IGameEngine engine, string key, string mitigation, string dodge, string crit, string critmod)
        {
            _engine = engine;
            Key = key;
            Mitigation = mitigation != null ? TreeConverter.Build(mitigation, engine) : null;
            Dodge = dodge != null ? TreeConverter.Build(dodge, engine) : null;
            CriticalChance = crit != null ? TreeConverter.Build(crit, engine) : null;
            CriticalModifier = critmod != null ? TreeConverter.Build(critmod, engine) : null;
        }

        public bool GetDodge(Entity source, Entity target)
        {
            return Dodge != null && Utils.Utility.Chance(_engine.GetSanitizer().SanitizeSkillEntities(Dodge, source, target).Resolve().Value.ToDouble());
        }

        public double GetMitigatedAmount(double amount, Entity source, Entity target)
        {
            bool crited;
            if (CriticalChance != null)
            {
                MeNode resolvedCritChance = _engine.GetSanitizer().SanitizeSkillEntities(CriticalChance, source, target)
                    .Resolve();

                crited = Utils.Utility.Chance(resolvedCritChance.Value.ToDouble());
            }
            else
            {
                crited = false;
            }

            double mutliplier = 1.0;
            if (crited)
            {
                if(CriticalModifier!=null)
                    mutliplier = _engine.GetSanitizer().SanitizeSkillEntities(CriticalModifier, source, target)
                    .Resolve().Value.ToDouble();
            }

            double finalAmount = mutliplier * amount;
            if (Mitigation != null)
            {
                MeNode mitigation = _engine.GetSanitizer().SanitizeMitigation(Mitigation, target, source, finalAmount)
                    .Resolve();
                return mitigation.Value.ToDouble();
            }
            else
            {
                return finalAmount;
            }

        }
    }
}
