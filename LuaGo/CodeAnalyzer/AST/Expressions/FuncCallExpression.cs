namespace LuaGo.CodeAnalyzer.AST.Expressions
{
    // 定义 FuncCallExpression 类型
    public class FuncCallExpression : Expression
    {
        public int Line { get; set; }
        public int LastLine { get; set; }
        public Expression PrefixExpression { get; set; }
        public StringExpression NameExpression { get; set; }
        public List<Expression> Args { get; set; }
    }
}
