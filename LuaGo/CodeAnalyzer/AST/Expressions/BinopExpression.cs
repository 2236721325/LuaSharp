namespace LuaGo.CodeAnalyzer.AST.Expressions
{
    // 定义 BinopExpression 类型
    public class BinopExpression : Expression
    {
        public int Line { get; set; }
        public int Op { get; set; }
        public Expression Expression1 { get; set; }
        public Expression Expression2 { get; set; }
    }
}
