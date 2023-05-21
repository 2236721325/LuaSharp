using LuaSharp.CodeAnalyzer.AST.Expressions;

namespace LuaSharp.CodeAnalyzer.AST.Statements
{
    // 定义类型 LabelStat
    public class LabelStatement : IStatement
    {

        public string TypeName => nameof(LabelStatement);

        public LabelStatement(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

    }

}
