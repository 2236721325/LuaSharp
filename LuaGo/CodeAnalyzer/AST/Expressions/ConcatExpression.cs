namespace LuaGo.CodeAnalyzer.AST.Expressions
{
    // 定义 ConcatExpression 类型
    public class ConcatExpression : IExpression
    {
        public int Line { get; set; }
        public List<IExpression> Expressions { get; set; }
    }
}
