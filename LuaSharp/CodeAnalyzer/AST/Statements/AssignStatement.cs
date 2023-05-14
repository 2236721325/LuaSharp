using LuaSharp.CodeAnalyzer.AST.Expressions;

namespace LuaSharp.CodeAnalyzer.AST.Statements
{
    public class AssignStatement : IStatement
    {
        public int LastLine { get; set; }

        public AssignStatement(int lastLine, List<IExpression> varList, List<IExpression> expressionList)
        {
            LastLine = lastLine;
            VarList = varList;
            ExpressionList = expressionList;
        }

        public List<IExpression> VarList { get; set; }

        public List<IExpression> ExpressionList { get; set; }
    }
}
