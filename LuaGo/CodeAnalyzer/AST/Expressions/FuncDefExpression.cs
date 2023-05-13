namespace LuaGo.CodeAnalyzer.AST.Expressions
{
    // 定义 FuncDefExpression 类型
    public class FuncDefExpression : IExpression
    {
        public int Line { get; set; }
        public int LastLine { get; set; }
        public List<string>? ParList { get; set; }
        public bool IsVararg { get; set; }

        public FuncDefExpression(bool isVararg, List<string>? parList, Block block, int line, int lastLine)
        {
            IsVararg = isVararg;
            ParList = parList;
            Block = block;
            Line = line;
            LastLine = lastLine;
        }

        public Block Block { get; set; }

   
    }
}
