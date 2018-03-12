using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

interface MemberData {
    string Data(object target);
}
class FieldData : MemberData {
    FieldInfo f;
    public FieldData(FieldInfo f) {this.f = f; }
    public string Data(object target) {
        string res = f.Name + ": ";
        res += f.GetValue(target) + ",";
        return res;
    }
}
class MethodData : MemberData {
    MethodInfo m;
    public MethodData(MethodInfo m) {this.m = m; }
    public string Data(object target) {
        string res = m.Name + ": ";
        res += m.Invoke(target, new object[0]) + ",";
        return res;
    }
}
class PropertyData : MemberData {
    PropertyInfo p;
    public PropertyData(PropertyInfo p) {this.p = p; }
    public string Data(object target) {
        string res = p.Name + ": ";
		res += p.GetValue(target) + ",";
        return res;
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
                res.Add(new FieldData(f));
        }
    }
    void LoadMethods(Type klass, List<MemberData> res) {
        MethodInfo[] ms = klass.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        foreach(MethodInfo m in ms) {
            if(m.GetParameters().Length == 0 && m.ReturnType != typeof(void)) {
                if(!m.IsDefined(typeof(IgnoreAttribute), false))
                    res.Add(new MethodData(m));
            }
        }
    }
    void LoadProperties(Type klass, List<MemberData> res) {
        PropertyInfo[] ps = klass.GetProperties();
        foreach(PropertyInfo p in ps) {
            if(!p.IsDefined(typeof(IgnoreAttribute), false))
                res.Add(new PropertyData(p));
        }
    }
    public void Log(object target) {
        Type klass = target.GetType();
        Type key = klass.IsArray? klass.GetElementType() : klass;
        MemberData[] val;
        if(!members.TryGetValue(key, out val)) { // out val <=> &val
            Console.WriteLine(target.ToString());
            return;
        }
        if(!klass.IsArray) LogObject(val, target);
        else LogArray(val, target);
    }
    void LogArray(MemberData[] val, object target) {
        bool isBidimensional = target.GetType().GetElementType().IsArray;
        Console.Write("{");
        /*
        Array arr = (Array) target;
        for(int i = 0; i < arr.Length; i++) {
            if(isBidimensional) LogArray(val, arr.GetValue(i)); // via Reflection API
            else LogObject(val, arr.GetValue(i));
        }
        */
        IEnumerable src = (IEnumerable) target;
        foreach(object elem in src) {
            if(isBidimensional) LogArray(val, elem); // via Iterator
            else LogObject(val, elem);
        }
        Console.WriteLine("}");
    }
    
    void LogObject(MemberData[] val, object target) {      
        string res = "[";
        foreach(MemberData m in val) {
            res += m.Data(target);
        }
        res += "]";
        Console.WriteLine(res);
    }
}
