namespace LuaSharp.CodeAnalyzer.AST.Expressions
{
    // 定义 ConcatExpression 类型
    public class ConcatExpression : IExpression
    {
        public int Line { get; set; }

        public ConcatExpression(int line, List<IExpression> expressions)
        {
            Line = line;
            Expressions = expressions;
        }

        public List<IExpression> Expressions { get; set; }
    }
}
