﻿using LuaGo.CodeAnalyzer.AST;
using LuaGo.CodeAnalyzer.AST.Expressions;
using LuaGo.CodeAnalyzer.AST.Statements;
using LuaGo.Exceptions;

namespace LuaGo.CodeAnalyzer.Parsers
{
    public partial class Parser
    {

        private Lexer lexer;
        public Parser(Lexer lexer)
        {
            this.lexer = lexer;
        }

        private Block ParseBlock()
        {
            return new Block(
                ParseStatements(),
                ParseReturnExpressions(),
                lexer.Line
                );

        }


        #region Utility
        private bool IsReturnOrBlockEnd(Token token)
        {
            switch (token.Kind)
            {
                case TokenKind.TOKEN_EOF:
                case TokenKind.TOKEN_KW_RETURN:
                case TokenKind.TOKEN_KW_END:
                case TokenKind.TOKEN_KW_ELSE:
                case TokenKind.TOKEN_KW_ELSEIF:
                case TokenKind.TOKEN_KW_UNTIL:
                    return true;


                default:
                    return false;
            }
        }

        private List<string> FinishNameList(string name0)
        {
            var nameList = new List<string>
            {
                name0
            };
            while (lexer.LookAhead().Kind == TokenKind.TOKEN_SEP_COMMA)
            {
                lexer.NextToken();
                nameList.Add(lexer.NextIdentifier().Value!);
            }
            return nameList;
        }

        #endregion
    }
}