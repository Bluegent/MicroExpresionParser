﻿namespace MicroExpressionParser.Core
{
    public static class StringConstants
    {
        //Operators
        public const string PLUS_OP = "+";
        public const string MINUS_OP = "-";
        public const string MULITPLY_OP = "*";
        public const string POWER_OP = "^";
        public const string DIVIDE_OP = "/";
        public const string GREATER_OP = ">";
        public const string LESSER_OP = "<";
        public const string EQUAL_OP = "=";
        public const string NOT_OP = "!";

        //Functions
        public const string HARM_F = "HARM";
        public const string HEAL_F = "HEAL";
        public const string MAX_F = "MAX";
        public const string MIN_F = "MIN";
        public const string ABS_F = "ABS";
        public const string RANDOM_F = "RANDOM";
        public const string GET_PROP_F = "GET_PROP";
        public const string GET_PLAYERS_F = "GET_PLAYERS";
        public const string GET_ACTIVE_PLAYERS_F = "GET_ACTIVE_PLAYERS";
        public const string NON_NEG_F = "NON_NEG";
        public const string ARRAY_F = "ARRAY";
        public const string IF_F = "IF";
        public const string ARR_RANDOM_F = "ARR_RANDOM";
        public const string CHANCE_F = "CHANCE";
        public const string CAST_F = "CAST";

        public const char PARAM_SEPARATOR = ',';
        public const char LEFT_PAREN = '(';
        public const char RIGHT_PAREN = ')';
        public const char SPECIAL_CHAR = '$';

        public static readonly string TargetKeyword = SPECIAL_CHAR + "TARGET";

        public static readonly string CasterKeyword = SPECIAL_CHAR + "CASTER";

        public const char FUNCTION_SEPARATOR = ';';
    }
}
