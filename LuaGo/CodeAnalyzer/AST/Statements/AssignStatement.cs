using LuaGo.CodeAnalyzer.AST.Expressions;

namespace LuaGo.CodeAnalyzer.AST.Statements
{
    public class AssignStatement:Statement
    {
        public int LastLine { get; set; }
        public List<Expression> VarList { get; set; }

        public List<Expression> ExpressionList { get; set; }
    }
}
