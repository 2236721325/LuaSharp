namespace LuaGo.CodeAnalyzer.AST.Expressions
{
    // 定义 ParensExpression 类型
    public class ParensExpression : IExpression
    {
        public IExpression Expression { get; set; }
    }
}
