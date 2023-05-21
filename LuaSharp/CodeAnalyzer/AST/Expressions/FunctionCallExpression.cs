using LuaSharp.CodeAnalyzer.AST.Statements;

namespace LuaSharp.CodeAnalyzer.AST.Expressions
{
    // 定义 FuncCallExpression 类型
    public class FunctionCallExpression : IExpression, IStatement
    {
        public string TypeName => nameof(FunctionCallExpression);

        public int Line { get; set; }
        public int LastLine { get; set; }
        public IExpression PrefixExpression { get; set; }

        public FunctionCallExpression(IExpression prefixExpression, StringExpression? nameExpression, List<IExpression> args, int line, int lastLine)
        {
            PrefixExpression = prefixExpression;
            NameExpression = nameExpression;
            Args = args;
            Line = line;
            LastLine = lastLine;
        }

        public StringExpression? NameExpression { get; set; }
        public List<IExpression> Args { get; set; }

    }


}
