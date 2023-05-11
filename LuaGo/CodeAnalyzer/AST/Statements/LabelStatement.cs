﻿namespace LuaGo.CodeAnalyzer.AST.Statements
{
    // 定义类型 LabelStat
    public class LabelStatement : Statement
    {
        public LabelStatement(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

    }

}