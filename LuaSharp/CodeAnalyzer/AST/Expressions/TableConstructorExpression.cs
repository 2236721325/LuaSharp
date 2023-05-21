namespace LuaSharp.CodeAnalyzer.AST.Expressions
{
    // 定义 TableConstructorExpression 类型
    public class TableConstructorExpression : IExpression
    {
        public string TypeName => nameof(TableConstructorExpression);

        public int Line { get; set; }
        public int LastLine { get; set; }
        public List<IExpression?> KeyExpressions { get; set; }

        public TableConstructorExpression(List<IExpression?> keyExpressions, List<IExpression> valExpressions, int line, int lastLine)
        {
            KeyExpressions = keyExpressions;
            ValExpressions = valExpressions;
            Line = line;
            LastLine = lastLine;
        }

        public List<IExpression> ValExpressions { get; set; }


    }
}
