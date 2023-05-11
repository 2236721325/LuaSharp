namespace LuaGo.CodeAnalyzer.AST.Expressionressions
{
    // 定义 IntegerExpression 类型
    public class IntegerExpression : Expression
    {
        public int Line { get; set; }
        public long Val { get; set; }
    }
}
