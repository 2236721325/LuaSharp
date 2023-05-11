using LuaGo.CodeAnalyzer.AST.Expressions;

namespace LuaGo.CodeAnalyzer.AST.Statements
{
    public class WhileStatement:Statement
    {
        public Expression Expression { get; set; }
        public Block Block { get; set; }
    }

}
