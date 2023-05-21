using LuaSharp.CodeAnalyzer.AST.Expressions;

namespace LuaSharp.CodeAnalyzer.AST.Statements
{
    /// <summary>
    ///  ;
    /// </summary>
    public class EmptyStatement : IStatement
    {
        public string TypeName => nameof(EmptyStatement);

    }

}
