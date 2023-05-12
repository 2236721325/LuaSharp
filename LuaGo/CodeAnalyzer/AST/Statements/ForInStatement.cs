using LuaGo.CodeAnalyzer.AST.Expressions;

namespace LuaGo.CodeAnalyzer.AST.Statements
{
    public class ForInStatement:IStatement
    {
        public int LineOfDo { get; set; }

        public ForInStatement(int lineOfDo, List<string> nameList, List<IExpression> expressionList, Block block)
        {
            LineOfDo = lineOfDo;
            NameList = nameList;
            ExpressionList = expressionList;
            Block = block;
        }

        public List<string> NameList { get; set; }

        public List<IExpression> ExpressionList { get; set; }

        public Block Block { get; set; }
    }

}
