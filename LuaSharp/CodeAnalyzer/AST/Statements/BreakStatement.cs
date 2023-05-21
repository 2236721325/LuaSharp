using LuaSharp.CodeAnalyzer.AST.Expressions;

namespace LuaSharp.CodeAnalyzer.AST.Statements
{
    /// <summary>
    /// break
    /// </summary>
    public class BreakStatement : IStatement
    {
        public string TypeName => nameof(BreakStatement);

        public int Line { get; set; }

        public BreakStatement(int line)
        {
            Line = line;
        }

    }
}
