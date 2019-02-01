namespace CompilerSimpleCSharp
{
    internal class KeywordToken : Token
    {
        public string value;

        public KeywordToken(string value)
        {
            this.value = value;
        }
    }
}