namespace LuaGo.CodeAnalyzer.AST.Expressions
{
    // 定义 StringExpression 类型
    public class StringExpression : Expression
    {
        public int Line { get; set; }
        public string Str { get; set; }
    }
}
