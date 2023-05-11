namespace LuaGo.CodeAnalyzer.AST.Expressions
{
    // 定义 TableConstructorExpression 类型
    public class TableConstructorExpression : Expression
    {
        public int Line { get; set; }
        public int LastLine { get; set; }
        public List<Expression> KeyExpressions { get; set; }
        public List<Expression> ValExpressions { get; set; }
    }
}
