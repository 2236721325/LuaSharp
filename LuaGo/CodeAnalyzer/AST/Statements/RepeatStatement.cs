using LuaGo.CodeAnalyzer.AST.Expressions;

namespace LuaGo.CodeAnalyzer.AST.Statements
{
    public class RepeatStatement : Statement
    {
        public Expression Expression { get; set; }
        public Block Block { get; set; }
    }

}
