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


public class Logger {
    private Dictionary<Type, MemberData[]> members = new Dictionary<Type, MemberData[]>();
    
    public void Add(Type klass) {
        List<MemberData> res = new List<MemberData>();
        LoadFields(klass, res);
        // LoadMethods(klass, res);
        // LoadProperties(klass, res);
        members.Add(klass, res.ToArray());
    }
    
    void LoadFields(Type klass, List<MemberData> res) {
        FieldInfo[] fs = klass.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
        foreach(FieldInfo f in fs) {
                res.Add(new FieldData(f));
        }
    }
    void LoadMethods(Type klass, List<MemberData> res) {
        MethodInfo[] ms = klass.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        foreach(MethodInfo m in ms) {
            if(m.GetParameters().Length == 0 && m.ReturnType != typeof(void)) {
                res.Add(new MethodData(m));
            }
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
