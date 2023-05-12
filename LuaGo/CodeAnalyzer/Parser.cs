using LuaGo.CodeAnalyzer.AST;
using LuaGo.CodeAnalyzer.AST.Expressions;
using LuaGo.CodeAnalyzer.AST.Statements;
using LuaGo.Exceptions;

namespace LuaGo.CodeAnalyzer
{
    public class Parser
    {

        private Lexer lexer;
        public Parser(Lexer lexer)
        {
            this.lexer = lexer;
        }

        private Block parseBlock()
        {
            return new Block(
                parseStatements(),
                parseReturnExpressions(),
                lexer.Line
                );
                
        }







        #region Statement parse function
        private List<IStatement> parseStatements()
        {
            var statements = new List<IStatement>();

            while (!isReturnOrBlockEnd(lexer.LookAhead()))
            {
                var statement = parseStatement();
                if (statement is not EmptyStatement)
                {
                    statements.Add(statement);
                }
            }
            return statements;
        }


        private IStatement parseStatement()
        {
            switch (lexer.LookAhead().Kind)
            {
                case TokenKind.TOKEN_SEP_SEMI:
                    return parseEmptyStatement();
                case TokenKind.TOKEN_KW_BREAK:
                    return parseBreakStatement();
                case TokenKind.TOKEN_SEP_LABEL:
                    return parseLabelStatement();
                case TokenKind.TOKEN_KW_GOTO:
                    return parseGotoStatement();
                case TokenKind.TOKEN_KW_DO:
                    return parseDoStatement();
                case TokenKind.TOKEN_KW_WHILE:
                    return parseWhileStatement();
                case TokenKind.TOKEN_KW_REPEAT:
                    return parseRepeatStatement();
                case TokenKind.TOKEN_KW_IF:
                    return parseIfStatement();
                case TokenKind.TOKEN_KW_FOR:
                    return parseForStatement();
                case TokenKind.TOKEN_KW_FUNCTION:
                    return parseFuncDefStatement();
                case TokenKind.TOKEN_KW_LOCAL:
                    return parseLocalAssignOrFuncDefStatement();
                default:
                    return parseAssignOrFuncCallStatement();
            }
        }

        private IStatement parseAssignOrFuncCallStatement()
        {
            
            var prefixExp = parsePrefixExpression();
            if (prefixExp is FunctionCallExpression  fc)
            {
                return fc;
            }
            else
            {
                return parseAssignStatement(prefixExp);
            }
        }

        private IStatement parseAssignStatement(IExpression var0)
        {
            var varList = finishVarList(var0);
            lexer.NextTokenOfKind(TokenKind.TOKEN_OP_ASSIGN);
            var expList = parseExpressions();
            var lastLine = lexer.Line;
            return new AssignStatement(lastLine, varList, expList);
        }




        private IStatement finishLocalVarDeclStatement()
        {
            var name0 = lexer.NextIdentifier().Value!;
            var nameList = finishNameList(name0);

            List<IExpression>? expressionList = null;
            if (lexer.LookAhead().Kind == TokenKind.TOKEN_OP_ASSIGN)
            {
                lexer.NextToken();
                expressionList = parseExpressions();
            }
            var lastLine = lexer.Line;
            return new LocalVarDeclareStatement(lastLine, nameList,expressionList);
        }

        private IStatement finishLocalFucDefStatement()
        {
            lexer.NextTokenOfKind(TokenKind.TOKEN_KW_FUNCTION);
            var name=lexer.NextIdentifier().Value!;
            var fdExp = parseFuncDefExpression();//function body
            return new LocalFunctionDefineStatement(name, fdExp);

            
        }



        private IStatement parseLocalAssignOrFuncDefStatement()
        {
            lexer.NextTokenOfKind(TokenKind.TOKEN_KW_LOCAL);

            if (lexer.LookAhead().Kind == TokenKind.TOKEN_KW_FUNCTION)
            {
                return finishLocalFucDefStatement();
            }
            else
            {
                return finishLocalVarDeclStatement();
            }
        }

       

