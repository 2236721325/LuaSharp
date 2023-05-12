namespace LuaGo.CodeAnalyzer.AST.Statements
{
    // 定义类型 DoStat
    public class DoStatement : IStatement
    {
        public Block Block { get; set; }

        public DoStatement(Block block)
        {
            Block = block;
        }
    }

}
