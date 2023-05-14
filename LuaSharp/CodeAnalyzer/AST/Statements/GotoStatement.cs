namespace LuaSharp.CodeAnalyzer.AST.Statements
{
    // 定义类型 GotoStat
    public class GotoStatement : IStatement
    {
        public string Name { get; set; }

        public GotoStatement(string name)
        {
            Name = name;
        }
    }

}
