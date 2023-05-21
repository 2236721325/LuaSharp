using System.Text.Json.Serialization;

namespace LuaSharp.CodeAnalyzer.AST.Statements
{
    /// <summary>
    /// 语句仅可以被执行，不可以求值。
    /// </summary>

    public interface IStatement
    {
        public string TypeName { get; }
    }

}
