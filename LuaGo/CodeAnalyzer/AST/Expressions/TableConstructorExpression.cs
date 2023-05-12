namespace LuaGo.CodeAnalyzer.AST.Expressions
{
    // 定义 TableConstructorExpression 类型
    public class TableConstructorExpression : IExpression
    {
        public int Line { get; set; }
        public int LastLine { get; set; }
        public List<IExpression> KeyExpressions { get; set; }
        public List<IExpression> ValExpressions { get; set; }
    }
}
