using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeySndr.Base.Exceptions
{
    public class ScriptException : Exception
    {
        public ScriptException(string m)
            : base(m)
        {

        }
    }

    public class ScriptParseException : ScriptException
    {
        public Jint.Parser.Ast.Program Program { get; private set; }

        public ScriptParseException(Jint.Parser.Ast.Program p, string m)
            : base(m)
        {
            Program = p;
        }
    }
}
