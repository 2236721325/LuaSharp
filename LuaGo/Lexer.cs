using LuaGo.Exceptions;
using Serilog;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;

namespace LuaGo
{
    public class Lexer
    {

        /// <summary>
        /// 源代码
        /// </summary>
        public string Chunk { get; set; }
        public string ChunkName { get; set; }
        /// <summary>
        /// 当前行号
        /// </summary>
        public int Line { get; set; }

        public Lexer(string chunkName, string chunk)
        {
            ChunkName = chunkName;
            Chunk = chunk;
            Line = 1;
        }



        public Token NextToken()
        {
            skipWhiteSpace();
            if (Chunk.Length == 0)
            {
                return new Token(TokenKind.TOKEN_EOF, Line, null);
            }

            // 前面的代码省略
            switch (Chunk[0])
            {
                case ';':
                    next(1);
                    return new Token(TokenKind.TOKEN_SEP_SEMI, Line, ";");
                case ',':
                    next(1);
                    return new Token(TokenKind.TOKEN_SEP_COMMA, Line, ",");
                case '(':
                    next(1);
                    return new Token(TokenKind.TOKEN_SEP_LPAREN, Line, "(");
                case ')':
                    next(1);
                    return new Token(TokenKind.TOKEN_SEP_RPAREN, Line, ")");
                case ']':
                    next(1);
                    return new Token(TokenKind.TOKEN_SEP_RBRACK, Line, "]");
                case '{':
                    next(1);
                    return new Token(TokenKind.TOKEN_SEP_LCURLY, Line, "{");
                case '}':
                    next(1);
                    return new Token(TokenKind.TOKEN_SEP_RCURLY, Line, "}");
                case '+':
                    next(1);
                    return new Token(TokenKind.TOKEN_OP_ADD, Line, "+");
                case '-':
                    next(1);
                    return new Token(TokenKind.TOKEN_OP_MINUS, Line, "-");
                case '*':
                    next(1);
                    return new Token(TokenKind.TOKEN_OP_MUL, Line, "*");
                case '^':
                    next(1);
                    return new Token(TokenKind.TOKEN_OP_POW, Line, "^");
                case '%':
                    next(1);
                    return new Token(TokenKind.TOKEN_OP_MOD, Line, "%");
                case '&':
                    next(1);
                    return new Token(TokenKind.TOKEN_OP_BAND, Line, "&");
                case '|':
                    next(1);
                    return new Token(TokenKind.TOKEN_OP_BOR, Line, "|");
                case '#':
                    next(1);
                    return new Token(TokenKind.TOKEN_OP_LEN, Line, "#");
                case ':':
                    if (Chunk.StartsWith("::"))
                    {
                        next(2);
                        return new Token(TokenKind.TOKEN_SEP_LABEL, Line, "::");
                    }
                    else
                    {
                        next(1);
                        return new Token(TokenKind.TOKEN_SEP_COLON, Line, ":");
                    }
                case '/':
                    if (Chunk.StartsWith("//"))
                    {
                        next(2);
                        return new Token(TokenKind.TOKEN_OP_IDIV, Line, "//");
                    }
                    else
                    {
                        next(1);
                        return new Token(TokenKind.TOKEN_OP_DIV, Line, "/");
                    }
                case '~':
                    if (Chunk.StartsWith("~~"))
                    {
                        next(2);
                        return new Token(TokenKind.TOKEN_OP_NE, Line, "~~");
                    }
                    else
                    {
                        next(1);
                        return new Token(TokenKind.TOKEN_OP_WAVE, Line, "~");
                    }
                case '=':
                    if (Chunk.StartsWith("=="))
                    {
                        next(2);
                        return new Token(TokenKind.TOKEN_OP_EQ, Line, "==");
                    }
                    else
                    {
                        next(1);
                        return new Token(TokenKind.TOKEN_OP_ASSIGN, Line, "=");
                    }
                case '<':
                    if (Chunk.StartsWith("<<"))
                    {
                        next(2);
                        return new Token(TokenKind.TOKEN_OP_SHL, Line, "<<");
                    }
                    else if (Chunk.StartsWith("<="))
                    {
                        next(2);
                        return new Token(TokenKind.TOKEN_OP_LE, Line, "<=");
                    }
                    else
                    {
                        next(1);
                        return new Token(TokenKind.TOKEN_OP_LT, Line, "<");
                    }
                case '>':
                    if (Chunk.StartsWith(">>"))
                    {
                        next(2);
                        return new Token(TokenKind.TOKEN_OP_SHR, Line, ">>");
                    }
                    else if (Chunk.StartsWith(">="))
                    {
                        next(2);
                        return new Token(TokenKind.TOKEN_OP_GE, Line, ">=");
                    }
                    else
                    {
                        next(1);
                        return new Token(TokenKind.TOKEN_OP_GT, Line, ">");
                    }
                case '.':
                    if (Chunk.StartsWith("..."))
                    {
                        next(3);
                        return new Token(TokenKind.TOKEN_VARARG, Line, "...");
                    }
                    else if (Chunk.StartsWith(".."))
                    {
                        next(2);
                        return new Token(TokenKind.TOKEN_OP_CONCAT, Line, "..");
                    }
                    else if (Chunk.Length == 1 || !Char.IsDigit(Chunk[1]))
                    {
                        next(1);
                        return new Token(TokenKind.TOKEN_SEP_DOT, Line, ".");
                    }
                    break;


                case '[':
                    if (Chunk.StartsWith("[[") || Chunk.StartsWith("-["))
                    {
                        return new Token(TokenKind.TOKEN_STRING, Line, scanLongString());

                    }
                    else
                    {
                        next(1);
                        return new Token(TokenKind.TOKEN_SEP_LBRACK, Line, "[");
                    }
                case '\'': case '"':
                    return new Token(TokenKind.TOKEN_STRING, Line, scanShortString());
                    
            }
            if (Char.IsDigit(Chunk[0])|| Chunk[0]=='0')
            {
                return scanNumber();
            }


            if (Chunk[0] == '_' || Char.IsLetter(Chunk[0]))
            {
                var value= scanIdentifier();
                if (Constants.keywords.ContainsKey(value))
                {
                    return new Token(Constants.keywords[value], Line, value);
                }
                else
                {
                    return new Token(TokenKind.TOKEN_IDENTIFIER, Line, value);
                }
            }
            throw new ErrorException("not expected!",Line);
        
        }

