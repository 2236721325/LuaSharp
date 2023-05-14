using LuaSharp.CodeAnalyzer.AST.Expressions;

namespace LuaSharp.CodeAnalyzer.AST.Statements
{
    public class LocalFunctionDefineStatement : IStatement
    {
        public string Name { get; set; }

        public LocalFunctionDefineStatement(string name, FuncDefExpression funcDefExpression)
        {
            Name = name;
            FuncDefExpression = funcDefExpression;
        }

        public FuncDefExpression FuncDefExpression { get; set; }

    }
}
