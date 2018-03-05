using System;
using System.Reflection;

public class Student
{
    readonly int nr;
    readonly string name;
    readonly int group;
    readonly string githubId;

    public Student(int nr, String name, int group, string githubId)
    {
        this.nr = nr;
        this.name = name;
        this.group = group;
        this.githubId = githubId;
    }
}

struct Point{
    int x, y;
    public Point(int x, int y) {
        this.x = x;
        this.y = y;
    }
    public double Module() {
        return Math.Sqrt(x*x + y*y);
    }
}

class Logger {
    public static void Log(object target) {
        Type klass = target.GetType();
        // By default BindingFlags = Public Static or Instance
        FieldInfo[] fs = klass.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
        string res = klass.Name + "[";
        foreach(FieldInfo f in fs) {
            res += f.Name + ": ";
            res += f.GetValue(target) + ",";
        }
        MethodInfo[] ms = klass.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        foreach(MethodInfo m in ms) {
            if(m.GetParameters().Length == 0 && m.ReturnType != typeof(void)) {
                res += m.Name + ": ";
                res += m.Invoke(target, new object[0]) + ",";
            }
        }
        res += "]";
        Console.WriteLine(res);
    }
}

public class App {
    public static void Main(String [] args) {
        Point p = new Point(5, 7);
        Console.WriteLine(p); // ToString de Object
        Logger.Log(p);
        
        Student s = new Student(154134, "Ze Manel", 5243, "ze");
        Console.WriteLine(s); // ToString de Object
        Logger.Log(s);
        
    }
    
}