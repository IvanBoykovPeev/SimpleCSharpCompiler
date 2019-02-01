using CompilerSimpleCSharp.TableSymbols;
using System;

namespace CompilerSimpleCSharp
{
    internal class TypeSymbol : TableSymbol
    {
        private Type type;

        public TypeSymbol(IdentToken identToken, Type type) : base(identToken.value)
        {
            this.type = type;
        }
    }
}