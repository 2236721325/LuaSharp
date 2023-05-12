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
        public List<IStatement> StatementList { get; set; }

        public Block(List<IStatement> statementList, List<IExpression>? returnExpression, int lastLine)
        {
            StatementList = statementList;
            ReturnExpression = returnExpression;
            LastLine = lastLine;
        }

        public List<IExpression>? ReturnExpression { get; set; }
    }
}
