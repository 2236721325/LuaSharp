namespace LuaGo.CodeAnalyzer.AST.Expressionressions
{
    // 定义 ConcatExpression 类型
    public class ConcatExpression : Expression
    {
        public int Line { get; set; }
        public List<Expression> Expressions { get; set; }
    }
}
