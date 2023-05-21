using LuaSharp.CodeAnalyzer;
using LuaSharp.CodeAnalyzer.Parsers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace LuaSharp.Test.CodeAnalyzer
{
    public class ParserTest
    {

        private readonly ITestOutputHelper _Output;

        public ParserTest(ITestOutputHelper output)
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
            var file_path = Path.Combine(root_path, "LuaTestCode", file_name);
            var code = File.ReadAllText(file_path);
            var lexer = new Lexer("main", code);
            var parser = new Parser(lexer);
            var block=parser.Parse();
            //var result = JsonSerializer.Serialize(block, new JsonSerializerOptions()
            //{
            //    WriteIndented = true,
            //    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            //    MaxDepth=100
            //});
            //string jsonString = JsonSerializer.Serialize(block);

            _Output.WriteLine(JsonConvert.SerializeObject(block, Formatting.Indented, new JsonSerializerSettings()
            {
                //ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                //PreserveReferencesHandling=PreserveReferencesHandling.Objects
            }));
        }

    }

}
