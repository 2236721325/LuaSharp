
using LuaGo.CodeAnalyzer.AST.Expressionressions;

namespace LuaGo.CodeAnalyzer.AST.Statements
{
    public class RepeatStatement : Statement
    {
        public Expression ConditionExpression { get; set; }
        public Block Block { get; set; }

        public RepeatStatement(Block block, Expression conditionExpression)
        {
            Block = block;
            ConditionExpression = conditionExpression;
        }
    }

}
