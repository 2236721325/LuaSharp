namespace LuaGo
{
    class Token
    {
        public TokenKind Kind { get; set; }
        public int Line { get; set; }
        public string? Value { get; set; }

    }
}