namespace LuaGo.CodeAnalyzer.AST.Expressions
{
    // 定义 IntegerExpression 类型
    public class IntegerExpression : IExpression
    {
        public int Line { get; set; }

        public IntegerExpression(int line, long value)
        {
            Line = line;
            Value = value;
        }

        public long Value { get; set; }
    }
}
