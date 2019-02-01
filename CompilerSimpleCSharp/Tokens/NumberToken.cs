namespace CompilerSimpleCSharp
{
    internal class NumberToken : Token
    {
        private long value;

        public NumberToken(long value) : base()
        {
            this.Value = value;
        }

        public long Value { get => value; set => this.value = value; }
    }
}