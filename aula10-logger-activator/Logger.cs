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
abstract class AbstractMemberData : MemberData {
	protected readonly Logger logger;
	protected readonly FormatAttribute fa;
	
	public AbstractMemberData(Logger logger, MemberInfo info) {
		this.logger = logger;
		this.fa = (FormatAttribute) info.GetCustomAttribute(typeof(FormatAttribute), false);
	}
	
	public abstract void Data(object target);
}

class FieldData : AbstractMemberData {
	FieldInfo f;
	
    public FieldData(Logger logger, FieldInfo f) : base(logger,f) {
		this.f = f;
	}
	
    public override void Data(object target) {
        Console.Write(f.Name + ": ");
        if(fa != null)
            Console.Write(fa.Format(f.GetValue(target)));
        else
            logger.Log(f.GetValue(target));
        Console.Write(",");
    }
}
class MethodData : AbstractMemberData {
	MethodInfo m;
	
    public MethodData(Logger logger, MethodInfo m) : base(logger,m) {
		this.m = m;
	}
	
    public override void Data(object target) {
        Console.Write(m.Name + ": ");
        if(fa != null)
            Console.Write(fa.Format(m.Invoke(target, new object[0])));
        else
            logger.Log(m.Invoke(target, new object[0]));
        Console.Write(",");
    }
}
class PropertyData : AbstractMemberData {
	PropertyInfo p;
	
    public PropertyData(Logger logger, PropertyInfo p) : base(logger,p) {
		this.p = p;
	}
	
    public override void Data(object target) {
        Console.Write(p.Name + ": ");
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
