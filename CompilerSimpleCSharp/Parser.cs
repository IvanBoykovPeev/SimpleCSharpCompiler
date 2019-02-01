using System;
using System.Reflection.Emit;
using CompilerSimpleCSharp.TableSymbols;

namespace CompilerSimpleCSharp
{
    internal class Parser
    {
        private Scanner scanner;
        private Token currentToken;
        private Table symbolTable;
        private Emit emiter;


        public Parser(Scanner scanner, Table symbolTable, Emit emiter)
        {
            this.emiter = emiter;
            this.scanner = scanner;
            this.symbolTable = symbolTable;
            ReadNextToken();
        }

        internal bool Parse()
        {
            while (IsStatement()) ;
            emiter.ReadKey();
            emiter.AddPop();
            return (currentToken is EOFToken);
        }

        #region IsA...
        private bool IsStatement()
        {

            if (IsExpression())
            {
                if (!CheckSpecialSymbol(";"))
                {
                    Console.WriteLine("Expected special symbol ';41'");
                    return false;
                }

                emiter.AddPop();
                return true;
            }
            if (!CheckSpecialSymbol(";48"))
                return false;

            return true;
        }

        private bool IsExpression()
        {
            if (IsBitwiseExpression())
            {
                return true;
            }
            return false;
        }

        private bool IsBitwiseExpression()
        {
            if (IsAdditiveExpression())
            {
                return true;
            }
            return false;
        }

        private bool IsAdditiveExpression()
        {
            if (isMultiplicativeExpression())
            {
                if (CheckSpecialSymbol("+"))
                {
                    if (!IsAdditiveExpression())
                    {
                        Console.WriteLine("Multiplicative Expression Required");
                        return false;
                    }
                    emiter.AddPlus();
                }
                return true;
            }
            return false;
        }

        private bool isMultiplicativeExpression()
        {
            if (IsPrimaryExpression())
            {
                if (CheckSpecialSymbol("*"))
                {
                    if (!isMultiplicativeExpression())
                    {
                        Console.WriteLine("Primary Expression Required!");
                        return false;
                    }
                    emiter.AddMul();
                }
                return true;
            }
            return false;
        }

        ///    PrimaryExpression = Ident['=' Expression] | '~' PrimaryExpression |
        ///      '++' Ident | '--' Ident |Ident/ '++' | Ident '--' | 
		///     Number | PrintFunc | ScanfFunc | '(' Expression ')'
        private bool IsPrimaryExpression()
        {

            Token tempToken = currentToken;

            if (CheckIdent())
            {
                LocalVariableSymbol localVar = this.GetLocalVariableSymbol(tempToken);

                if (CheckSpecialSymbol("="))
                {
                    if (!IsExpression())
                    {
                        Console.WriteLine("Expected Expression");
                        return false;
                    }
                    emiter.AddLocalVarAssigment(localVar.localVariableInfo);
                    emiter.AddGetLocalVar(localVar.localVariableInfo);
                    return true;
                }
                if (CheckSpecialSymbol("++"))
                {
                    emiter.AddGetLocalVar(localVar.localVariableInfo);
                    emiter.AddDuplicate();
                    emiter.AddGetNumber(1);
                    emiter.AddPlus();
                    emiter.AddLocalVarAssigment(localVar.localVariableInfo);
                    return true;
                }
                emiter.AddGetLocalVar(localVar.localVariableInfo);
                return true;
            }
            if (CheckSpecialSymbol("("))
            {
                if (!IsExpression())
                {
                    Console.WriteLine("Expected Expresion137'");
                    return false;
                }
                if (!CheckSpecialSymbol(")"))
                {
                    Console.WriteLine("Expected SpecialSymbol ')141'");
                    return false;
                }
                return true;
            }
            if (CheckNumber())
            {
                emiter.AddGetNumber(((NumberToken)tempToken).Value);
                return true;
            }
            if (CheckKeyword("scanf"))
            {
                if (!CheckSpecialSymbol("("))
                {
                    Console.WriteLine("Expected special symbol '('");
                    return false;
                }
                if (!CheckSpecialSymbol(")"))
                {
                    Console.WriteLine("Expected special symbol ')'");
                    return false;
                }
                emiter.EmitReadLine();
                return true;
            }
            if (CheckKeyword("printf"))
            {
                if (!CheckSpecialSymbol("("))
                {
                    Console.WriteLine("Expected special symbol '('");
                    return false;
                }
                if (!IsExpression())
                {
                    Console.WriteLine("Expected special symbol 'Expr'");
                    return false;
                }
                else
                {
                    emiter.EmitWriteLine();
                    emiter.AddGetNumber(0);
                }
                if (!CheckSpecialSymbol(")"))
                {
                    Console.WriteLine("Expected special symbol ')'");
                    return false;
                }
                return true;
            }
            if (CheckSpecialSymbol("++"))
            {
                tempToken = currentToken;
                if (!CheckIdent())
                {
                    Console.WriteLine("Ident Required!");
                    return false;
                }
                LocalVariableSymbol localVariable = this.GetLocalVariableSymbol(tempToken);
                emiter.AddGetLocalVar(localVariable.localVariableInfo);
                emiter.AddGetNumber(1);
                emiter.AddPlus();
                emiter.AddDuplicate();
                emiter.AddLocalVarAssigment(localVariable.localVariableInfo);
                return true;
            }

            return false;
        }
        #endregion

        #region Check Token
        private bool CheckSpecialSymbol(string symbol)
        {
            bool result = (currentToken is SpecialSymbolToken) && ((SpecialSymbolToken)currentToken).value == symbol;
            if (result) ReadNextToken();
            return result;
        }

        private bool CheckIdent()
        {
            bool result = (currentToken is IdentToken);
            if (result) ReadNextToken();
            return result;
        }

        private bool CheckKeyword(string keyword)
        {
            bool result = (currentToken is KeywordToken) && ((KeywordToken)currentToken).value == keyword;
            if (result) ReadNextToken();
            return result;
        }
        private bool CheckNumber()
        {
            bool result = (currentToken is NumberToken);
            if (result)
            {
                ReadNextToken();
            }
            return result;
            
        }
        #endregion

        private LocalVariableSymbol GetLocalVariableSymbol(Token token)
        {
            IdentToken tempIdent = (IdentToken)token;
            LocalVariableSymbol localVar;

            if (!symbolTable.ExistCurrentScopeSymbol(tempIdent.value))
            {
                LocalBuilder tmpVar = emiter.AddLocalVar(tempIdent.value, typeof(int));
                localVar = symbolTable.AddLocalVar(tempIdent, tmpVar);
            }
            else
            {
                localVar = (LocalVariableSymbol)symbolTable.GetSymbol(tempIdent.value);
            }
            return localVar;
        }

        private void ReadNextToken()
        {
            currentToken = scanner.Next();
        }
    }
}