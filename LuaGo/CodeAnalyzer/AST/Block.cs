using LuaGo.CodeAnalyzer.AST.Expressions;
using LuaGo.CodeAnalyzer.AST.Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaGo.CodeAnalyzer.AST
{
    public class Block
    {
        public int LastLine { get; set; }
        public List<Statement> StatementList { get; set; }

        public Block(List<Statement> statementList, List<Expression> returnExpression, int lastLine)
        {
            StatementList = statementList;
            ReturnExpression = returnExpression;
            LastLine = lastLine;
        }

        public List<Expression> ReturnExpression { get; set; }
    }
}
