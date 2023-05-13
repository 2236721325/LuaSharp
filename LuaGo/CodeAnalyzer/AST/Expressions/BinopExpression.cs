namespace LuaGo.CodeAnalyzer.AST.Expressions
{
    // 定义 BinopExpression 类型
    /// <summary>
    /// 二元表达式
    /// </summary>
    public class BinopExpression : IExpression
    {
        public int Line { get; set; }
        public TokenKind Op { get; set; }
        public IExpression Expression1 { get; set; }
        public IExpression Expression2 { get; set; }
        public BinopExpression(IExpression expression1, TokenKind op, IExpression expression2, int line)
        {
            Expression1 = expression1;
            Op = op;
            Expression2 = expression2;
            Line = line;
        }
    }
}
