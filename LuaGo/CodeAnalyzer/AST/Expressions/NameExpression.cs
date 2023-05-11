namespace LuaGo.CodeAnalyzer.AST.Expressionressions
{
    // 定义 NameExpression 类型
    public class NameExpression : Expression
    {
        public int Line { get; set; }
        public string Name { get; set; }
    }
}
