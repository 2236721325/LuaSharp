
using System.Text.Json;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace LuaGo.Test.CodeAnalyzer
{
    public class LexerTest
    {

        private readonly ITestOutputHelper _Output;

        public LexerTest(ITestOutputHelper output)
        {
            _Output = output;
        }


        [Fact]
        public void BasicTest()
        {
            var codes=Directory.GetFiles("LuaTestCode");

            foreach(var code in codes)
            {
                SingleTest(code);
            }

        }

        private void SingleTest(string file_path)
        {
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
