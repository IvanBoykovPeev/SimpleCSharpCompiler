namespace CompilerSimpleCSharp
{
    internal class IdentToken : Token
    {
        public string value;

        public IdentToken(string value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return value.ToString();
        }
    }
}