namespace LuaSharp.CodeAnalyzer.AST.Expressions
{
    // 定义 TrueExpression 类型
    public class TrueExpression : IExpression
    {
        public string TypeName => nameof(TrueExpression);

        public int Line { get; set; }

        public TrueExpression(int line)
        {
            Line = line;
        }

    }
}
