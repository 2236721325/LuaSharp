using LuaSharp.CodeAnalyzer.AST.Expressions;

namespace LuaSharp.CodeAnalyzer.AST.Statements
{
    // 定义类型 DoStat
    public class DoStatement : IStatement
    {
        public string TypeName => nameof(DoStatement);

        public Block Block { get; set; }

        public DoStatement(Block block)
        {
            Block = block;
        }


    }

}
