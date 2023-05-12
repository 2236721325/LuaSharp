namespace LuaGo.CodeAnalyzer.AST.Expressions
{
    // 定义 UnopExpression 类型
    public class UnopExpression : IExpression
    {
        public int Line { get; set; }
        public int Op { get; set; }
        public IExpression Expression { get; set; }
    }
}
