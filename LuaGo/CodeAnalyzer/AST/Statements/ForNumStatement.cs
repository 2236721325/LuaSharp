using LuaGo.CodeAnalyzer.AST.Expressions;

namespace LuaGo.CodeAnalyzer.AST.Statements
{
    public class ForNumStatement:Statement
    {
        public int LineOfFor { get; set; }

        public ForNumStatement(int lineOfFor, int lineOfDo, string varName, Expression initExpression, Expression limitExpression, Expression stepExpression, Block block)
        {
            LineOfFor = lineOfFor;
            LineOfDo = lineOfDo;
            VarName = varName;
            InitExpression = initExpression;
            LimitExpression = limitExpression;
            StepExpression = stepExpression;
            Block = block;
        }

        public int LineOfDo { get; set; }

        public string VarName { get; set; }

        public Expression InitExpression { get; set; }

        public Expression LimitExpression { get; set; }

        public Expression StepExpression { get; set; }

        public Block Block { get; set; }


    }

}
