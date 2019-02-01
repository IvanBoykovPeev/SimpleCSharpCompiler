using System;

namespace CompilerSimpleCSharp
{
    internal class OtherToken : Token
    {
        private string value;

        public OtherToken(string value)
        {
            this.value = value;
        }
    }
}