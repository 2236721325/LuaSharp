namespace LuaSharp.CodeAnalyzer.AST.Expressions
{
    // 定义 NameExpression 类型
    public class NameExpression : IExpression
    {
        public string TypeName => nameof(NameExpression);

        public int Line { get; set; }
        public string Name { get; set; }

        public NameExpression(string name, int line)
        {
            Name = name;
            Line = line;
        }

    }
}
