using System;
using System.Reflection;
using System.Text;

namespace CompilerSimpleCSharp.TableSymbols
{
    class LocalVariableSymbol : TableSymbol
    {
        public LocalVariableInfo localVariableInfo;

        public LocalVariableSymbol(IdentToken token, LocalVariableInfo localVariableInfo) : base(token.value)
        {
            this.localVariableInfo = localVariableInfo;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} - {1} localvartype ={2} localindex ={3}", value, GetType(), localVariableInfo.LocalType, localVariableInfo.LocalIndex);
            return sb.ToString();
        }
    }
}
