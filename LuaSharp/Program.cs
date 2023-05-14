using LuaSharp.CodeAnalyzer;
using Serilog;
using System.Text.Json;

namespace LuaSharp
{
    public class Program
    {
        static void ConfigureLog()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
        }
        static void Main(string[] args)
        {
            ConfigureLog();
            Log.Information("Test Started.");
            var chunk = @"
array = {""Google"", ""Runoob""}
function elementIterator (collection)
   local index = 0
   local count = #collection
   -- 闭包函数
   return function ()
      index = index + 1
      if index <= count
      then
         --  返回迭代器的当前元素
         return collection[index]
      end
   end
end

for element in elementIterator(array)
do
   print(element)
end";
            var lexer = new Lexer("main", chunk);
            while (true)
            {
                var token = lexer.NextToken();

                var str = JsonSerializer.Serialize(token);
                Console.WriteLine(str);


                if (token.Kind == TokenKind.TOKEN_EOF)
                {
                    break;
                }
            }
            Log.Information("Test over.");

        }
    }
}