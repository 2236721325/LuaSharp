using LuaGo.CodeAnalyzer.AST.Expressions;

namespace LuaGo.CodeAnalyzer.AST.Statements
{
    public class ForNumStatement:Statement
    {
        public int LineOfFor { get; set; }
        public int LineOfDo { get; set; }

        public string VarName { get; set; }

        public Expression InitExpression { get; set; }

        public Expression LimitExpression { get; set; }

        public Expression StepExpression { get; set; }

        public Block Block { get; set; }


    }

}
