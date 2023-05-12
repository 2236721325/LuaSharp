using LuaGo.CodeAnalyzer.AST.Statements;

namespace LuaGo.CodeAnalyzer.AST.Expressions
{
    // 定义 FuncCallExpression 类型
    public class FunctionCallExpression : IExpression, IStatement
    {
        public int Line { get; set; }
        public int LastLine { get; set; }
        public IExpression PrefixExpression { get; set; }
        public StringExpression NameExpression { get; set; }
        public List<IExpression> Args { get; set; }


    }


}
