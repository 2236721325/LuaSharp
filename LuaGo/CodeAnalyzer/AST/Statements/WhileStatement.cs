
using LuaGo.CodeAnalyzer.AST.Expressions;

namespace LuaGo.CodeAnalyzer.AST.Statements
{
    public class WhileStatement:IStatement
    {
        public IExpression ConditionExpression { get; set; }
        public Block Block { get; set; }

        public WhileStatement(Block block, IExpression conditionExpression)
        {
            Block = block;
            ConditionExpression = conditionExpression;
        }
    }

}
