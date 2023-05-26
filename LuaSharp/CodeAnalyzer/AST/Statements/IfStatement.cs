using LuaSharp.CodeAnalyzer.AST;
using LuaSharp.CodeAnalyzer.AST.Expressions;
using System.Text.Json.Serialization;

namespace LuaSharp.CodeAnalyzer.AST.Statements
{

   

    public class IfStatement : IStatement
    {
        public string TypeName => nameof(IfStatement);

        public List<IExpression> Expressions { get; set; }

        public List<Block> Blocks { get; set; }

        public IfStatement(List<Block> blocks, List<IExpression> expressions)
        {
            Blocks = blocks;
            Expressions = expressions;
        }
    }

}
