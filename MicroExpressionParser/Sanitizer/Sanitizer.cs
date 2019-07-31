﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MicroExpressionParser.Sanitizer
{
    using System.Runtime.CompilerServices;

    public class Sanitizer
    {
        private IGameEngine _engine;

        public Sanitizer(IGameEngine engine)
        {
            _engine = engine;
        }


        public Token[] ConvertToGetProp(Token token, Entity entity)
        {
            Token[] result = new Token[5];
            result[0] = new Token("GET_PROP");
            result[1] = new Token("(");
            result[2] = new Token(entity.Key);
            result[3] = token;
            result[4] = new Token(")");
            return result;
        }

        public Token[] ReplaceEntities(Token[] tokens, Entity caster, Entity target)
        {
            List<Token> result = new List<Token>();
            foreach (Token token in tokens)
            {
                if (token.Type == TokenType.Variable && token.Value.StartsWith(char.ToString(ParserConstants.SPECIAL_CHAR)))
                {
                    if(token.Value.Equals(ParserConstants.TargetKeyword))
                        result.Add(new Token(target.Key));
                    else if (token.Value.Equals(ParserConstants.CasterKeyword))
                        result.Add(new Token(caster.Key));
                }
                else
                {
                    result.Add(token);
                }
            }
            return result.ToArray();
        }

        public MeVariable SanitizeSkill(string skill, Entity caster, Entity target)
        {
            Token[] tokens = Tokenizer.Tokenize(skill);
            Token[] sanitizedTokens = ReplaceEntities(tokens, caster,target);
            return FunctionalTreeConverter.BuildTree(sanitizedTokens, _engine).Value;
        }

        public Token[] ReplaceProperties(Token[] tokens, Entity entity)
        {
            List<Token> result = new List<Token>();
            foreach (Token token in tokens)

            {
                if (token.Type == TokenType.Variable)
                {
                    if (entity.GetProperty(token.Value) != null)
                    {
                        result.AddRange(ConvertToGetProp(token,entity));
                    }
                    else
                    {
                        result.Add(token);
                    }
                }
                else
                {
                    result.Add(token);
                }
            }
            return result.ToArray();
        }

        public double SanitizeCompoundStat(Entity entity, string expression)
        {
            Token[] tokens = Tokenizer.Tokenize(expression);
            Token[] sanitizedTokens = ReplaceProperties(tokens, entity);
            return FunctionalTreeConverter.BuildTree(sanitizedTokens,_engine).Value.ToDouble();

        }
    }
}
