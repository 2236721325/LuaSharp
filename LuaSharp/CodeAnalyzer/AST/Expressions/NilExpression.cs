﻿namespace LuaSharp.CodeAnalyzer.AST.Expressions
{
    // 定义 NilExpressionression 类型
    public class NilExpression : IExpression
    {
        public string TypeName => nameof(NilExpression);

        public int Line { get; set; }

        public NilExpression(int line)
        {
            Line = line;
        }

    }
}
