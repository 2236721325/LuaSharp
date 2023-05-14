using LuaSharp.CodeAnalyzer.AST.Expressions;

namespace LuaSharp.CodeAnalyzer.AST.Statements
{
    public class LocalVarDeclareStatement : IStatement
    {
        public int LastLine { get; set; }

        public LocalVarDeclareStatement(int lastLine, List<string> nameList, List<IExpression>? expressionList)
        {
            LastLine = lastLine;
            NameList = nameList;
            ExpressionList = expressionList;
        }

        public List<string> NameList { get; set; }

        public List<IExpression>? ExpressionList { get; set; }
    }

}
