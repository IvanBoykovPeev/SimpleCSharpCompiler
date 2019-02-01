using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using CompilerSimpleCSharp.TableSymbols;

namespace CompilerSimpleCSharp
{
    internal class Table
    {
        private Stack<Dictionary<string, TableSymbol>> symbolTable;
        private Dictionary<string, TableSymbol> fieldScope;

        public Table()
        {
            symbolTable = new Stack<Dictionary<string, TableSymbol>>();
            this.fieldScope = BeginScope();
        }

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();
            int i = symbolTable.Count;
            s.AppendFormat("=========\n");
            foreach (var table in symbolTable)
            {
                s.AppendFormat("---[{0}]---\n", i--);
                foreach (var row in table)
                {
                    s.AppendFormat("[{0}] {1}\n", row.Key, row.Value);
                }
            }
            s.AppendFormat("=========\n");
            return s.ToString();
        }

        internal Dictionary<string, TableSymbol> BeginScope()
        {
            symbolTable.Push(new Dictionary<string, TableSymbol>());
            return symbolTable.Peek();
        }

        public LocalVariableSymbol AddLocalVar(IdentToken token, LocalBuilder localBuilder)
        {
            LocalVariableSymbol result = new LocalVariableSymbol(token, localBuilder);
            symbolTable.Peek().Add(token.value, result);
            return result;
        }

        internal TableSymbol GetSymbol(string ident)
        {
            TableSymbol result;
            foreach (Dictionary<string, TableSymbol> table in symbolTable)
            {
                if (table.TryGetValue(ident, out result))
                {
                    return result;
                }
                return result;
            }
            Console.WriteLine("Return null");
            return null;
        }

        public bool ExistCurrentScopeSymbol(string ident)
        {
            return symbolTable.Peek().ContainsKey(ident);
        }
    }
}
    