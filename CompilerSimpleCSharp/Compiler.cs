using System;
using System.IO;

namespace CompilerSimpleCSharp
{
    static class Compiler
    {
        public static string fileName = @"C:\Users\blade\Desktop\inSimple.txt";
        internal static bool Compile()
        {
            TextReader reader = new StreamReader(fileName);
            Scanner scanner = new Scanner(reader);
            Table symbolTable = new Table();
            Emit emiter = new Emit(Path.GetFileNameWithoutExtension(fileName) + ".exe", symbolTable);
            Parser parser = new Parser(scanner, symbolTable, emiter);

            emiter.InitProgram();
            bool isProgram = parser.Parse();

            if (isProgram)
            {
                Console.WriteLine("The program compile SUCCESS! ");
                Console.WriteLine(symbolTable);
                emiter.WriteExecutable();
                return true;
            }
            else
            {
                Console.WriteLine("The program not compile");
                return false;
            }
            //Token token = scanner.Next();
            //while (!(token is EOFToken))
            //{
            //    Console.WriteLine(token.ToString());
            //    token = scanner.Next();
            //}

            
            
        }
    }
}