using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public interface MemberFormat {
    string Format(object val);
}
interface MemberData {
    void Data(object target);
}
class FieldData : MemberData {
    readonly FieldInfo f;
    readonly Logger logger;
    readonly FormatAttribute fa;
    public FieldData(Logger logger, FieldInfo f) {
        this.f = f; 
        this.logger = logger; 
        this.fa = (FormatAttribute) f.GetCustomAttribute(typeof(FormatAttribute), false);
    }
    public void Data(object target) {
        Console.Write(f.Name + ": ");
        if(fa != null)
            Console.Write(fa.Format(f.GetValue(target)));
        else
            logger.Log(f.GetValue(target));
        Console.Write(",");
    }
}
class MethodData : MemberData {
    MethodInfo m;
    Logger logger;
    public MethodData(Logger logger, MethodInfo m) {this.m = m; this.logger = logger; }
    public void Data(object target) {
        Console.Write(m.Name + ": ");
        FormatAttribute fa = (FormatAttribute) m.GetCustomAttribute(typeof(FormatAttribute), false);
        if(fa != null)
            Console.Write(fa.Format(m.Invoke(target, new object[0])));
        else
            logger.Log(m.Invoke(target, new object[0]));
        Console.Write(",");
    }
}
class PropertyData : MemberData {
    PropertyInfo p;
    Logger logger;
    public PropertyData(Logger logger, PropertyInfo p) {this.p = p; this.logger = logger; }
    public void Data(object target) {
        Console.Write(p.Name + ": ");
        FormatAttribute fa = (FormatAttribute) p.GetCustomAttribute(typeof(FormatAttribute), false);
		if(fa != null)
            Console.Write(fa.Format(p.GetValue(target)));
        else
            logger.Log(p.GetValue(target));
        Console.Write(",");
    }
}

public enum LogEnum {Properties, Fields, Methods}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple=true)]
public class LoggerAttribute : Attribute{
    public LogEnum LogMember {get; set; }
    public LoggerAttribute(LogEnum val) {
        LogMember = val;
    }
}
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Property , AllowMultiple=false)]
public class IgnoreAttribute : Attribute{

}
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Property , AllowMultiple=false)]
public class FormatAttribute : Attribute{
    MemberFormat formatter;
    public FormatAttribute(Type klass) {
        if(!typeof(MemberFormat).IsAssignableFrom(klass))
            throw new ArgumentException("Parameter should implement MemberFormat!");
        formatter = (MemberFormat) Activator.CreateInstance(klass);
    }
    public string Format(object val) {
        return formatter.Format(val);
    }
}


public class Logger {
    private Dictionary<Type, MemberData[]> members = new Dictionary<Type, MemberData[]>();
    
    public void Add(Type klass) {
        List<MemberData> res = new List<MemberData>();
        object[] attrs = klass.GetCustomAttributes(typeof(LoggerAttribute), false);
		foreach (object o in attrs) {
			LoggerAttribute l = (LoggerAttribute) o;
			if (l.LogMember.Equals(LogEnum.Properties)) LoadProperties(klass, res);
			else if (l.LogMember.Equals(LogEnum.Fields)) LoadFields(klass, res);
			else if (l.LogMember.Equals(LogEnum.Methods)) LoadMethods(klass, res);
		}
        members.Add(klass, res.ToArray());
    }
    
    void LoadFields(Type klass, List<MemberData> res) {
        FieldInfo[] fs = klass.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
        foreach(FieldInfo f in fs) {
            if(!f.IsDefined(typeof(IgnoreAttribute), false))
                res.Add(new FieldData(this, f));
        }
    }
    void LoadMethods(Type klass, List<MemberData> res) {
        MethodInfo[] ms = klass.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        foreach(MethodInfo m in ms) {
            if(m.GetParameters().Length == 0 && m.ReturnType != typeof(void)) {
                if(!m.IsDefined(typeof(IgnoreAttribute), false))
                    res.Add(new MethodData(this, m));
            }
        }
    }
    void LoadProperties(Type klass, List<MemberData> res) {
        PropertyInfo[] ps = klass.GetProperties();
        foreach(PropertyInfo p in ps) {
            if(!p.IsDefined(typeof(IgnoreAttribute), false))
                res.Add(new PropertyData(this, p));
        }
    }
    public void Log(object target) {
        Type klass = target.GetType();
        Type key = klass.IsArray? klass.GetElementType() : klass;
        MemberData[] val;
        if(!members.TryGetValue(key, out val)) { // out val <=> &val
            Console.Write(target.ToString());
            return;
        }
        if(!klass.IsArray) LogObject(val, target);
        else LogArray(val, target);
    }
    void LogArray(MemberData[] val, object target) {
        bool isBidimensional = target.GetType().GetElementType().IsArray;
        Console.Write("{");
        IEnumerable src = (IEnumerable) target;
        foreach(object elem in src) {
            if(isBidimensional) LogArray(val, elem); // via Iterator
            else LogObject(val, elem);
        }
        Console.WriteLine("}");
    }
    
    void LogObject(MemberData[] val, object target) {      
        Console.Write("[");
        foreach(MemberData m in val) {
            m.Data(target);
        }
        Console.Write("]");
    }
}
