namespace LuaGo.CodeAnalyzer.AST.Expressions
{
    // 定义 ParensExpression 类型
    public class ParensExpression : Expression
    {
        public Expression Expression { get; set; }
    }
}
