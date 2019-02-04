using System;

namespace CompilerSimpleCSharp
{
    internal class OtherToken : Token
    {
        public string value;

        public OtherToken(string value) : base()
        {
            this.Value = value;
        }

        public string Value { get => value; set => this.value = value; }
    }
}