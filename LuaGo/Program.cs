using System.Text.Json;

namespace LuaGo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var chunk = "a = 5133132.313               -- 全局变量\r\nlocal b = 5         -- 局部变量\r\n\r\nfunction joke()\r\n    c = 5           -- 全局变量\r\n    local d = 6     -- 局部变量\r\nend\r\n\r\njoke()\r\nprint(c,d)          --> 5 nil\r\n\r\ndo\r\n    local a = 6     -- 局部变量\r\n    b = 6           -- 对局部变量重新赋值\r\n    print(a,b);     --> 6 6\r\nend\r\n\r\nprint(a,b)      --> 5 6";
            var lexer = new Lexer("main", chunk);
            while (true)
            {
                var token=lexer.NextToken();

                var str=JsonSerializer.Serialize(token);
                Console.WriteLine(str);


                if (token.Kind == TokenKind.TOKEN_EOF)
                {
                    break;
                }
            }
        }
    }
}