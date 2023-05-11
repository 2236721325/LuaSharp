namespace LuaGo.CodeAnalyzer.AST.Expressionressions
{
    // 定义 FloatExpression 类型
    public class FloatExpression : Expression
    {
        public int Line { get; set; }
        public double Val { get; set; }
    }
}
