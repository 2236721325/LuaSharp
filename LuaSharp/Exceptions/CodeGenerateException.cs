namespace LuaSharp.Exceptions
{
    public class CodeGenerateException:Exception
    {
        public CodeGenerateException() : base("LuaSharp defualt code generate exception message.") { }
        public CodeGenerateException(string message) : base(" [Code Generate Error]: " + message) { }
        public CodeGenerateException(string message, Exception innerException) : base(message, innerException) { }
    }
   
}
