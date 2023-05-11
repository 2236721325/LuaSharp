using LuaGo.CodeAnalyzer.AST;
using LuaGo.CodeAnalyzer.AST.Expressionressions;
using LuaGo.CodeAnalyzer.AST.Statements;

namespace LuaGo.CodeAnalyzer
{
    public class Parser
    {

        private Lexer lexer;
        public Parser(Lexer lexer)
        {
            this.lexer = lexer;
        }

        public Block ParseBlock()
        {
            return new Block(
                parseStatements(),
                parseReturnExpressions(),
                lexer.Line
                );
                
        }

        private List<Expression> parseReturnExpressions()
        {
            throw new NotImplementedException();
        }

        private List<Statement> parseStatements()
        {
            throw new NotImplementedException();
        }
        private Statement parseStatement()
        {

        }
    }
}