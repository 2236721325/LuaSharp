namespace LuaGo.CodeAnalyzer.AST.Statements
{
    // 定义类型 GotoStat
    public class GotoStatement : Statement
    {
        public string Name { get; set; }

        public GotoStatement(string name)
        {
            Name = name;
        }
    }

}
