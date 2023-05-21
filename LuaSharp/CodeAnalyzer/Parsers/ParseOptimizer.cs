using LuaSharp.CodeAnalyzer.AST.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSharp.CodeAnalyzer.Parsers
{
    partial class Parser
    {
        private IExpression OptimizeUnaryOp(UnopExpression exp)
        {
            switch (exp.Op)
            {
                case TokenKind.TOKEN_OP_UNM:
                    return OptimizeUnmOp(exp);

                case TokenKind.TOKEN_OP_NOT:
                    return OptimizeNotOp(exp);
                case TokenKind.TOKEN_OP_BNOT:
                    return OptimizeBnotOp(exp);

                default:
                    return exp;
            }
        }

        private IExpression OptimizeUnmOp(UnopExpression exp)
        {
            switch (exp.Expression)
            {
                case IntegerExpression integer_exp:
                    integer_exp.Value = -integer_exp.Value;
                    return integer_exp;
                case FloatExpression float_exp:
                    float_exp.Value = -float_exp.Value;
                    return float_exp;
                default:
                return exp;
                    
            }
        }
      

        private IExpression OptimizeNotOp(UnopExpression exp)
        {
            throw new NotImplementedException();
        }
        private IExpression OptimizeBnotOp(UnopExpression exp)
        {
            throw new NotImplementedException();
        }

    }
}
