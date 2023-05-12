using LuaGo.CodeAnalyzer.AST.Expressions;

namespace LuaGo.CodeAnalyzer.AST.Statements
{
    public class IfStatement : IStatement
    {
        public List<IExpression> Expressions { get; set; }

        public List<Block> Blocks { get; set; }

        public IfStatement(List<Block> blocks, List<IExpression> expressions)
        {
            Blocks = blocks;
            Expressions = expressions;
        }
    }

}
