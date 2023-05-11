
using LuaGo.CodeAnalyzer;
using System.Text.Json;
using Xunit.Abstractions;

namespace LuaGo.Test.CodeAnalyzer
{
    public class LexerTest
    {

        private readonly ITestOutputHelper _Output;

        public LexerTest(ITestOutputHelper output)
        {
            _Output = output;
        }

        [Theory]
        [InlineData("test1.lua")]
        [InlineData("test2.lua")]
        [InlineData("test3.lua")]
        [InlineData("test4.lua")]
        public void SingleTest(string file_name)
        {
            var root_path = Directory.GetCurrentDirectory();
            var file_path=Path.Combine(root_path,"LuaTestCode", file_name);
            var code=File.ReadAllText(file_path);
            var lexer = new Lexer("main", code);
            var tokens = new List<Token>();
            while (true)
            {
                var token = lexer.NextToken();
                tokens.Add(token);
                if (token.Kind == TokenKind.TOKEN_EOF)
                {
                    break;
                }
            }

            var result = JsonSerializer.Serialize(tokens, new JsonSerializerOptions()
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
            _Output.WriteLine(result);
        }

    }
}
