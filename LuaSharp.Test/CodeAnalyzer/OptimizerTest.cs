using LuaSharp.CodeAnalyzer;
using LuaSharp.CodeAnalyzer.Parsers;
using Newtonsoft.Json;
using Xunit.Abstractions;

namespace LuaSharp.Test.CodeAnalyzer
{
    public class OptimizerTest
    {
        private readonly ITestOutputHelper _Output;

        public OptimizerTest(ITestOutputHelper output)
        {
            _Output = output;
        }

        [Theory]
        [InlineData(@"
local hello=(1+3)*6/(1+2)
local a = true and false or false or not true
local b = not not not not not false
local c = ((1 | 2) & 3) >> 1 << 1
local d = (3 + 2 - 1) * (5 % 2) // 2 / 2 ^ 2
local e = - - - - -1
local f = ~ ~ ~ ~1
")]
        public void SingleTest(string code)
        {
            var lexer = new Lexer("main", code);
            var parser = new Parser(lexer);
            var block = parser.Parse();

            _Output.WriteLine(JsonConvert.SerializeObject(block, Formatting.Indented, new JsonSerializerSettings()
            {
                //ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                //PreserveReferencesHandling=PreserveReferencesHandling.Objects
            }));

        }
    }

}
