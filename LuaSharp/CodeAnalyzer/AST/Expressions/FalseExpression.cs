﻿namespace LuaSharp.CodeAnalyzer.AST.Expressions
{
    // 定义 FalseExpression 类型
    public class FalseExpression : IExpression
    {
        public string TypeName => nameof(FalseExpression);

        public int Line { get; set; }

        public FalseExpression(int line)
        {
            Line = line;
        }

    }
}
