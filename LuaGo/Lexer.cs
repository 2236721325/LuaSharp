using System.ComponentModel;
using System.Text.RegularExpressions;

namespace LuaGo
{
    class Lexer
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
                    else if (Chunk.Length == 1 || ! Char.IsDigit( Chunk[1]))
                    {
                        next(1);
                        return new Token(TokenKind.TOKEN_SEP_DOT, Line, ".");
                    }
                    break;

               
              
            }

            throw new Exception("not expected!");
        
        }

        private scanNumber()
        {
            if (Chunk[0] == '.')
            {
                next(1);
            }
            next(1);
            
            string 
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
                var reOpeningLongBracket = new Regex(@"^\[=*\[");
                var reClosingLongBracket = new Regex(@"^]=*\]");
                var openingLongBracket = reOpeningLongBracket.Match(Chunk);
                if (openingLongBracket.Success)
                {
                    return;
                    scanLongString();
                }
            }


            while (Chunk.Length > 0 && !isNewLine(Chunk[0]))
            {
                next(1);
            }
        }

        private void scanLongString()
        {

        }



    }
}