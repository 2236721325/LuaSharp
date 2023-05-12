namespace LuaGo.CodeAnalyzer.AST.Statements
{
    // 定义类型 LabelStat
    public class LabelStatement : IStatement
    {
        public LabelStatement(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

    }

}
