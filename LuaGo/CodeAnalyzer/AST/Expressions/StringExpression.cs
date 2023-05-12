namespace LuaGo.CodeAnalyzer.AST.Expressions
{
    // 定义 StringExpression 类型
    public class StringExpression : IExpression
    {
        
        public int Line { get; set; }
        public string Str { get; set; }

        public StringExpression(string str, int line)
        {
            Str = str;
            Line = line;
        }
    }
}
