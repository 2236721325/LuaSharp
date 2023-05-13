using LuaGo.CodeAnalyzer.AST.Expressions;
using LuaGo.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            exp4  ::= exp3 {('+' | '-' ) exp3}
            exp3  ::= exp2 {( '＊' | '/' | '//' | '%' ) exp2}
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



        #region expression parse 12-0
        // exp12 ::= exp11 {or exp11}
        private IExpression ParseExpression12()
        {
            var exp = ParseExpression11();
            while (lexer.LookAhead().Kind == TokenKind.TOKEN_OP_OR)
            {
                var or_token = lexer.NextToken();

                exp = new BinopExpression(
                    exp, or_token.Kind,
                    ParseExpression11(),
                    or_token.Line
                    );

            }
            return exp;
        }

        // exp11 ::= exp10 {and exp10}
        private IExpression ParseExpression11()
        {
            var exp = ParseExpression10();
            while (lexer.LookAhead().Kind == TokenKind.TOKEN_OP_AND)
            {
                var or_token = lexer.NextToken();

                exp = new BinopExpression(
                    exp, or_token.Kind,
                    ParseExpression10(),
                    or_token.Line
                    );
            }
            return exp;
        }

        // exp10 ::= exp9 {('<' | '>' | '<=' | '>=' | '～=' | '==') exp9}

        private IExpression ParseExpression10()
        {
            var exp = ParseExpression9();
            while (true)
            {
                switch (lexer.LookAhead().Kind)
                {
                    case TokenKind.TOKEN_OP_LE:
                    case TokenKind.TOKEN_OP_LT:
                    case TokenKind.TOKEN_OP_GE:
                    case TokenKind.TOKEN_OP_GT:
                    case TokenKind.TOKEN_OP_NE:
                    case TokenKind.TOKEN_OP_EQ:
                        var op_token = lexer.NextToken();
                        exp = new BinopExpression(exp, op_token.Kind, ParseExpression9(), op_token.Line);
                        break;
                    default:
                        return exp;
                }
            }

        }
        //   exp9  ::= exp8 {'|' exp8}
        private IExpression ParseExpression9()
        {
            var exp = ParseExpression8();
            while (lexer.LookAhead().Kind == TokenKind.TOKEN_OP_BOR)
            {
                var op_token = lexer.NextToken();
                exp = new BinopExpression(exp, op_token.Kind, ParseExpression8(), op_token.Line);
            }
            return exp;
        }

        //exp8  ::= exp7 {'～' exp7}
        private IExpression ParseExpression8()
        {
            var exp = ParseExpression7();
            while (lexer.LookAhead().Kind == TokenKind.TOKEN_OP_BXOR)
            {
                var op_token = lexer.NextToken();
                exp = new BinopExpression(exp, op_token.Kind, ParseExpression7(), op_token.Line);
            }
            return exp;
        }

        //exp7  ::= exp6 {'&' exp6}

        private IExpression ParseExpression7()
        {
            var exp = ParseExpression6();
            while (lexer.LookAhead().Kind == TokenKind.TOKEN_OP_BAND)
            {
                var op_token = lexer.NextToken();
                exp = new BinopExpression(exp, op_token.Kind, ParseExpression6(), op_token.Line);
            }
            return exp;
        }

        // exp6  ::= exp5 {('<<' | '>>') exp5}
        private IExpression ParseExpression6()
        {
            var exp = ParseExpression5();
            while (true)
            {
                switch (lexer.LookAhead().Kind)
                {
                    case TokenKind.TOKEN_OP_SHL:
                    case TokenKind.TOKEN_OP_SHR:
                        var op_token = lexer.NextToken();
                        exp = new BinopExpression(exp, op_token.Kind, ParseExpression5(), op_token.Line);
                        break;
                    default:
                        return exp;
                }
            }
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

        // exp4  ::= exp3 {('+' | '-' ) exp3}
        private IExpression ParseExpression4()
        {
            var exp = ParseExpression3();
            while (true)
            {
                switch (lexer.LookAhead().Kind)
                {
                    case TokenKind.TOKEN_OP_ADD:
                    case TokenKind.TOKEN_OP_SUB:
                        var op_token = lexer.NextToken();
                        exp = new BinopExpression(exp, op_token.Kind, ParseExpression3(), op_token.Line);
                        break;
                    default:
                        return exp;
                }
            }
        }

        // exp3  ::= exp2 {( '＊' | '/' | '//' | '%' ) exp2}

        private IExpression ParseExpression3()
        {
            var exp = ParseExpression2();
            while (true)
            {
                switch (lexer.LookAhead().Kind)
                {
                    case TokenKind.TOKEN_OP_MUL:
                    case TokenKind.TOKEN_OP_MOD:
                    case TokenKind.TOKEN_OP_DIV:
                    case TokenKind.TOKEN_OP_IDIV:

                        var op_token = lexer.NextToken();
                        exp = new BinopExpression(exp, op_token.Kind, ParseExpression2(), op_token.Line);
                        break;
                    default:
                        return exp;
                }
            }
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
                    var op_token = lexer.NextToken();
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


        #endregion


        // tableconstructor ::= ‘{’ [fieldlist] ‘}’
        private IExpression ParseTableConstructorExpression()
        {
            int line = lexer.Line;
            lexer.NextTokenOfKind(TokenKind.TOKEN_SEP_LCURLY); // {
            var (keyExps, valExps) = ParseFieldList();
            lexer.NextTokenOfKind(TokenKind.TOKEN_SEP_RCURLY); // }
            int lastLine = lexer.Line;
            return new TableConstructorExpression( keyExps, valExps,line,lastLine);
        }

        // fieldlist ::= field {fieldsep field} [fieldsep]

        private (List<IExpression?> keyExps,List<IExpression> ValueExps) ParseFieldList()
        {
            List<IExpression?> ks = new List<IExpression?>();
            List<IExpression> vs = new List<IExpression>();

            if (lexer.LookAhead().Kind != TokenKind.TOKEN_SEP_RCURLY)
            {
                (IExpression? k, IExpression v) = ParseField();
                ks.Add(k);
                vs.Add(v);

                while (IsFieldSep(lexer.LookAhead()))
                {
                    lexer.NextToken();
                    if (lexer.LookAhead().Kind !=TokenKind.TOKEN_SEP_RCURLY)
                    {
                        (k,v) = ParseField();
                        ks.Add(k);
                        vs.Add(v);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return (ks, vs);
        
        }

        private bool IsFieldSep(Token token)
        {
            return token.Kind == TokenKind.TOKEN_SEP_COMMA || token.Kind == TokenKind.TOKEN_SEP_SEMI;
        }

        // field ::= ‘[’ exp ‘]’ ‘=’ exp | Name ‘=’ exp | exp
        private (IExpression? keyExp, IExpression valueExp) ParseField()
        {

            if (lexer.LookAhead().Kind == TokenKind.TOKEN_SEP_LBRACK)
            {
                lexer.NextToken();
                var keyExp = ParseExpression();
                lexer.NextTokenOfKind(TokenKind.TOKEN_SEP_RBRACK);
                lexer.NextTokenOfKind(TokenKind.TOKEN_OP_ASSIGN);
                var valExp = ParseExpression();
                return (keyExp, valExp);

            }
            var exp = ParseExpression();
            if (exp is NameExpression nameExp && lexer.LookAhead().Kind == TokenKind.TOKEN_OP_ASSIGN)
            {
                lexer.NextToken();
                var keyExp = new StringExpression(nameExp.Name, nameExp.Line);
                var valExp = ParseExpression();
                return (keyExp, valExp);
            }
            return (null, exp);

        }

        private IExpression ParseNumberExpression()
        {
            var number_token = lexer.NextToken();
            if (long.TryParse(number_token.Value, out long long_result))
            {
                return new IntegerExpression(number_token.Line, long_result);
            }
            else if (double.TryParse(number_token.Value, out double double_result))
            {
                return new FloatExpression(number_token.Line, double_result);
            }
            else
            {
                throw new SyntaxException("not a number!", number_token.Line);
            }
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
            throw new SyntaxException("checkVar Failed !", lexer.Line);
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
            var line = lexer.Line;
            lexer.NextTokenOfKind(TokenKind.TOKEN_SEP_LPAREN);

            var parList = ParseParList(out bool isVararg);
            lexer.NextTokenOfKind(TokenKind.TOKEN_SEP_RPAREN);
            var block = ParseBlock();

            var end_token = lexer.NextTokenOfKind(TokenKind.TOKEN_KW_END);
            return new FuncDefExpression(isVararg, parList, block, line, end_token.Line);
        }

        private List<string>? ParseParList(out bool isVararg)
        {
            var names = new List<string>();
            isVararg = false;
            switch (lexer.LookAhead().Kind)
            {
                case TokenKind.TOKEN_SEP_RPAREN:
                    return null;

                case TokenKind.TOKEN_VARARG:
                    lexer.NextToken();
                    isVararg = true;
                    return null;
            }
            var name_token = lexer.NextIdentifier();
            names.Add(name_token.Value!);
            while (lexer.LookAhead().Kind == TokenKind.TOKEN_SEP_COMMA)
            {
                lexer.NextToken();
                if (lexer.LookAhead().Kind == TokenKind.TOKEN_IDENTIFIER)
                {
                    name_token = lexer.NextIdentifier();
                    names.Add(name_token.Value!);
                }
                else
                {
                    lexer.NextTokenOfKind(TokenKind.TOKEN_VARARG);
                    isVararg = true;
                    break;
                }
            }
            return names;
        }
    }

}
