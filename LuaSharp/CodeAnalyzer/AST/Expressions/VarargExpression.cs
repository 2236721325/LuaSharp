namespace LuaSharp.CodeAnalyzer.AST.Expressions
{
    // 定义 VarargExpression 类型
    public class VarargExpression : IExpression
    {
        public int Line { get; set; }

        public VarargExpression(int line)
        {
            Line = line;
        }

        public string TypeName => nameof(VarargExpression);

    }
}
