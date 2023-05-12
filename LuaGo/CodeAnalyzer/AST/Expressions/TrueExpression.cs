namespace LuaGo.CodeAnalyzer.AST.Expressions
{
    // 定义 TrueExpression 类型
    public class TrueExpression : IExpression
    {
        public int Line { get; set; }

        public TrueExpression(int line)
        {
            Line = line;
        }
    }
}
