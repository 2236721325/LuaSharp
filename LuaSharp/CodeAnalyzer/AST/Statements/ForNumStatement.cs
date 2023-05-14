using LuaSharp.CodeAnalyzer.AST.Expressions;

namespace LuaSharp.CodeAnalyzer.AST.Statements
{
    public class ForNumStatement : IStatement
    {
        public int LineOfFor { get; set; }

        public ForNumStatement(int lineOfFor, int lineOfDo, string varName, IExpression initExpression, IExpression limitExpression, IExpression stepExpression, Block block)
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

        public IExpression InitExpression { get; set; }

        public IExpression LimitExpression { get; set; }

        public IExpression StepExpression { get; set; }

        public Block Block { get; set; }


    }

}
