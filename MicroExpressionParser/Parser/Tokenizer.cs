﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MicroExpressionParser
{
    public enum TokenType
    {
        Variable, LeftParen, RightParen, Separator, Function, Operator
    }

    public class Token
    {
        public TokenType Type { get; set; }
        public string Value { get; set; }
        public static TokenType GetType(string str)
        {
            if (ParserConstants.IsFunction(str))
                return TokenType.Function;
            if (ParserConstants.IsOperator(str))
                return TokenType.Operator;
            if (ParserConstants.IsLeftParen(str))
                return TokenType.LeftParen;
            if (ParserConstants.IsRightParen(str))
                return TokenType.RightParen;
            if (ParserConstants.IsSeparator(str))
                return TokenType.Separator;
            return TokenType.Variable;
        }
        public Token(string value)
        {
            Value = value;
            Type = GetType(value);
        }
    }

    public class Tokenizer
    {

        private static string Sanitize(string expression)
        {
            return expression.Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("\t", "").Trim();
        }

        public static Token[] Tokenize(string expression)
        {
            List<Token> result = new List<Token>();
            string sanitized = Sanitize(expression);            
            string current = "";
            foreach (char c in sanitized)
            {
                if (!ParserConstants.IsSpecialChar(c))
                {
                    current += c;
                }
                else if(c == '-')
                {
                    if (current.Length != 0)
                    {
                        Token testToken = new Token(current);
                        if (testToken.Type == TokenType.Variable)
                        {
                            result.Add(testToken);
                            current = "";
                        }
                    }
                    Token prev = result.Last();
                    if (prev.Type == TokenType.Function)
                        throw new Exception("Found minus after function with no (, function: " + prev.Value);
                    else if (prev.Type == TokenType.Variable || prev.Type == TokenType.RightParen)
                    {
                        result.Add(new Token(char.ToString(c)));
                    }
                    else
                    {
                        
                        current = "-";
                    }
                }
                else
                {
                    if (current.Length != 0)
                    {
                        result.Add(new Token(current));
                        current = "";                       
                    }
                    result.Add(new Token(char.ToString(c)));
                }
            }
            if (current.Length != 0)
                result.Add(new Token(current));
            return result.ToArray();
        }
    }
}
