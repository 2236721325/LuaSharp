
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSharp.CodeAnalyzer.AST.Statements;
using LuaSharp.CodeAnalyzer.AST.Expressions;
using LuaSharp.CodeAnalyzer;
using LuaSharp.CodeAnalyzer.AST;

namespace LuaGo.CodeAnalyzer.Parsers
{
    /// <summary>
    /// Parse statement
    /// </summary>
    public partial class Parser
    {
        #region Statement parse function
        private List<IStatement> ParseStatements()
        {
            var statements = new List<IStatement>();

            while (!IsReturnOrBlockEnd(lexer.LookAhead()))
            {
                var statement = ParseStatement();
                if (statement is not EmptyStatement)
                {
                    statements.Add(statement);
                }
            }
            return statements;
        }


        private IStatement ParseStatement()
        {
            switch (lexer.LookAhead().Kind)
            {
                case TokenKind.TOKEN_SEP_SEMI:
                    return ParseEmptyStatement();
                case TokenKind.TOKEN_KW_BREAK:
                    return ParseBreakStatement();
                case TokenKind.TOKEN_SEP_LABEL:
                    return ParseLabelStatement();
                case TokenKind.TOKEN_KW_GOTO:
                    return ParseGotoStatement();
                case TokenKind.TOKEN_KW_DO:
                    return ParseDoStatement();
                case TokenKind.TOKEN_KW_WHILE:
                    return ParseWhileStatement();
                case TokenKind.TOKEN_KW_REPEAT:
                    return ParseRepeatStatement();
                case TokenKind.TOKEN_KW_IF:
                    return ParseIfStatement();
                case TokenKind.TOKEN_KW_FOR:
                    return ParseForStatement();
                case TokenKind.TOKEN_KW_FUNCTION:
                    return ParseFuncDefStatement();
                case TokenKind.TOKEN_KW_LOCAL:
                    return ParseLocalAssignOrFuncDefStatement();
                default:
                    return ParseAssignOrFuncCallStatement();
            }
        }

        private IStatement ParseAssignOrFuncCallStatement()
        {

            var prefixExp = ParsePrefixExpression();
            if (prefixExp is FunctionCallExpression fc)
            {
                return fc;
            }
            else
            {
                return ParseAssignStatement(prefixExp);
            }
        }

        private IStatement ParseAssignStatement(IExpression var0)
        {
            var varList = FinishVarList(var0);
            lexer.NextTokenOfKind(TokenKind.TOKEN_OP_ASSIGN);
            var expList = ParseExpressions();
            var lastLine = lexer.Line;
            return new AssignStatement(lastLine, varList, expList);
        }




        private IStatement FinishLocalVarDeclStatement()
        {
            var name0 = lexer.NextIdentifier().Value!;
            var nameList = FinishNameList(name0);

            List<IExpression>? expressionList = null;
            if (lexer.LookAhead().Kind == TokenKind.TOKEN_OP_ASSIGN)
            {
                lexer.NextToken();
                expressionList = ParseExpressions();
            }
            var lastLine = lexer.Line;
            return new LocalVarDeclareStatement(lastLine, nameList, expressionList);
        }

        private IStatement FinishLocalFucDefStatement()
        {
            lexer.NextTokenOfKind(TokenKind.TOKEN_KW_FUNCTION);
            var name = lexer.NextIdentifier().Value!;
            var fdExp = ParseFuncDefExpression();//function body
            return new LocalFunctionDefineStatement(name, fdExp);


        }



        private IStatement ParseLocalAssignOrFuncDefStatement()
        {
            lexer.NextTokenOfKind(TokenKind.TOKEN_KW_LOCAL);

            if (lexer.LookAhead().Kind == TokenKind.TOKEN_KW_FUNCTION)
            {
                return FinishLocalFucDefStatement();
            }
            else
            {
                return FinishLocalVarDeclStatement();
            }
        }



        private AssignStatement ParseFuncDefStatement()
        {
            lexer.NextTokenOfKind(TokenKind.TOKEN_KW_FUNCTION);
            var fnExp = ParseFuncName(out bool hasColon);
            var fdExp = ParseFuncDefExpression();
            if (hasColon)
            {
                if (fdExp.ParList == null)
                {
                    fdExp.ParList = new List<string>();
                }
                fdExp.ParList.Add("");
                fdExp.ParList.InsertRange(1, fdExp.ParList);
                fdExp.ParList.Insert(0, "self");

            }
            return new AssignStatement(
                fdExp.Line,
                new List<IExpression>() { fnExp },
                new List<IExpression> { fdExp }
                );

        }