        private string scanShortString()
        {
            var reShortRegex = new Regex(Constants.ShortStrRegexString);
            var strMatch = reShortRegex.Match(Chunk);
            if (strMatch.Success)
            {
                var str = strMatch.Value;
                next(str.Length);
                str = str.Substring(1, str.Length - 2);
                if (str.Contains("\\"))
                {
                    Line += Regex.Matches(str,Constants.NewLineRegexString).Count;
                    str = escape(str);
                }
                return str;
            }
            throw new ErrorException("unfinished string",Line);
        }
        public string escape(string str)
        {
            StringBuilder buf = new StringBuilder();

            while (str.Length > 0)
            {
                if (str[0] != '\\')
                {
                    buf.Append(str[0]);
                    str = str.Substring(1);
                    continue;
                }

                if (str.Length == 1)
                {
                    throw new ErrorException("unfinished string", Line);
                }

                switch (str[1])
                {
                    case 'a':
                        buf.Append('\a');
                        str = str.Substring(2);
                        continue;
                    case 'b':
                        buf.Append('\b');
                        str = str.Substring(2);
                        continue;
                    case 'f':
                        buf.Append('\f');
                        str = str.Substring(2);
                        continue;
                    case 'n':
                    case '\n':
                        buf.Append('\n');
                        str = str.Substring(2);
                        continue;
                    case 'r':
                        buf.Append('\r');
                        str = str.Substring(2);
                        continue;
                    case 't':
                        buf.Append('\t');
                        str = str.Substring(2);
                        continue;
                    case 'v':
                        buf.Append('\v');
                        str = str.Substring(2);
                        continue;
                    case '"':
                        buf.Append('"');
                        str = str.Substring(2);
                        continue;
                    case '\'':
                        buf.Append('\'');
                        str = str.Substring(2);
                        continue;
                    case '\\':
                        buf.Append('\\');
                        str = str.Substring(2);
                        continue;
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9': // \ddd
                        string found = Regex.Match(str,Constants.DecEscapeSeqRegexString).Value;
                        if (found != "")
                        {
                            int d = int.Parse(found.Substring(1), System.Globalization.NumberStyles.Integer);
                            if (d <= 0xFF)
                            {
                                buf.Append((char)d);
                                str = str.Substring(found.Length);
                                continue;
                            }
                            throw new ErrorException($"decimal escape too large near '{found}'",Line);
                        }
                        break;
                    case 'x': // \xXX
                        found = Regex.Match(str,Constants.HexEscapeSeqRegexString).Value;
                        if (found != "")
                        {
                            int d = int.Parse(found.Substring(2), System.Globalization.NumberStyles.HexNumber);
                            buf.Append((char)d);
                            str = str.Substring(found.Length);
                            continue;
                        }
                        break;
                    case 'u': // \u{XXX}
                        found = Regex.Match(str,Constants.UnicodeEscapeSeqRegexString).Value;
                        if (found != "")
                        {
                            int d = int.Parse(found.Substring(3, found.Length - 4), System.Globalization.NumberStyles.HexNumber);
                            if (d <= 0x10FFFF)
                            {
                                buf.Append(char.ConvertFromUtf32(d));
                                str = str.Substring(found.Length);
                                continue;
                            }
                            throw new ErrorException($"UTF-8 value too large near '{found}'",Line);
                        }
                        break;
                    case 'z':
                        str = str.Substring(2);
                        while (str.Length > 0 && isWhiteSpace(str[0])) // todo
                        {
                            str = str.Substring(1);
                        }
                        continue;
                }
                throw new ErrorException($"invalid escape sequence near '\\{str[1]}'", Line);
            }

            return buf.ToString();
        }

