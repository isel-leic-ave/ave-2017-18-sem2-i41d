using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;


public interface DynamicLogger {
    string Log(object target);
}

public class LoggerEmit {

    private static readonly MethodInfo concat = typeof(string)
           .GetMethod("Concat", new Type[]{typeof(object), typeof(object)});

   private static readonly MethodInfo concatStr = typeof(string)
           .GetMethod("Concat", new Type[]{typeof(string), typeof(string)});
           
    private Dictionary<Type, DynamicLogger> loggers = new Dictionary<Type, DynamicLogger>();
    
    public void Add(Type klass) {
        // 1. Gerar uma classe que implementa DynamicLogger
        // 2. Instanciar a classe 1.
        // 3. Guardar em loggers
        DynamicLogger dynLogger = BuildLogger(klass);
        loggers.Add(klass, dynLogger);
    }
    
    public string Log(object target) {
        Type klass = target.GetType();
        Type key = klass.IsArray? klass.GetElementType() : klass;
        DynamicLogger logger;
        if(!loggers.TryGetValue(key, out logger)) { // out val <=> &val
            return target.ToString();
        }
        if(klass.IsArray){
            IEnumerable arr = (IEnumerable) target;
            string res = "[";
            foreach(object elem in arr) res += logger.Log(elem) + ",";
            return res + "]";
        } else            
            return logger.Log(target);
    }
    
    // 1. Gerar uma classe que implementa DynamicLogger
    // 2. Instanciar a classe 1. e retornar
    DynamicLogger BuildLogger(Type modelKlass) {
            AssemblyName aName = new AssemblyName("Logger" + modelKlass.Name);
        AssemblyBuilder ab = 
            AppDomain.CurrentDomain.DefineDynamicAssembly(
                aName, 
                AssemblyBuilderAccess.RunAndSave);

        // For a single-module assembly, the module name is usually
        // the assembly name plus an extension.
        ModuleBuilder mb = 
            ab.DefineDynamicModule(aName.Name, aName.Name + ".dll");

        TypeBuilder tb = mb.DefineType(
            "Logger" + modelKlass.Name, 
             TypeAttributes.Public,
             typeof(object),
             new Type[]{ typeof(DynamicLogger)});

        BuildMethodLog(modelKlass, tb);
             
        // Finish the type.
        Type t = tb.CreateType();

        // The following line saves the single-module assembly. This
        // requires AssemblyBuilderAccess to include Save. 
        // 
        ab.Save(aName.Name + ".dll");
        return (DynamicLogger) Activator.CreateInstance(t);
    }
    
    static void BuildMethodLog(Type modelKlass, TypeBuilder logger) {
        // Overrides Log() method
        MethodBuilder log = logger.DefineMethod("Log",
             MethodAttributes.Public | MethodAttributes.ReuseSlot | 
             MethodAttributes.Virtual | MethodAttributes.HideBySig,
            typeof(string), 
            new Type[]{typeof(object)});
        ILGenerator il = log.GetILGenerator();
        
        // Student/Point/Triangle... res = (Student) target
        LocalBuilder res = il.DeclareLocal(modelKlass);
        il.Emit(OpCodes.Ldarg_1);
        if(typeof(ValueType).IsAssignableFrom(modelKlass))
            il.Emit(OpCodes.Unbox_Any, modelKlass);
        else 
            il.Emit(OpCodes.Castclass, modelKlass);
        il.Emit(OpCodes.Stloc, res);
        
        // "" + std.Nr
        il.Emit(OpCodes.Ldstr, "");
        foreach(PropertyInfo p in modelKlass.GetProperties()) {
            if(typeof(ValueType).IsAssignableFrom(modelKlass)){
                il.Emit(OpCodes.Ldloca_S, res);
                il.Emit(OpCodes.Call,  p.GetGetMethod());
            }
            else {
                il.Emit(OpCodes.Ldloc, res);
                il.Emit(OpCodes.Callvirt,  p.GetGetMethod());
            }
            if(typeof(ValueType).IsAssignableFrom(p.PropertyType))
                il.Emit(OpCodes.Box, p.PropertyType);
            // !!!! Falta chamar logger.Log(val) onde val é o valor da propriedade
            il.Emit(OpCodes.Call, concat);
            il.Emit(OpCodes.Ldstr, ", ");
            il.Emit(OpCodes.Call, concatStr);
        }
        il.Emit(OpCodes.Ret);
    }
}