        private IStatement FinishForNumStatement(int lineOfFor, string identifierName)
        {
            lexer.NextTokenOfKind(TokenKind.TOKEN_OP_ASSIGN);
            var initExp = ParseExpression();
            lexer.NextTokenOfKind(TokenKind.TOKEN_SEP_COMMA);
            var limitExp = ParseExpression();
            lexer.NextTokenOfKind(TokenKind.TOKEN_SEP_COMMA);
            IExpression stepExp;
            if (lexer.LookAhead().Kind == TokenKind.TOKEN_SEP_COMMA)
            {
                lexer.NextToken();
                stepExp = ParseExpression();
            }
            else
            {
                stepExp = new IntegerExpression(lexer.Line, 1);
            }
            var lineOfDo = lexer.NextTokenOfKind(TokenKind.TOKEN_KW_DO).Line;
            var block = ParseBlock();
            lexer.NextTokenOfKind(TokenKind.TOKEN_KW_END);
            return new ForNumStatement(lineOfFor, lineOfDo, identifierName, initExp, limitExp, stepExp, block);
        }
        private IStatement FinishForInStatement(string name0)
        {
            var nameList = FinishNameList(name0);
            lexer.NextTokenOfKind(TokenKind.TOKEN_KW_IN);
            var expList = ParseExpressions();
            var lineOfDo = lexer.NextTokenOfKind(TokenKind.TOKEN_KW_DO).Line;
            var block = ParseBlock();
            lexer.NextTokenOfKind(TokenKind.TOKEN_KW_END);
            return new ForInStatement(lineOfDo, nameList, expList, block);

        }


        private IStatement ParseForStatement()
        {
            var forToken = lexer.NextTokenOfKind(TokenKind.TOKEN_KW_FOR);
            var identifierName = lexer.NextIdentifier().Value!;
            if (lexer.LookAhead().Kind == TokenKind.TOKEN_OP_ASSIGN)
            {
                return FinishForNumStatement(forToken.Line, identifierName);
            }
            else
            {
                return FinishForInStatement(identifierName);
            }
        }

        private IStatement ParseIfStatement()
        {
            var conditions = new List<IExpression>();
            var blocks = new List<Block>();
            lexer.NextTokenOfKind(TokenKind.TOKEN_KW_IF);
            conditions.Add(ParseExpression());
            lexer.NextTokenOfKind(TokenKind.TOKEN_KW_THEN);
            blocks.Add(ParseBlock());
            while (lexer.LookAhead().Kind == TokenKind.TOKEN_KW_ELSEIF)
            {
                lexer.NextToken();
                conditions.Add(ParseExpression());
                lexer.NextTokenOfKind(TokenKind.TOKEN_KW_THEN);
                blocks.Add(ParseBlock());
            }
            //else block -> elseif true then block 
            if (lexer.LookAhead().Kind == TokenKind.TOKEN_KW_ELSE)
            {
                lexer.NextToken();
                conditions.Add(new TrueExpression(lexer.Line));
                blocks.Add(ParseBlock());
            }
            lexer.NextTokenOfKind(TokenKind.TOKEN_KW_END);
            return new IfStatement(blocks, conditions);
        }

        private IStatement ParseRepeatStatement()
        {
            lexer.NextTokenOfKind(TokenKind.TOKEN_KW_REPEAT);
            var block = ParseBlock();
            lexer.NextTokenOfKind(TokenKind.TOKEN_KW_UNTIL);
            var condition = ParseExpression();
            return new RepeatStatement(block, condition);
        }

        private IStatement ParseWhileStatement()
        {
            lexer.NextTokenOfKind(TokenKind.TOKEN_KW_WHILE);
            var condition = ParseExpression();
            lexer.NextTokenOfKind(TokenKind.TOKEN_KW_DO);
            var block = ParseBlock();
            lexer.NextTokenOfKind(TokenKind.TOKEN_KW_END);
            return new WhileStatement(block, condition);
        }

        private IStatement ParseDoStatement()
        {
            lexer.NextTokenOfKind(TokenKind.TOKEN_KW_DO);
            var block = ParseBlock();
            lexer.NextTokenOfKind(TokenKind.TOKEN_KW_END);
            return new DoStatement(block);
        }

        private IStatement ParseGotoStatement()
        {
            lexer.NextTokenOfKind(TokenKind.TOKEN_KW_GOTO);
            var nameToken = lexer.NextIdentifier();

            return new GotoStatement(nameToken.Value!);
        }

        private IStatement ParseLabelStatement()
        {
            lexer.NextTokenOfKind(TokenKind.TOKEN_SEP_LABEL);
            var nameToken = lexer.NextIdentifier();
            lexer.NextTokenOfKind(TokenKind.TOKEN_SEP_LABEL);
            return new LabelStatement(nameToken.Value!);
        }

        private IStatement ParseBreakStatement()
        {
            lexer.NextTokenOfKind(TokenKind.TOKEN_KW_BREAK);
            return new BreakStatement(lexer.Line);
        }

        private IStatement ParseEmptyStatement()
        {
            lexer.NextTokenOfKind(TokenKind.TOKEN_SEP_SEMI);
            return new EmptyStatement();
        }



        #endregion

    }
}
