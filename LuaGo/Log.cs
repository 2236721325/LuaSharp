namespace LuaGo
{
    public class Log
    {
        public static void Error(int line,string msg)
        {
            Console.WriteLine($"Error at line {line} : {msg}");
        }

        public static void UnexpectedChar(int line,char c)
        {
            Error(line, $"UnexpectedChar:{c}");
        }
    }
}