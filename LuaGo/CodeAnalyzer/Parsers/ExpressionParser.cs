using LuaGo.CodeAnalyzer.AST.Expressions;
using LuaGo.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaGo.CodeAnalyzer.Parsers
{
/*
            exp    ::= exp12
            exp12 ::= exp11 {or exp11}
            exp11 ::= exp10 {andexp10}
            exp10 ::= exp9 {('<' | '>' | '<=' | '>=' | '～=' | '==') exp9}
            exp9  ::= exp8 {'|' exp8}
            exp8  ::= exp7 {'～' exp7}
            exp7  ::= exp6 {'&' exp6}
            exp6  ::= exp5 {('<<' | '>>') exp5}
            exp5  ::= exp4 {'..' exp4}
            exp4  ::= exp3 {('+' | '-' | '＊' | '/' | '//' | '%') exp3}
            exp2  ::= {('not' | '#' | '-' | '～')} exp1
            exp1  ::= exp0 {'^' exp2}
            exp0  ::= nil | false | true | Numeral | LiteralString
                    | '...' | functiondef | prefixexp | tableconstructor


*/
    /// <summary>
    /// Parse expression
    /// </summary>
    public partial class Parser
    {
        private IExpression ParseExpression()
        {
            return ParseExpression12();
        }

        private IExpression ParseExpression12()
        {
            var exp = ParseExpression11();
            while (lexer.LookAhead().Kind == TokenKind.TOKEN_OP_OR)
            {
                var or_token=lexer.NextToken();

                exp = new BinopExpression(
                    exp, or_token.Kind,
                    ParseExpression11(),
                    or_token.Line
                    );

            }
            return exp;
        }

        private IExpression ParseExpression11()
        {
            throw new NotImplementedException();
        }

        // exp5  ::= exp4 {'..' exp4}
        private IExpression ParseExpression5()
        {
            var exp = ParseExpression4();
            if (lexer.LookAhead().Kind != TokenKind.TOKEN_OP_CONCAT)
            {
                return exp;
            }
            int line = 0;
            var expList = new List<IExpression>()
            {
                exp
            };
            while (lexer.LookAhead().Kind == TokenKind.TOKEN_OP_CONCAT)
            {
                line = lexer.NextToken().Line;
                expList.Add(ParseExpression4());
            }
            return new ConcatExpression(line, expList);
        }

        // exp4  ::= exp3 {('+' | '-' | '＊' | '/' | '//' | '%') exp3}
        private IExpression ParseExpression4()
        {
            throw new NotImplementedException();
        }



        // exp2  ::= {('not' | '#' | '-' | '～')} exp1
        private IExpression ParseExpression2()
        {
            switch (lexer.LookAhead().Kind)
            {
                case TokenKind.TOKEN_OP_UNM:
                case TokenKind.TOKEN_OP_BNOT:
                case TokenKind.TOKEN_OP_LEN:
                case TokenKind.TOKEN_OP_NOT:
                    var op_token=lexer.NextToken();
                    return new UnopExpression(op_token.Line, op_token.Kind, ParseExpression2());
             
            }
            return ParseExpression1();
        }
        private IExpression ParseExpression1()
        {
            var exp = ParseExpression0();
            if (lexer.LookAhead().Kind == TokenKind.TOKEN_OP_POW)
            {
                var pow_token = lexer.NextToken();
                exp = new BinopExpression(exp, pow_token.Kind, ParseExpression2(), pow_token.Line);
            }
            return exp;
        }


        // exp0  ::= nil | false | true | Numeral | LiteralString
        //      | '...' | functiondef | prefixexp | tableconstructor
        private IExpression ParseExpression0()
        {
            Token token_temp;
            switch (lexer.LookAhead().Kind)
            {
                case TokenKind.TOKEN_VARARG: // '...'
                    token_temp = lexer.NextToken();
                    return new VarargExpression(token_temp.Line);

                case TokenKind.TOKEN_KW_NIL: // nil
                    token_temp = lexer.NextToken();
                    return new NilExpression(token_temp.Line);

                case TokenKind.TOKEN_KW_TRUE: // true
                    token_temp = lexer.NextToken();
                    return new TrueExpression(token_temp.Line);

                case TokenKind.TOKEN_KW_FALSE: // false
                    token_temp = lexer.NextToken();
                    return new FalseExpression(token_temp.Line);

                case TokenKind.TOKEN_STRING: // LiteralString
                    token_temp = lexer.NextToken();
                    return new StringExpression(token_temp.Value!, token_temp.Line);

                case TokenKind.TOKEN_NUMBER: // Numeral
                    return ParseNumberExpression();

                case TokenKind.TOKEN_SEP_LCURLY: // tableconstructor
                    return ParseTableConstructorExpression();

                case TokenKind.TOKEN_KW_FUNCTION: // functiondef
                    lexer.NextToken();
                    return ParseFuncDefExpression();

                default: // prefixexp
                    return ParsePrefixExpression();
            }
        }

        private IExpression ParseTableConstructorExpression()
        {
            throw new NotImplementedException();
        }

        private IExpression ParseNumberExpression()
        {
            throw new NotImplementedException();
        }
    

        private List<IExpression>? ParseReturnExpressions()
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
                    var expressionList = ParseExpressions();
                    if (lexer.LookAhead().Kind == TokenKind.TOKEN_SEP_SEMI)
                    {
                        lexer.NextToken();
                    }
                    return expressionList;
            }
        }

        private List<IExpression> ParseExpressions()
        {
            var expressionList = new List<IExpression>();
            expressionList.Add(ParseExpression());
            while (lexer.LookAhead().Kind == TokenKind.TOKEN_SEP_COMMA)
            {
                lexer.NextToken();
                expressionList.Add(ParseExpression());
            }
            return expressionList;
        }
        private IExpression ParsePrefixExpression()
        {
            throw new NotImplementedException();
        }
        private List<IExpression> FinishVarList(IExpression var0)
        {
            var vars = new List<IExpression>
            {
                CheckVar(var0)
            };
            while (lexer.LookAhead().Kind == TokenKind.TOKEN_SEP_COMMA)
            {
                lexer.NextToken();
                var exp = ParsePrefixExpression();
                vars.Add(CheckVar(exp));
            }
            return vars;
        }
        private IExpression CheckVar(IExpression exp)
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
        private IExpression ParseFuncName(out bool hasColon)
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

        private FuncDefExpression ParseFuncDefExpression()
        {
            throw new NotImplementedException();
        }



    }

}
