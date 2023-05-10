using System.Text.RegularExpressions;

namespace LuaGo
{
    class Constants
    {

        public static readonly Dictionary<string, TokenKind> keywords = new()
        {
            {"and", TokenKind.TOKEN_OP_AND},
            {"break", TokenKind. TOKEN_KW_BREAK},
            {"do", TokenKind. TOKEN_KW_DO},
            {"else",TokenKind. TOKEN_KW_ELSE},
            {"elseif",TokenKind. TOKEN_KW_ELSEIF},
            {"end",TokenKind. TOKEN_KW_END},
            {"false",TokenKind. TOKEN_KW_FALSE},
            {"for",TokenKind. TOKEN_KW_FOR},
            {"function",TokenKind. TOKEN_KW_FUNCTION},
            {"goto",TokenKind. TOKEN_KW_GOTO},
            {"if",TokenKind. TOKEN_KW_IF},
            {"in",TokenKind. TOKEN_KW_IN},
            {"local",TokenKind. TOKEN_KW_LOCAL},
            {"nil",TokenKind. TOKEN_KW_NIL},
            {"not",TokenKind. TOKEN_OP_NOT},
            {"or",TokenKind. TOKEN_OP_OR},
            {"repeat",TokenKind. TOKEN_KW_REPEAT},
            {"return",TokenKind. TOKEN_KW_RETURN},
            {"then",TokenKind. TOKEN_KW_THEN},
            {"true", TokenKind.TOKEN_KW_TRUE},
            {"until",TokenKind. TOKEN_KW_UNTIL},
            {"while",TokenKind. TOKEN_KW_WHILE},
        };

        public static readonly char[] WhiteSpace = new char[]
        {
            '\t',
            '\n',
            '\v',
            '\f',
            '\r',
            ' ',
        };
        public const string NumberRegexString = @"^0[xX][0-9a-fA-F]*(\.[0-9a-fA-F]*)?([pP][+\-]?[0-9]+)?|^[0-9]*(\.[0-9]*)?([eE][+\-]?[0-9]+)?";
        public const string IdentifierRegexString = @"^[_\d\w]+";
    }
}