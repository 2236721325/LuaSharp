namespace LuaSharp.CodeAnalyzer.AST.Expressions
{
    // 定义 FloatExpression 类型
    public class FloatExpression : IExpression
    {
        public int Line { get; set; }

        public FloatExpression(int line, double val)
        {
            Line = line;
            Val = val;
        }

        public double Val { get; set; }
    }
}
