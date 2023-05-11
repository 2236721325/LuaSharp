namespace LuaGo.CodeAnalyzer.AST.Expressions
{
    // 定义 FuncDefExpression 类型
    public class FuncDefExpression : Expression
    {
        public int Line { get; set; }
        public int LastLine { get; set; }
        public List<string> ParList { get; set; }
        public bool IsVararg { get; set; }
        public Block Block { get; set; }
    }
}
