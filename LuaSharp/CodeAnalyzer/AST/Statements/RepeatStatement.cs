using LuaSharp.CodeAnalyzer.AST.Expressions;

namespace LuaSharp.CodeAnalyzer.AST.Statements
{
    public class RepeatStatement : IStatement
    {
        public string TypeName => nameof(RepeatStatement);

        public IExpression ConditionExpression { get; set; }
        public Block Block { get; set; }

        public RepeatStatement(Block block, IExpression conditionExpression)
        {
            Block = block;
            ConditionExpression = conditionExpression;
        }
    }

}