        private string scanLongString()
        {
            string openingLongBracket = new Regex(Constants.OpeningLongBracketRegexString).Match(Chunk).Value;
            if (string.IsNullOrEmpty(openingLongBracket))
            {
                throw new ErrorException($"invalid long string delimiter near '{Chunk.Substring(0, 2)}'",Line);
            }

            string closingLongBracket = openingLongBracket.Replace("[", "]");
            int closingLongBracketIdx = Chunk.IndexOf(closingLongBracket);
            if (closingLongBracketIdx < 0)
            {
                throw new ErrorException("unfinished long string or comment", Line);
            }

            string str = Chunk.Substring(openingLongBracket.Length, closingLongBracketIdx - openingLongBracket.Length);
            next(closingLongBracketIdx + closingLongBracket.Length);

            var newLineRegex = new Regex(Constants.NewLineRegexString);
            str = newLineRegex.Replace(str, "\n");
            Line += str.Count(c => c == '\n');
            if (str.Length > 0 && str[0] == '\n')
            {
                str = str.Substring(1);
            }

            return str;
        }

        private string scanIdentifier()
        {
            return scan(Constants.IdentifierRegexString);
        }
        

        private string scan(string regexString)
        {
            var regex = new Regex(regexString);
            var match = regex.Match(Chunk);
            if (match.Success)
            {
                var value = match.Value;
                next(value.Length);
                return value;
            }

            throw new Exception("Unexpected Match");
        }
        private Token scanNumber()
        {
            var value = scan(Constants.NumberRegexString);
            return new Token(TokenKind.TOKEN_NUMBER, Line, value);
         
        }
        private void skipWhiteSpace()
        {
            while (Chunk.Length > 0)
            {
                if (Chunk.StartsWith("--"))
                {
                    skipComment();
                }
                else if (Chunk.StartsWith("\r\n") || Chunk.StartsWith("\n\r"))
                {
                    next(2);
                    Line += 1;
                }
                else if (isNewLine(Chunk[0]))
                {
                    next(1);
                    Line += 1;
                }
                else if (isWhiteSpace(Chunk[0]))
                {
                    next(1);
                }
                else
                {
                    break;
                }
            }
        }

        private void next(int n)
        {
            this.Chunk = this.Chunk[n..];
        }

        private bool isWhiteSpace(char c)
        {
            return Constants.WhiteSpace.Contains(c);
        }

        private bool isNewLine(char c)
        {
            return c == '\r' || c == '\n';
        }

        /*Lua 注释
         * eg --short 
         *    -->anther 
         *    --[long]--
         *    --[===[
                 another
                 long comment
                ]===]
         */
        private void skipComment()
        {
            next(2);
            if (Chunk.StartsWith("["))
            {

                var openingLongBracket = Regex.Match(Chunk, Constants.OpeningLongBracketRegexString).Value;
                if (openingLongBracket!="")
                {
                    scanLongString();
                    return;
                }
            }


            while (Chunk.Length > 0 && !isNewLine(Chunk[0]))
            {
                next(1);
            }
        }

     



    }
}