using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CompilerSimpleCSharp.TableSymbols
{
    class FieldSymbol : TableSymbol
    {
        private FieldInfo fieldInfo;

        public FieldSymbol(IdentToken token, FieldInfo fieldInfo) : base(token.value)
        {
            this.fieldInfo = fieldInfo;
        }
    }
}
