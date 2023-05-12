

using LuaGo.CodeAnalyzer.AST.Expressions;

namespace LuaGo.CodeAnalyzer.AST.Statements
{
    public class RepeatStatement : IStatement
    {
        public IExpression ConditionExpression { get; set; }
        public Block Block { get; set; }

        public RepeatStatement(Block block, IExpression conditionExpression)
        {
            Block = block;
            ConditionExpression = conditionExpression;
        }
    }

}
