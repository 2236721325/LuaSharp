namespace LuaGo.CodeAnalyzer.AST.Statements
{
    /// <summary>
    /// break
    /// </summary>
    public class BreakStatement : IStatement
    {
        public int Line { get; set; }

        public BreakStatement(int line)
        {
            Line = line;
        }
    }
}
