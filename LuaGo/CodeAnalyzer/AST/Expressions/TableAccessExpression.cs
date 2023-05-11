namespace LuaGo.CodeAnalyzer.AST.Expressionressions
{
    // 定义 TableAccessExpression 类型
    public class TableAccessExpression : Expression
    {
        public int LastLine { get; set; }
        public Expression PrefixExpression { get; set; }
        public Expression KeyExpression { get; set; }
    }
}
