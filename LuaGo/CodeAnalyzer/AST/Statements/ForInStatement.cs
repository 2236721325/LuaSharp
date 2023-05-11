using LuaGo.CodeAnalyzer.AST.Expressions;

namespace LuaGo.CodeAnalyzer.AST.Statements
{
    public class ForInStatement:Statement
    {
        public int LineOfDo { get; set; }
        public List<string> NameList { get; set; }

        public List<Expression> ExpressionList { get; set; }

        public Block Block { get; set; }
    }

}
