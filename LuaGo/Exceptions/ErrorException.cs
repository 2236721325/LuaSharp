using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaGo.Exceptions
{
    public class ErrorException : Exception
    {
        public ErrorException() : base("LuaGo defualt exception message.") { }

        public ErrorException(string message,int line):base(message + $" -> at line {line}") { }
        
            

        public ErrorException(string message, Exception innerException) : base(message, innerException) { }

    }
}
