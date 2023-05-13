using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaGo.Exceptions
{
    public class SyntaxException : Exception
    {
        public SyntaxException() : base("LuaGo defualt syntax exception message.") { }

        public SyntaxException(string message,int line):base("[Syntax Error]: "+ message + $" -> at line {line}") { }
        public SyntaxException(string message, Exception innerException) : base(message, innerException) { }

    }
}
