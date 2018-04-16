using System;
using System.Reflection;
using System.Reflection.Emit;

public class App {
    public static void Main(String [] args) {
        Type klass = Build();
        object o = Activator.CreateInstance(klass, new object[]{89});
        Console.WriteLine(o.ToString());
    }
    
    
    public static Type Build() {
        AssemblyName aName = new AssemblyName("DynamicAssemblyExample");
        AssemblyBuilder ab = 
            AppDomain.CurrentDomain.DefineDynamicAssembly(
                aName, 
                AssemblyBuilderAccess.RunAndSave);

        // For a single-module assembly, the module name is usually
        // the assembly name plus an extension.
        ModuleBuilder mb = 
            ab.DefineDynamicModule(aName.Name, aName.Name + ".dll");

        TypeBuilder tb = mb.DefineType(
            "MyDynamicType", 
             TypeAttributes.Public);

         // Add a private field of type int (Int32).
        FieldBuilder fbNumber = tb.DefineField(
            "m_number", 
            typeof(int), 
            FieldAttributes.Private);

        // Define a constructor that takes an integer argument and 
        // stores it in the private field. 
        Type[] parameterTypes = { typeof(int) };
        ConstructorBuilder ctor = tb.DefineConstructor(
            MethodAttributes.Public, 
            CallingConventions.Standard, 
            parameterTypes);
        
        ILGenerator il = ctor.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldarg_1);
        il.Emit(OpCodes.Stfld, fbNumber);
        il.Emit(OpCodes.Ret);
        
        // Overrides ToString() method
        MethodBuilder toStr = tb.DefineMethod("ToString",
             MethodAttributes.Public | MethodAttributes.ReuseSlot | 
             MethodAttributes.Virtual | MethodAttributes.HideBySig,
            typeof(string), 
            new Type[0]);
            
        MethodInfo concat = typeof(string)
            .GetMethod("Concat", new Type[]{typeof(object), typeof(object)});
        il = toStr.GetILGenerator();
        il.Emit(OpCodes.Ldstr, "m_number = ");
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldfld, fbNumber);
        il.Emit(OpCodes.Box, fbNumber.FieldType);
        il.Emit(OpCodes.Call, concat);
        il.Emit(OpCodes.Ret);

        // Finish the type.
        Type t = tb.CreateType();

        // The following line saves the single-module assembly. This
        // requires AssemblyBuilderAccess to include Save. 
        // 
        ab.Save(aName.Name + ".dll");
        return t;
    }    
}