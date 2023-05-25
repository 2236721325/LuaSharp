using LuaSharp.CodeAnalyzer.AST.Expressions;
using LuaSharp.CodeAnalyzer.AST.Statements;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSharp.CodeAnalyzer.Parsers
{
    public partial class Parser
    {
        // prefixexp ::= var | functioncall | ‘(’ exp ‘)’
        // var ::=  Name | prefixexp ‘[’ exp ‘]’ | prefixexp ‘.’ Name
        // functioncall ::=  prefixexp args | prefixexp ‘:’ Name args

        /*
        prefixexp ::= Name
            | ‘(’ exp ‘)’
            | prefixexp ‘[’ exp ‘]’
            | prefixexp ‘.’ Name
            | prefixexp [‘:’ Name] args
        */
        private IExpression ParsePrefixExpression()
        {
            IExpression exp;
            if (lexer.LookAhead().Kind == TokenKind.TOKEN_IDENTIFIER)
            {
                var name_token = lexer.NextIdentifier();
                exp = new NameExpression(name_token.Value!, name_token.Line); ;
            }
            else //'(' exp ')'
            {
                exp = ParseParensExpression();
            }
            return FinishPrefixExpression(exp);
        }


        // (exp)
        private IExpression ParseParensExpression()
        {
            lexer.NextTokenOfKind(TokenKind.TOKEN_SEP_LPAREN);
            var exp = ParseExpression();
            lexer.NextTokenOfKind(TokenKind.TOKEN_SEP_RPAREN);
            switch (exp)
            {
                case VarargExpression:
                case FunctionCallExpression:
                case NameExpression:
                case TableAccessExpression:
                    return new ParensExpression(exp);
                default:
                    return exp;
            }
        }

        /*
        prefixexp ::= Name
            | ‘(’ exp ‘)’
            | prefixexp ‘[’ exp ‘]’
            | prefixexp ‘.’ Name
            | prefixexp [‘:’ Name] args
        */
        private IExpression FinishPrefixExpression(IExpression exp)
        {
            while (true)
            {
                switch (lexer.LookAhead().Kind)
                {
                    case TokenKind.TOKEN_SEP_LBRACK:
                        lexer.NextToken();
                        var keyExp = ParseExpression();
                        lexer.NextTokenOfKind(TokenKind.TOKEN_SEP_RBRACK);
                        exp = new TableAccessExpression(lexer.Line, exp, keyExp);   
                        break;
                    case TokenKind.TOKEN_SEP_DOT:
                        lexer.NextToken();
                        var name_token=lexer.NextIdentifier();
                        keyExp = new StringExpression(name_token.Value!, name_token.Line);
                        exp=new TableAccessExpression(name_token.Line,exp, keyExp);
                        break;

                    case TokenKind.TOKEN_SEP_COLON:
                    case TokenKind.TOKEN_SEP_LPAREN:
                    case TokenKind.TOKEN_SEP_LCURLY:
                    case TokenKind.TOKEN_STRING:
                        exp = FinishFucCallExpression(exp); // [':' Name] args
                        break;

                    default:
                        return exp;
                }
            }
        }

        private IExpression FinishFucCallExpression(IExpression prefixExp)
        {
            var nameExp = ParseNameExpression();
            var line = lexer.Line;
            var args = ParseArgs();
            var lastLine = lexer.Line;
            return new FunctionCallExpression(prefixExp, nameExp, args, line, lastLine);
        }

        private List<IExpression> ParseArgs()
        {
            List<IExpression>? args = null;
            switch (lexer.LookAhead().Kind)
            {
                case TokenKind.TOKEN_SEP_LPAREN:
                    lexer.NextToken();
                    if(lexer.LookAhead().Kind!=TokenKind.TOKEN_SEP_RPAREN)
                    {
                        args = ParseExpressions();
                    }
                    lexer.NextTokenOfKind(TokenKind.TOKEN_SEP_RPAREN);
                    break;
                case TokenKind.TOKEN_SEP_LCURLY:
                    args = new List<IExpression>()
                    {
                        ParseTableConstructorExpression()
                    };
                    break;
                default:
                    var str_token = lexer.NextTokenOfKind(TokenKind.TOKEN_STRING);
                    args = new List<IExpression>()
                    {
                        new StringExpression(str_token.Value!,str_token.Line)
                    };
                    break;
            }
            return args!;
        }

        private StringExpression? ParseNameExpression()
        {
            if (lexer.LookAhead().Kind == TokenKind.TOKEN_SEP_COLON)
            {
                lexer.NextToken();
                var name = lexer.NextIdentifier();
                return new StringExpression(name.Value!, name.Line);
            }
            return null;
        }
    }
}
