namespace LuaSharp.CodeAnalyzer.AST.Expressions
{
    // 定义 TableAccessExpression 类型
    public class TableAccessExpression : IExpression
    {
        public string TypeName => nameof(TableAccessExpression);

        public int LastLine { get; set; }

        public TableAccessExpression(int lastLine, IExpression prefixExpression, IExpression keyExpression)
        {
            LastLine = lastLine;
            PrefixExpression = prefixExpression;
            KeyExpression = keyExpression;
        }

        public IExpression PrefixExpression { get; set; }
        public IExpression KeyExpression { get; set; }

    }
}
