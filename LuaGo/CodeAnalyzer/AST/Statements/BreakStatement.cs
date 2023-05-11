namespace LuaGo.CodeAnalyzer.AST.Statements
{
    /// <summary>
    /// break
    /// </summary>
    public class BreakStatement : Statement
    {
        public int Line { get; set; }

        public BreakStatement(int line)
        {
            Line = line;
        }
    }
}
