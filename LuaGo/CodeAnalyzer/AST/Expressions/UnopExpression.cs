namespace LuaGo.CodeAnalyzer.AST.Expressionressions
{
    // 定义 UnopExpression 类型
    public class UnopExpression : Expression
    {
        public int Line { get; set; }
        public int Op { get; set; }
        public Expression Expression { get; set; }
    }
}
