using LuaSharp.CodeAnalyzer.AST.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LuaSharp.CodeAnalyzer.Parsers
{
    partial class Parser
    {
        public IExpression OptimizeLogicalOr(BinopExpression exp)
        {
            if (IsTrue(exp.Expression1))
            {
                return exp.Expression1; // true or x => true
            }
            if (IsFalse(exp.Expression1) && !IsVarargOrFuncCall(exp.Expression2))
            {
                return exp.Expression2; // false or x => x
            }
            return exp;
        }
        public IExpression OptimizeLogicalAnd(BinopExpression exp)
        {
            if (IsFalse(exp.Expression1))
            {
                return exp.Expression1; // false and x => false
            }
            if (IsTrue(exp.Expression1) && !IsVarargOrFuncCall(exp.Expression2))
            {
                return exp.Expression2; // true and x => x
            }
            return exp;
        }

        public IExpression OptimizeBitwiseBinaryOp(BinopExpression exp)
        {
            if (CastToInt(exp.Expression1, out long i) && CastToInt(exp.Expression2, out long j))
            {
                switch (exp.Op)
                {
                    case TokenKind.TOKEN_OP_BAND:
                        return new IntegerExpression(exp.Line, i & j);
                    case TokenKind.TOKEN_OP_BOR:
                        return new IntegerExpression(exp.Line, i | j);
                    case TokenKind.TOKEN_OP_BXOR:
                        return new IntegerExpression(exp.Line, i ^ j);
                    case TokenKind.TOKEN_OP_SHL:
                        return new IntegerExpression(exp.Line, i<< (int)j);
                    case TokenKind.TOKEN_OP_SHR:
                        return new IntegerExpression(exp.Line, i >> (int)j);
                }
            }
            return exp;
        }
        private IExpression OptimizePow(IExpression exp)
        {
            if (exp is BinopExpression binop)
            {
                if (binop.Op == TokenKind.TOKEN_OP_POW)
                {
                    binop.Expression2 = OptimizePow(binop.Expression2);
                }
                return OptimizeArithBinaryOp(binop);

            }
            return exp;
        }

        //ToDo
        private IExpression OptimizeArithBinaryOp(BinopExpression binop)
        {
            if (binop.Expression1 is IntegerExpression x && binop.Expression2 is IntegerExpression y)
            {
                switch (binop.Op)
                {
                    case TokenKind.TOKEN_OP_ADD:
                        return new IntegerExpression(binop.Line, x.Value + y.Value);
                    case TokenKind.TOKEN_OP_SUB:
                        return new IntegerExpression(binop.Line, x.Value - y.Value);
                    case TokenKind.TOKEN_OP_MUL:
                        return new IntegerExpression(binop.Line, x.Value * y.Value);
                    case TokenKind.TOKEN_OP_DIV:
                        if (y.Value != 0)
                        {
                            return new IntegerExpression(binop.Line, x.Value / y.Value);
                        }
                        break;
                    case TokenKind.TOKEN_OP_MOD:
                        if (y.Value != 0)
                        {
                            return new IntegerExpression(binop.Line, x.Value % y.Value);
                        }
                        break;
                }
            }

            if (CastToFloat(binop.Expression1, out double f) && CastToFloat(binop.Expression1, out double g))
            {
                switch (binop.Op)
                {
                    case TokenKind.TOKEN_OP_ADD:
                        return new FloatExpression(binop.Line, f + g);
                    case TokenKind.TOKEN_OP_SUB:
                        return new FloatExpression(binop.Line, f - g);
                    case TokenKind.TOKEN_OP_MUL:
                        return new FloatExpression(binop.Line, f * g);
                    case TokenKind.TOKEN_OP_DIV:
                        if (g != 0)
                        {
                            return new FloatExpression(binop.Line, f / g);
                        }
                        break;
                    case TokenKind.TOKEN_OP_MOD:
                        if (g != 0)
                        {
                            return new FloatExpression(binop.Line, f % g);
                        }
                        break;

                    case TokenKind.TOKEN_OP_POW:
                        return new FloatExpression(binop.Line, Math.Pow(f, g));
                }
            }
            return binop;
        }

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

        /// <summary>
        /// Optimize -
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Optimize not
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private IExpression OptimizeNotOp(UnopExpression exp)
        {
            switch (exp.Expression)
            {
                //false
                case NilExpression integer_exp:
                case FalseExpression float_exp:
                    return new TrueExpression(exp.Line);


                //true
                case TrueExpression:
                case IntegerExpression:
                case FloatExpression:
                case StringExpression:
                    return new FalseExpression(exp.Line);
                default:
                    return exp;
            }
        }
        // ^
        private IExpression OptimizeBnotOp(UnopExpression exp)
        {
            switch (exp.Expression)
            {
                case IntegerExpression integer_exp:
                    integer_exp.Value = ~integer_exp.Value;
                    return integer_exp;
                case FloatExpression float_exp:
                    var value = Convert.ToInt64(float_exp.Value);
                    return new IntegerExpression(float_exp.Line, ~value);
                default:
                    return exp;
            }
        }



        private bool IsFalse(IExpression exp)
        {
            switch (exp)
            {
                case FalseExpression:
                case NilExpression:
                    return true;
                default:
                    return false;
            }
        }

        public bool IsTrue(IExpression exp)
        {
            switch (exp)
            {
                case TrueExpression:
                case IntegerExpression:
                case FloatExpression:
                case StringExpression:
                    return true;
                default:
                    return false;
            }
        }

        public bool IsVarargOrFuncCall(IExpression exp)
        {
            switch (exp)
            {
                case VarargExpression:
                case FunctionCallExpression:
                    return true;
                default:
                    return false;
            }
        }

        public bool CastToInt(IExpression exp, out long value)
        {
            switch (exp)
            {
                case IntegerExpression x:
                    value = x.Value;
                    return true;
                case FloatExpression x:
                    value = Convert.ToInt64(x.Value);
                    return true;
                default:
                    value = 0;
                    return false;
            }
        }

        public bool CastToFloat(IExpression exp, out double value)
        {
            switch (exp)
            {
                case IntegerExpression x:
                    value = (double)x.Value;
                    return true;
                case FloatExpression x:
                    value = x.Value;
                    return true;
                default:
                    value = 0;
                    return false;
            }
        }
    }
}
