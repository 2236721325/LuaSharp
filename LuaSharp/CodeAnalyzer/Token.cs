namespace LuaSharp.CodeAnalyzer
{
    public class Token
    {
        public TokenKind Kind { get; set; }
        public int Line { get; set; }
        public string? Value { get; set; }



        public Token(TokenKind kind, int line, string? value)
        {
            Value = value;
            Line = line;
            Kind = kind;
        }



    }
}