namespace LuaSharp.CodeAnalyzer.AST.Expressions
{
    // 定义 FloatExpression 类型
    public class FloatExpression : IExpression
    {
        public string TypeName => nameof(FloatExpression);

        public int Line { get; set; }

        public FloatExpression(int line, double value)
        {
            Line = line;
            Value = value;
        }

        public double Value { get; set; }

    }
}
