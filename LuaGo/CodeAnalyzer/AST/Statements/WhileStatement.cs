
using LuaGo.CodeAnalyzer.AST.Expressionressions;

namespace LuaGo.CodeAnalyzer.AST.Statements
{
    public class WhileStatement:Statement
    {
        public Expression ConditionExpression { get; set; }
        public Block Block { get; set; }

        public WhileStatement(Block block, Expression conditionExpression)
        {
            Block = block;
            ConditionExpression = conditionExpression;
        }
    }

}
