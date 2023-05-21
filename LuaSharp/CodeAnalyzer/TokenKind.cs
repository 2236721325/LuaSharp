
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LuaSharp.CodeAnalyzer
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TokenKind
    {
        /// <summary>
        /// end of file
        /// </summary>
        TOKEN_EOF,              // end-of-file
        /// <summary>
        /// ...
        /// </summary>
        TOKEN_VARARG,           // ...
        /// <summary>
        /// ;
        /// </summary>
        TOKEN_SEP_SEMI,         // ;
        /// <summary>
        /// ,
        /// </summary>
        TOKEN_SEP_COMMA,        // ,ss
        /// <summary>
        /// .
        /// </summary>
        TOKEN_SEP_DOT,          // .
        /// <summary>
        /// :
        /// </summary>
        TOKEN_SEP_COLON,        // :
        /// <summary>
        /// ::
        /// </summary>
        TOKEN_SEP_LABEL,        // ::
        /// <summary>
        /// (
        /// </summary>
        TOKEN_SEP_LPAREN,       // (
        /// <summary>
        /// )
        /// </summary>
        TOKEN_SEP_RPAREN,       // )
        /// <summary>
        /// [
        /// </summary>
        TOKEN_SEP_LBRACK,       // [
        /// <summary>
        /// ]
        /// </summary>
        TOKEN_SEP_RBRACK,       // ]
        /// <summary>
        /// {
        /// </summary>
        TOKEN_SEP_LCURLY,       // {
        /// <summary>
        /// }
        /// </summary>
        TOKEN_SEP_RCURLY,       // }
        /// <summary>
        /// =
        /// </summary>
        TOKEN_OP_ASSIGN,        // =
        /// <summary>
        /// -
        /// </summary>
        TOKEN_OP_MINUS,         // - (sub or unm)
        /// <summary>
        /// ~
        /// </summary>
        TOKEN_OP_WAVE,          // ~ (bnot or bxor)
        /// <summary>
        /// +
        /// </summary>
        TOKEN_OP_ADD,           // +
        /// <summary>
        /// *
        /// </summary>
        TOKEN_OP_MUL,           // *
        /// <summary>
        /// /
        /// </summary>
        TOKEN_OP_DIV,           // /
        /// <summary>
        /// //
        /// </summary>
        TOKEN_OP_IDIV,          // //
        /// <summary>
        /// ^
        /// </summary>
        TOKEN_OP_POW,           // ^
        /// <summary>
        /// %
        /// </summary>
        TOKEN_OP_MOD,           // %
        /// <summary>
        /// &
        /// </summary>
        TOKEN_OP_BAND,          // &
        /// <summary>
        /// |
        /// </summary>
        TOKEN_OP_BOR,           // |
        /// <summary>
        /// >>
        /// </summary>
        TOKEN_OP_SHR,           // >>
        /// <summary>
        /// <<
        /// </summary>
        TOKEN_OP_SHL,           // <<
        /// <summary>
        /// ..
        /// </summary>
        TOKEN_OP_CONCAT,        // ..

        /// <summary>
        /// <
        /// </summary>
        TOKEN_OP_LT,            // <
        /// <summary>
        /// <=
        /// </summary>
        TOKEN_OP_LE,            // <=
        /// <summary>
        /// >
        /// </summary>
        TOKEN_OP_GT,            // >
        /// <summary>
        /// >=
        /// </summary>
        TOKEN_OP_GE,            // >=
        /// <summary>
        /// ==
        /// </summary>
        TOKEN_OP_EQ,            // ==

        /// <summary>
        /// ~=
        /// </summary>
        TOKEN_OP_NE,            // ~=
        /// <summary>
        /// #
        /// </summary>
        TOKEN_OP_LEN,           // #
        /// <summary>
        /// and
        /// </summary>
        TOKEN_OP_AND,           // and
        /// <summary>
        /// or
        /// </summary>
        TOKEN_OP_OR,            // or
        /// <summary>
        /// not
        /// </summary>
        TOKEN_OP_NOT,           // not
        /// <summary>
        /// break
        /// </summary>
        TOKEN_KW_BREAK,         // break

        /// <summary>
        /// do
        /// </summary>
        TOKEN_KW_DO,            // do
        /// <summary>
        /// else
        /// </summary>
        TOKEN_KW_ELSE,          // else
        /// <summary>
        /// elseif
        /// </summary>
        TOKEN_KW_ELSEIF,        // elseif
        /// <summary>
        /// end
        /// </summary>
        TOKEN_KW_END,           // end
        /// <summary>
        /// false
        /// </summary>
        TOKEN_KW_FALSE,         // false
        /// <summary>
        /// for
        /// </summary>
        TOKEN_KW_FOR,           // for
        /// <summary>
        /// function
        /// </summary>
        TOKEN_KW_FUNCTION,      // function
        /// <summary>
        /// goto
        /// </summary>
        TOKEN_KW_GOTO,          // goto
        /// <summary>
        /// if
        /// </summary>
        TOKEN_KW_IF,            // if
        /// <summary>
        /// in
        /// </summary>
        TOKEN_KW_IN,            // in
        /// <summary>
        /// local
        /// </summary>
        TOKEN_KW_LOCAL,         // local
        /// <summary>
        /// nil
        /// </summary>
        TOKEN_KW_NIL,           // nil
        /// <summary>
        /// repeat
        /// </summary>
        TOKEN_KW_REPEAT,        // repeat
        /// <summary>
        /// return
        /// </summary>
        TOKEN_KW_RETURN,        // return
        /// <summary>
        /// then
        /// </summary>
        TOKEN_KW_THEN,          // then
        /// <summary>
        /// true
        /// </summary>
        TOKEN_KW_TRUE,          // true
        /// <summary>
        /// until
        /// </summary>
        TOKEN_KW_UNTIL,         // until
        /// <summary>
        /// while
        /// </summary>
        TOKEN_KW_WHILE,         // while
        /// <summary>
        /// identifier
        /// </summary>
        TOKEN_IDENTIFIER,       // identifier
        /// <summary>
        /// number literal
        /// </summary>
        TOKEN_NUMBER,           // number literal
        /// <summary>
        /// string literal
        /// </summary>
        TOKEN_STRING,           // string literal
        /// <summary>
        /// -
        /// </summary>
        TOKEN_OP_UNM = TOKEN_OP_MINUS,  // unary minus
        /// <summary>
        /// -
        /// </summary>
        TOKEN_OP_SUB = TOKEN_OP_MINUS,
        /// <summary>
        /// ~
        /// </summary>
        TOKEN_OP_BNOT = TOKEN_OP_WAVE,
        /// <summary>
        /// ~
        /// </summary>
        TOKEN_OP_BXOR = TOKEN_OP_WAVE
    }
}