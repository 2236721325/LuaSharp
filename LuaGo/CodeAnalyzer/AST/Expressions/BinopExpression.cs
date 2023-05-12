namespace LuaGo.CodeAnalyzer.AST.Expressions
{
    // 定义 BinopExpression 类型
    public class BinopExpression : IExpression
    {
        public int Line { get; set; }
        public int Op { get; set; }
        public IExpression Expression1 { get; set; }
        public IExpression Expression2 { get; set; }
    }
}
