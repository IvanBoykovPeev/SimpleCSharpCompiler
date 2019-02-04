using System;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;

namespace CompilerSimpleCSharp
{
    internal class Emit
    {
        private AssemblyBuilder assemblyBuilder;
        private ModuleBuilder moduleBuilder;
        private Table symbolTable;
        private TypeBuilder typeBuilder;
        private ConstructorBuilder constructorBuilder;
        private ILGenerator ilGenerator;
        private MethodBuilder methodBuilder;
        private string executableName;

        public Emit(string name, Table symbolTable)
        {
            this.symbolTable = symbolTable;
            this.executableName = name;

            AssemblyName assemblyName = new AssemblyName();
            assemblyName.Name = Path.GetFileNameWithoutExtension(name);

            string moduleName = Path.GetFileName(name);
            assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Save);
            moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName + "Module", moduleName);
            
        }

        internal void InitProgram()
        {
            //Създава клас
            typeBuilder = moduleBuilder.DefineType("program");

            //конструктор
            constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[] {});
            ILGenerator iLGenerator = constructorBuilder.GetILGenerator();
            iLGenerator.Emit(OpCodes.Ret);


            //main метод
            methodBuilder = typeBuilder.DefineMethod("Main", MethodAttributes.Public | MethodAttributes.Static);
            methodBuilder.InitLocals = true;
            ilGenerator = methodBuilder.GetILGenerator();
        }

        internal Type WriteExecutable()
        {
            
            ilGenerator.Emit(OpCodes.Ret);

            Type retValue = typeBuilder.CreateType();
            assemblyBuilder.SetEntryPoint(methodBuilder);
            assemblyBuilder.Save(executableName);
            return retValue;
        }

        internal void AddGetLocalVar(LocalVariableInfo localVariableInfo)
        {
            ilGenerator.Emit(OpCodes.Ldloc, (LocalBuilder)localVariableInfo);
        }

        internal void AddLocalVarAssigment(LocalVariableInfo localVariableInfo)
        {
            ilGenerator.Emit(OpCodes.Stloc, (LocalBuilder)localVariableInfo);
        }

        internal LocalBuilder AddLocalVar(string value, Type type)
        {
            LocalBuilder result = ilGenerator.DeclareLocal(type);
            if (!type.IsValueType)
            {
                ilGenerator.Emit(OpCodes.Newobj, type);
                ilGenerator.Emit(OpCodes.Stloc, result);
            }

            return result;
        }

        internal void AddPop()
        {
            ilGenerator.Emit(OpCodes.Pop);
        }

        internal void AddGetNumber(long value)
        {
            if (value >= Int32.MinValue && value <= Int32.MaxValue)
            {
                ilGenerator.Emit(OpCodes.Ldc_I4, (Int32)value);
            }
            else
            {
                ilGenerator.Emit(OpCodes.Ldc_I8, value);
            }
        }        

        internal void AddPlus()
        {
            ilGenerator.Emit(OpCodes.Add);
        }        

        internal void AddMinus()
        {
            ilGenerator.Emit(OpCodes.Sub);
        }

        internal void AddMul()
        {
            ilGenerator.Emit(OpCodes.Mul);
        }

        internal void AddDiv()
        {
            ilGenerator.Emit(OpCodes.Div);
        }

        internal void AddRem()
        {
            ilGenerator.Emit(OpCodes.Rem);
        }

        internal void EmitReadLine()
        {
            MethodInfo readLineMetodInfo = typeof(Console).GetMethod("ReadLine", new Type[0]);
            MethodInfo convertInt32MetodInfo = typeof(Convert).GetMethod("ToInt32", new Type[] { typeof(string) });

            ilGenerator.EmitCall(OpCodes.Call, readLineMetodInfo, null);
            ilGenerator.EmitCall(OpCodes.Call, convertInt32MetodInfo, null);
        }

        internal void EmitWriteLine()
        {

            MethodInfo writeMetodInfo = typeof(Console).GetMethod("WriteLine", new Type[] { typeof(int) });
            ilGenerator.EmitCall(OpCodes.Call, writeMetodInfo, null);
        }

        internal void ReadKey()
        {
            ilGenerator.Emit(OpCodes.Call, (typeof(Console)).GetMethod("ReadKey", new Type[0]));
        }

        internal void AddDuplicate()
        {
            ilGenerator.Emit(OpCodes.Dup);
        }
    }
}
