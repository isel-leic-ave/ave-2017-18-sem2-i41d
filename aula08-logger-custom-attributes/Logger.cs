using System;
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

public enum Log {Properties, Fields, Methods}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple=true)]
public class LoggerAttribute : Attribute{
    public Log LogMember {get; set; }
    public LoggerAttribute(Log val) {
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
        // LoadFields(klass, res);
        // LoadMethods(klass, res);
        LoadProperties(klass, res);
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
        string res = klass.Name + "[";
        foreach(MemberData m in members[klass]) {
            res += m.Data(target);
        }
        res += "]";
        Console.WriteLine(res);
    }
}
