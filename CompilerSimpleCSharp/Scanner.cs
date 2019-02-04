using System;
using System.IO;
using System.Text;

namespace CompilerSimpleCSharp
{
    class Scanner
    {
        const char EOF = '\u001a';
        const char CR = '\r';
        const char LF = '\n';

        private static readonly string keyWords = " scanf printf ";
        private static readonly string specialSymbolsOne = "();*/%";
        private static readonly string specialSymbolsTwo = "+=-";
        private static readonly string specialSymbolsTwoPairs = " ++ -- "; //Warning:Need space between pair

        private TextReader reader;
        private char currentChar;

        public Scanner(TextReader reader)
        {
            this.reader = reader;
            ReadNextChar();
        }

        internal Token Next()
        {
            while (true)
            {
                //Check for IdentToken Or IdentToken
                if (currentChar >= 'a' && currentChar <= 'z' 
                    || currentChar >= 'A' && currentChar <= 'Z')
                {
                    StringBuilder sb = new StringBuilder();
                    while (currentChar >= 'a' && currentChar <= 'z' 
                        || currentChar >= 'A' && currentChar <= 'Z' 
                        || currentChar >= '0' && currentChar <= '9')
                    {
                        sb.Append(currentChar);
                        ReadNextChar();
                    }
                    string ident = sb.ToString();
                    if (keyWords.Contains(string.Format($" {ident} ")))
                    {
                        return new KeywordToken(ident);
                    }
                    return new IdentToken(sb.ToString());
                }
                //Check NumberToken
                if (currentChar >= '0' && currentChar <= '9')
                {
                    StringBuilder sb = new StringBuilder();
                    while (currentChar >= '0' && currentChar <= '9')
                    {
                        sb.Append(currentChar);
                        ReadNextChar();
                    }
                    return new NumberToken(Convert.ToInt64(sb.ToString()));
                }
                //Skip Space
                if (currentChar == CR || currentChar == LF 
                    || currentChar == ' ' || currentChar == '\t')
                {
                    ReadNextChar();
                    continue;
                }
                //Check specialSymbolsOne
                if (specialSymbolsOne.Contains(currentChar.ToString()))
                {
                    char spChar = currentChar;
                    ReadNextChar();
                    return new SpecialSymbolToken(spChar.ToString());
                }
                //Check specialSymbolsTwo
                if (specialSymbolsTwo.Contains(currentChar.ToString()))
                {
                    char spCharOne = currentChar;
                    ReadNextChar();
                    char spCharTwo = currentChar;
                    if (specialSymbolsTwoPairs.Contains(" " +spCharOne + spCharTwo + " "))
                    {
                        ReadNextChar();
                        return new SpecialSymbolToken(spCharOne.ToString() + spCharTwo.ToString());
                    }
                    return new SpecialSymbolToken(spCharOne.ToString());
                }
                if (currentChar == EOF)
                {
                    return new EOFToken();
                }

                string str = currentChar.ToString();
                ReadNextChar();
                return new OtherToken(str);
            }
        }


        private void ReadNextChar()
        {
            int char1 = reader.Read();
            if (char1 < 0)
            {
                currentChar = EOF;
            }
            else
            {
                currentChar = (char)char1;
                if (currentChar == CR)
                {
                }
                else if (currentChar == LF)
                {
                }
            }
        }
    }
}
