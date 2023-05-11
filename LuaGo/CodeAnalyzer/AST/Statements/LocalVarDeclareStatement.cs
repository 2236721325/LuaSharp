using LuaGo.CodeAnalyzer.AST.Expressions;

namespace LuaGo.CodeAnalyzer.AST.Statements
{
    public class LocalVarDeclareStatement:Statement
    {
        public int LastLine { get; set; }
        public List<string> NameList { get; set; }

        public List<Expression> ExpressionList { get; set; }
    }

}
