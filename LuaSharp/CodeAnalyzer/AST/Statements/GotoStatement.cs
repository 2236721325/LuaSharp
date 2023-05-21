using LuaSharp.CodeAnalyzer.AST.Expressions;

namespace LuaSharp.CodeAnalyzer.AST.Statements
{
    // 定义类型 GotoStat
    public class GotoStatement : IStatement
    {
        public string TypeName => nameof(GotoStatement);

        public string Name { get; set; }

        public GotoStatement(string name)
        {
            Name = name;
        }
    }

}
