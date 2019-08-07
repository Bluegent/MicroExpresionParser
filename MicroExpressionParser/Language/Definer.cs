﻿namespace RPGEngine.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MicroExpressionParser;

    public class Definer
    {
        private static readonly  Definer Instance = new Definer();
        public Dictionary<string, Operator> Operators;
        public Dictionary<string, Function> Functions;
        private bool _initialized;

        public static Definer Get()
        {
            return Instance;
        }

        private Definer()
        {
            _initialized = false;
        }

        public bool IsSpecialChar(char c)
        {
            return c == Constants.PARAM_SEPARATOR || c == Constants.LEFT_PAREN || c == Constants.RIGHT_PAREN || IsOperator(char.ToString(c));
        }

        public bool IsSeparator(string str)
        {
            if (str.Length != 1)
                return false;
            if (str[0] == Constants.PARAM_SEPARATOR)
                return true;
            return false;
        }

        public bool IsFunction(string str)
        {
            return Functions.ContainsKey(str);
        }

        public bool IsOperator(string str)
        {
            return Operators.ContainsKey(str);
        }

        public bool IsLeftParen(string str)
        {
            if (str.Length != 1)
                return false;
            if (str[0] == Constants.LEFT_PAREN)
                return true;
            return false;
        }

        public bool IsRightParen(string str)
        {
            if (str.Length != 1)
                return false;
            if (str[0] == Constants.RIGHT_PAREN)
                return true;
            return false;
        }

        private void AddOperator(
            string character,
            int precedence,
            bool leftAssoc,
            Func<MeVariable[], Operator, MeVariable> operation,
            int opCount = 2)
        {
            Operator op = new Operator(character, precedence, leftAssoc, opCount);
            op.Operation = operation;
            Operators.Add(op.Character, op);
        }

        private void AddFunction(string name, Func<MeVariable[], Function, MeVariable> operation, int parameterCount = -1, bool[] executeInPlace = null)
        {
            Function func = new Function(name, parameterCount, executeInPlace);
            func.Operation = operation;
            Functions.Add(func.Name, func);
        }
        public void Init(IGameEngine engine)
        {
            if (_initialized)
                return;
            _initialized = true;

            Operators = new Dictionary<string, Operator>();
            Functions = new Dictionary<string, Function>();

            AddOperator(Constants.PLUS_OP, 1, true,
            (values, op) =>
            {
                op.ValidateParameters(values.Length);
                return values[0].ToDouble() + values[1].ToDouble();
            });

            AddOperator(Constants.MINUS_OP, 1, true,
            (values, op) =>
            {
                op.ValidateParameters(values.Length);
                return values[0].ToDouble() - values[1].ToDouble();
            });


            AddOperator(Constants.MULITPLY_OP, 2, true,
                (values, op) =>
                {
                    op.ValidateParameters(values.Length);
                    return values[0].ToDouble() * values[1].ToDouble();
                });

            AddOperator(Constants.POWER_OP, 3, true,
                (values, op) =>
                {
                    op.ValidateParameters(values.Length);
                    return Math.Pow(values[0].ToDouble(), values[1].ToDouble());
                });

            AddOperator(Constants.DIVIDE_OP, 2, true,
                (values, op) =>
                {
                    op.ValidateParameters(values.Length);
                    return values[0].ToDouble() / values[1].ToDouble();
                });

            AddOperator(Constants.NOT_OP, 2, true,
                (values, op) =>
                {
                    op.ValidateParameters(values.Length);
                    return !values[0].ToBoolean();
                }, 1);

            AddOperator(Constants.GREATER_OP, 0, true,
                (values, op) =>
                {
                    op.ValidateParameters(values.Length);
                    return values[0].ToDouble() > values[1].ToDouble();
                });
            AddOperator(Constants.LESSER_OP, 0, true,
                (values, op) =>
                {
                    op.ValidateParameters(values.Length);
                    return values[0].ToDouble() < values[1].ToDouble();
                });

            AddOperator(Constants.EQUAL_OP, 0, true,
                (values, op) =>
                {
                    op.ValidateParameters(values.Length);
                    return values[0].ToDouble() == values[1].ToDouble();
                });

            AddFunction(Constants.MAX_F,
                (values, func) =>
                {
                    func.ValidateParameters(values.Length);
                    double[] parameters = MeVariable.ToDoubleArray(values);
                    return parameters.Max();
                });

            AddFunction(Constants.MIN_F,
                (values, func) =>
                {
                    func.ValidateParameters(values.Length);
                    double[] parameters = MeVariable.ToDoubleArray(values);
                    return parameters.Min();
                });


            AddFunction(Constants.ABS_F,
                (values, func) =>
                {
                    func.ValidateParameters(values.Length);
                    return Math.Abs(values[0].ToDouble());
                }, 1);

            AddFunction(Constants.NON_NEG_F,
                (values, func) =>
                {
                    func.ValidateParameters(values.Length);
                    double value = values[0].ToDouble();
                    return value > 0 ? value : 0;
                }, 1);

            AddFunction(Constants.RANDOM_F,
                (values, func) =>
                {
                    func.ValidateParameters(values.Length);
                    return new Random().Next((int)values[0].ToDouble(), (int)values[1].ToDouble());
                }, 2);


            AddFunction(Constants.HARM_F,
                (values, func) =>
                {
                    func.ValidateParameters(values.Length);
                    Entity target = values[0].ToEntity();
                    Entity source = values[1].ToEntity();
                    DamageType damageType = values[2].ToDamageType();
                    double amount = values[3].ToDouble();
                    target.TakeDamage(amount, damageType, source);
                    return null;
                }, 4);

            AddFunction(Constants.HEAL_F,
                (values, func) =>
                {
                    func.ValidateParameters(values.Length);
                    Entity target = values[0].ToEntity();
                    Entity source = values[1].ToEntity();
                    double amount = values[2].ToDouble();
                    target.GetHealed(amount, source);
                    return null;
                }, 3);

            AddFunction(Constants.ARRAY_F,
                (values, func) =>
                {
                    func.ValidateParameters(values.Length);
                    return new MeVariable() { Type = VariableType.Array, Value = values };
                });

            AddFunction(Constants.GET_PLAYERS_F,
                (values, func) =>
                {
                    func.ValidateParameters(values.Length);
                    Entity[] players = engine.GetAllPlayers();
                    List<MeVariable> playerList = new List<MeVariable>();
                    foreach (Entity entity in players)
                    {
                        playerList.Add(entity);
                    }
                    return new MeVariable() { Value = playerList.ToArray(), Type = VariableType.Array };
                }, 0);

            AddFunction(Constants.GET_ACTIVE_PLAYERS_F,
                (values, func) =>
                {
                    func.ValidateParameters(values.Length);
                    //TODO: Implement retrieving ONLY active players
                    Entity[] players = engine.GetAllPlayers();
                    List<MeVariable> playerList = new List<MeVariable>();
                    foreach (Entity entity in players)
                    {
                        playerList.Add(entity);
                    }
                    return new MeVariable() { Value = playerList.ToArray(), Type = VariableType.Array };
                }, 0);

            AddFunction(Constants.GET_PROP_F,
                (values, func) =>
                {
                    func.ValidateParameters(values.Length);
                    Entity entity = values[0].ToEntity();
                    string prop = values[1].ToString();
                    return entity.GetProperty(prop).Value;
                }, 2);

            AddFunction(Constants.IF_F,
                (values, func) =>
                {
                    //IF(CONDITION,THEN,ELSE)
                    func.ValidateParameters(values.Length);
                    bool condition = values[0].ToBoolean();
                    if (condition)
                    {
                        return values[1].Execute();
                    }
                    else
                    {
                        return values[2].Execute();
                    }
                }, 3, new bool[] { true, false, false });
            AddFunction(Constants.ARR_RANDOM_F,
                (values, func) =>
                {
                    func.ValidateParameters(values.Length);
                    MeVariable[] input = values[0].ToArray();
                    int index = new Random().Next(0, input.Length);
                    return input[index];

                }, 1);

            AddFunction(Constants.CHANCE_F,
                (values, func) =>
                {
                    func.ValidateParameters(values.Length);
                    double chance = values[0].ToDouble() * 10;
                    int dice = new Random().Next(0, 1000);

                    return dice < chance;

                }, 1);

            AddFunction(Constants.CAST_F,
                (values, func) =>
                {
                    //CAST(CASTER,TARGET,SKILL)
                    func.ValidateParameters(values.Length);
                    Entity caster = values[0].ToEntity();
                    Entity target = values[1].ToEntity();
                    string skillKey = values[2].ToString();
                    caster.Cast(target, skillKey);
                    return null;

                }, 3);
            AddFunction(Constants.MOD_VALUE_F,
               (values, func) =>
               {
                   //MOD_VALUE(stat,Amount)
                   func.ValidateParameters(values.Length);
                   string stat = values[0].ToString();
                   double amount = values[1].ToDouble();
                   StatModifier mod = new StatModifier() { Amount = amount, StatKey = stat };
                   return new MeVariable { Type = VariableType.StatModifier, Value = mod };
               }, 2);
            AddFunction(Constants.APPLY_F,
               (values, func) =>
               {
                   //APPLYSTATUS(target,Source,status_key,duration,amounts)
                   Entity target = values[0].ToEntity();
                   Entity source = values[1].ToEntity();
                   StatusTemplate effect = engine.GetStatusByKey(values[2].ToString());
                   double duration = values[3].ToDouble();
                   double[] amounts = MeVariable.ToDoubleArray(values[4].ToArray());
                   func.ValidateParameters(values.Length);

                   //TODO:construct a statusEffect
                   return null;
               }, 5);

        }
    }
}