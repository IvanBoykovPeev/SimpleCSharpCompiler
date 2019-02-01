using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CompilerSimpleCSharp
{
    class Scanner
    {
        const char EOF = '\u001a';
        const char CR = '\r';
        const char LF = '\n';

        static readonly string keyWords = " scanf printf ";
        private static readonly string specialSymbolsOne = "();*";
        private static readonly string specialSymbolsTwo = "+=-";
        private static readonly string specialSymbolsTwoPairs = " ++ --";

        private TextReader reader;
        private char currentChar;

        public Scanner(TextReader reader)
        {
            this.reader = reader;
        }

        internal Token Next()
        {
            while (true)
            {
                if (currentChar >= 'a' && currentChar <= 'z' || currentChar >= 'A' && currentChar <= 'Z')
                {
                    StringBuilder sb = new StringBuilder();
                    while (currentChar >= 'a' && currentChar <= 'z' || currentChar >= 'A' && currentChar <= 'Z' || currentChar >= '0' && currentChar <= '9')
                    {
                        sb.Append(currentChar);
                        ReadNextChar();
                    }
                    string ident = sb.ToString();
                    if (keyWords.Contains(" " + ident + " "))
                    {
                        return new KeywordToken(ident);
                    }
                    return new IdentToken(sb.ToString());
                }
                else if (currentChar >= '0' && currentChar <= '9')
                {
                    StringBuilder sb = new StringBuilder();
                    while (currentChar >= '0' && currentChar <= '9')
                    {
                        sb.Append(currentChar);
                        ReadNextChar();
                    }
                    return new NumberToken(Convert.ToInt64(sb.ToString()));
                }
                else if (currentChar == CR || currentChar == LF || currentChar == ' ' || currentChar == '\t')
                {
                    ReadNextChar();
                    continue;
                }
                else if (specialSymbolsOne.Contains(currentChar.ToString()))
                {
                    char spChar = currentChar;
                    ReadNextChar();
                    return new SpecialSymbolToken(spChar.ToString());
                }
                else if (specialSymbolsTwo.Contains(currentChar.ToString()))
                {
                    char spCharOne = currentChar;
                    ReadNextChar();
                    char spCharTwo = currentChar;
                    if (specialSymbolsTwoPairs.Contains(" " + spCharOne + spCharTwo + " "))
                    {
                        ReadNextChar();
                        return new SpecialSymbolToken(spCharOne.ToString() + spCharTwo.ToString());
                    }
                    return new SpecialSymbolToken(spCharOne.ToString());
                }
                else if (currentChar == EOF)
                {
                    return new EOFToken();
                }
                else
                {
                    string str = currentChar.ToString();
                    ReadNextChar();
                    //return new OtherToken(str.ToString());
                }
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
