using LuaGo.CodeAnalyzer.AST.Expressions;

namespace LuaGo.CodeAnalyzer.AST.Statements
{
    public class IfStatement : Statement
    {
        public List<Expression> Expressions { get; set; }

        public List<Block> Blocks { get; set; }

        public IfStatement(List<Block> blocks, List<Expression> expressions)
        {
            Blocks = blocks;
            Expressions = expressions;
        }
    }

}
