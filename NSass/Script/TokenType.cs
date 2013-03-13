using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSass.Script
{
    public enum TokenType
    {
        Plus,
        Minus,
        Times,
        Div,
        Mod,
        SingleEq,
        Colon,
        LParen,
        RParen,
        Comma,
        And,
        Or,
        Not,
        Eq,
        Neq,
        Gte,
        Lte,
        Gt,
        Lt,
        BeginInterpolation,
        EndInterpolation,
        SemiColon,
        LCurly,
        Splat
    }
}
