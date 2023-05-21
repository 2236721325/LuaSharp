using LuaSharp.CodeAnalyzer.AST.Expressions;
using LuaSharp.CodeAnalyzer.AST.Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LuaSharp.CodeAnalyzer.AST
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