        private AssignStatement parseFuncDefStatement()
        {
            lexer.NextTokenOfKind(TokenKind.TOKEN_KW_FUNCTION);
            var fnExp = parseFuncName(out bool hasColon);
            var fdExp = parseFuncDefExpression();
            if (hasColon)
            {
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


        private IStatement finishForNumStatement(int lineOfFor,string identifierName)
        {
            lexer.NextTokenOfKind(TokenKind.TOKEN_OP_ASSIGN);
            var initExp = parseExpression();
            lexer.NextTokenOfKind(TokenKind.TOKEN_SEP_COMMA);
            var limitExp = parseExpression();
            lexer.NextTokenOfKind(TokenKind.TOKEN_SEP_COMMA);
            IExpression stepExp;
            if (lexer.LookAhead().Kind == TokenKind.TOKEN_SEP_COMMA)
            {
                lexer.NextToken();
                stepExp = parseExpression();
            }
            else
            {
                stepExp = new IntegerExpression(lexer.Line, 1);
            }
            var lineOfDo = lexer.NextTokenOfKind(TokenKind.TOKEN_KW_DO).Line;
            var block = parseBlock();
            lexer.NextTokenOfKind(TokenKind.TOKEN_KW_END);
            return new ForNumStatement(lineOfFor, lineOfDo, identifierName, initExp, limitExp, stepExp, block);
        }
        private IStatement finishForInStatement(string name0)
        {
            var nameList = finishNameList(name0);
            lexer.NextTokenOfKind(TokenKind.TOKEN_KW_IN);
            var expList = parseExpressions();
            var lineOfDo = lexer.NextTokenOfKind(TokenKind.TOKEN_KW_DO).Line;
            var block = parseBlock();
            lexer.NextTokenOfKind(TokenKind.TOKEN_KW_END);
            return new ForInStatement(lineOfDo, nameList, expList, block);

        }


        private IStatement parseForStatement()
        {
            var forToken = lexer.NextTokenOfKind(TokenKind.TOKEN_KW_FOR);
            var identifierName = lexer.NextIdentifier().Value!;
            if (lexer.LookAhead().Kind == TokenKind.TOKEN_OP_ASSIGN)
            {
                return finishForNumStatement(forToken.Line,identifierName);
            }
            else
            {
                return finishForInStatement(identifierName);
            }
        }

        private IStatement parseIfStatement()
        {
            var conditions = new List<IExpression>();
            var blocks = new List<Block>();
            lexer.NextTokenOfKind(TokenKind.TOKEN_KW_IF);
            conditions.Add(parseExpression());
            lexer.NextTokenOfKind(TokenKind.TOKEN_KW_THEN);
            blocks.Add(parseBlock());
            while (lexer.LookAhead().Kind == TokenKind.TOKEN_KW_ELSEIF)
            {
                lexer.NextToken();
                conditions.Add(parseExpression());
                lexer.NextTokenOfKind(TokenKind.TOKEN_KW_THEN);
                blocks.Add(parseBlock());
            }
            //else block -> elseif true then block 
            if (lexer.LookAhead().Kind == TokenKind.TOKEN_KW_ELSE)
            {
                lexer.NextToken();
                conditions.Add(new TrueExpression(lexer.Line));
                blocks.Add(parseBlock());
            }
            lexer.NextTokenOfKind(TokenKind.TOKEN_KW_END);
            return new IfStatement(blocks,conditions);
        }

        private IStatement parseRepeatStatement()
        {
            lexer.NextTokenOfKind(TokenKind.TOKEN_KW_REPEAT);
            var block = parseBlock();
            lexer.NextTokenOfKind(TokenKind.TOKEN_KW_UNTIL);
            var condition = parseExpression();
            return new RepeatStatement(block,condition);
        }

        private IStatement parseWhileStatement()
        {
            lexer.NextTokenOfKind(TokenKind.TOKEN_KW_WHILE);
            var condition = parseExpression();
            lexer.NextTokenOfKind(TokenKind.TOKEN_KW_DO);
            var block = parseBlock();
            lexer.NextTokenOfKind(TokenKind.TOKEN_KW_END);
            return new WhileStatement(block, condition);
        }

        private IStatement parseDoStatement()
        {
            lexer.NextTokenOfKind(TokenKind.TOKEN_KW_DO);
            var block=parseBlock();
            lexer.NextTokenOfKind(TokenKind.TOKEN_KW_END);
            return new DoStatement(block);
        }

        private IStatement parseGotoStatement()
        {
            lexer.NextTokenOfKind(TokenKind.TOKEN_KW_GOTO);
            var nameToken = lexer.NextIdentifier();

            return new GotoStatement(nameToken.Value!);
        }

        private IStatement parseLabelStatement()
        {
            lexer.NextTokenOfKind(TokenKind.TOKEN_SEP_LABEL);
            var nameToken = lexer.NextIdentifier();
            lexer.NextTokenOfKind(TokenKind.TOKEN_SEP_LABEL);
            return new LabelStatement(nameToken.Value!);
        }

        private IStatement parseBreakStatement()
        {
            lexer.NextTokenOfKind(TokenKind.TOKEN_KW_BREAK);
            return new BreakStatement(lexer.Line);
        }

        private IStatement parseEmptyStatement()
        {
            lexer.NextTokenOfKind(TokenKind.TOKEN_SEP_SEMI);
            return new EmptyStatement();
        }



        #endregion

        #region Expression parse fuction
        private IExpression parseExpression()
        {
            throw new NotImplementedException();
        }


        private List<IExpression>? parseReturnExpressions()
        {
            if (lexer.LookAhead().Kind != TokenKind.TOKEN_KW_RETURN)
            {
                return null;
            }
            lexer.NextToken();
            switch (lexer.LookAhead().Kind)
            {
                case TokenKind.TOKEN_EOF:
                case TokenKind.TOKEN_KW_END:
                case TokenKind.TOKEN_KW_ELSE:
                case TokenKind.TOKEN_KW_ELSEIF:
                case TokenKind.TOKEN_KW_UNTIL:
                    return new List<IExpression>(0);
                case TokenKind.TOKEN_SEP_SEMI:
                    lexer.NextToken();
                    return new List<IExpression>(0);
                default:
                    var expressionList = parseExpressions();
                    if (lexer.LookAhead().Kind == TokenKind.TOKEN_SEP_SEMI)
                    {
                        lexer.NextToken();
                    }
                    return expressionList;
            }
        }

        private List<IExpression> parseExpressions()
        {
            var expressionList = new List<IExpression>();
            expressionList.Add(parseExpression());
            while (lexer.LookAhead().Kind == TokenKind.TOKEN_SEP_COMMA)
            {
                lexer.NextToken();
                expressionList.Add(parseExpression());
            }
            return expressionList;
        }
        private IExpression parsePrefixExpression()
        {
            throw new NotImplementedException();
        }
        private List<IExpression> finishVarList(IExpression var0)
        {
            var vars = new List<IExpression>
            {
                checkVar(var0)
            };
            while (lexer.LookAhead().Kind == TokenKind.TOKEN_SEP_COMMA)
            {
                lexer.NextToken();
                var exp = parsePrefixExpression();
                vars.Add(checkVar(exp));
            }
            return vars;
        }
        private IExpression checkVar(IExpression exp)
        {
            switch (exp)
            {
                case NameExpression _:
                case TableAccessExpression _:
                    return exp;
                default:
                    break;
            }
            throw new ErrorException("checkVar Failed !", lexer.Line);
        }
        private IExpression parseFuncName(out bool hasColon)
        {
            hasColon = false;
            var nameToken = lexer.NextIdentifier();
            IExpression exp = new NameExpression(nameToken.Value!, nameToken.Line);
            while (lexer.LookAhead().Kind == TokenKind.TOKEN_SEP_DOT)
            {
                lexer.NextToken();
                nameToken = lexer.NextIdentifier();
                var idx = new StringExpression(nameToken.Value!, nameToken.Line);
                exp = new TableAccessExpression(nameToken.Line, exp, idx);

            }
            if (lexer.LookAhead().Kind == TokenKind.TOKEN_SEP_COLON)
            {
                lexer.NextToken();
                nameToken = lexer.NextIdentifier();
                var idx = new StringExpression(nameToken.Value!, nameToken.Line);
                exp = new TableAccessExpression(nameToken.Line, exp, idx);
                hasColon = true;
            }
            return exp;
        }

        private FuncDefExpression parseFuncDefExpression()
        {

        }


        #endregion

        #region Utility
        private bool isReturnOrBlockEnd(Token token)
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
    
        private List<string> finishNameList(string name0)
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