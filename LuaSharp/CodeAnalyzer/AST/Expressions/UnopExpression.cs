namespace LuaSharp.CodeAnalyzer.AST.Expressions
{
    // 定义 UnopExpression 类型
    public class UnopExpression : IExpression
    {
        public string TypeName => nameof(UnopExpression);

        public int Line { get; set; }

        public UnopExpression(int line, TokenKind op, IExpression expression)
        {
            Line = line;
            Op = op;
            Expression = expression;
        }

        public TokenKind Op { get; set; }
        public IExpression Expression { get; set; }


    }
}
