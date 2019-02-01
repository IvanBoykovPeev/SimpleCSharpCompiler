namespace CompilerSimpleCSharp
{
    internal class SpecialSymbolToken : Token
    {
        public string value;

        public SpecialSymbolToken(string value) : base()
        {
            this.value = value;
        }
    }
}