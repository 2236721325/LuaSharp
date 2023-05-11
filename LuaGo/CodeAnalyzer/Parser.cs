using LuaGo.CodeAnalyzer.AST;
using LuaGo.CodeAnalyzer.AST.Expressions;
using LuaGo.CodeAnalyzer.AST.Statements;

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

        private List<Expression> parseReturnExpressions()
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
                    return new List<Expression>(0);
                case TokenKind.TOKEN_SEP_SEMI:
                    lexer.NextToken();
                    return new List<Expression>(0);
                default:
                    var expressionList = parseExpressions();
                    if (lexer.LookAhead().Kind== TokenKind.TOKEN_SEP_SEMI)
                    {
                        lexer.NextToken();
                    }
                    return expressionList;
            }
        }

        private List<Expression> parseExpressions()
        {
            var expressionList = new List<Expression>();
            expressionList.Add(parseExpression());
            while (lexer.LookAhead().Kind==TokenKind.TOKEN_SEP_COMMA)
            {
                lexer.NextToken();
                expressionList.Add(parseExpression());
            }
            return expressionList;
        }

     

        private List<Statement> parseStatements()
        {
            var statements = new List<Statement>();

            while (!isReturnOrBlockEnd(lexer.LookAhead()))
            {
                var statement = parseStatement();
                if(statement is not EmptyStatement)
                {
                    statements.Add(statement);
                }
            }
            return statements;
        }

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

        private Statement parseStatement()
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


        #region single statement parse function


        private Statement parseAssignOrFuncCallStatement()
        {
            throw new NotImplementedException();
        }

        private Statement parseLocalAssignOrFuncDefStatement()
        {
            throw new NotImplementedException();
        }

        private Statement parseFuncDefStatement()
        {
            throw new NotImplementedException();
        }


        private Statement finishForNumStatement(int lineOfFor,string identifierName)
        {
            lexer.NextTokenOfKind(TokenKind.TOKEN_OP_ASSIGN);
            var initExp = parseExpression();
            lexer.NextTokenOfKind(TokenKind.TOKEN_SEP_COMMA);
            var limitExp = parseExpression();
            lexer.NextTokenOfKind(TokenKind.TOKEN_SEP_COMMA);
            Expression stepExp;
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
        private Statement finishForInStatement(string identifierName)
        {

        }
        private Statement parseForStatement()
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

        private Statement parseIfStatement()
        {
            var conditions = new List<Expression>();
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

        private Statement parseRepeatStatement()
        {
            lexer.NextTokenOfKind(TokenKind.TOKEN_KW_REPEAT);
            var block = parseBlock();
            lexer.NextTokenOfKind(TokenKind.TOKEN_KW_UNTIL);
            var condition = parseExpression();
            return new RepeatStatement(block,condition);
        }

        private Statement parseWhileStatement()
        {
            lexer.NextTokenOfKind(TokenKind.TOKEN_KW_WHILE);
            var condition = parseExpression();
            lexer.NextTokenOfKind(TokenKind.TOKEN_KW_DO);
            var block = parseBlock();
            lexer.NextTokenOfKind(TokenKind.TOKEN_KW_END);
            return new WhileStatement(block, condition);
        }

        private Statement parseDoStatement()
        {
            lexer.NextTokenOfKind(TokenKind.TOKEN_KW_DO);
            var block=parseBlock();
            lexer.NextTokenOfKind(TokenKind.TOKEN_KW_END);
            return new DoStatement(block);
        }

        private Statement parseGotoStatement()
        {
            lexer.NextTokenOfKind(TokenKind.TOKEN_KW_GOTO);
            var nameToken = lexer.NextIdentifier();

            return new GotoStatement(nameToken.Value);
        }

        private Statement parseLabelStatement()
        {
            lexer.NextTokenOfKind(TokenKind.TOKEN_SEP_LABEL);
            var nameToken = lexer.NextIdentifier();
            lexer.NextTokenOfKind(TokenKind.TOKEN_SEP_LABEL);
            return new LabelStatement(nameToken.Value);
        }

        private Statement parseBreakStatement()
        {
            lexer.NextTokenOfKind(TokenKind.TOKEN_KW_BREAK);
            return new BreakStatement(lexer.Line);
        }

        private Statement parseEmptyStatement()
        {
            lexer.NextTokenOfKind(TokenKind.TOKEN_SEP_SEMI);
            return new EmptyStatement();
        }
    
        private Expression parseExpression()
        {
            throw new NotImplementedException();
        }


        #endregion

    }
}