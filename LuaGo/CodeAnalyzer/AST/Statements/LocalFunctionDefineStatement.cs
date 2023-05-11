using LuaGo.CodeAnalyzer.AST.Expressions;

namespace LuaGo.CodeAnalyzer.AST.Statements
{
    public class LocalFunctionDefineStatement:Statement
    {
        public string Name { get; set; }

        public FunctionDefineExpression Expression { get; set; }
    }
}
