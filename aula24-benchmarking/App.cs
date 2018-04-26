using System;

class App{
   static readonly Student st = 
    new Student(154134, "Ze Manel", 5243, "ze", new DateTime(1990, 12,7));
   static readonly LoggerEmit emit = new LoggerEmit();
   static readonly Logger reflect = new Logger();   
   static void Main() {
        emit.Add(typeof(Student));
        reflect.Add(typeof(Student));
   
        Console.WriteLine(reflect.Log(st));
        Console.WriteLine(emit.Log(st));
        
        NBench.Bench(App.BenchLoggerStudent, "Logger Student");
        NBench.Bench(App.BenchLoggerReflect, "Logger Reflect");
        NBench.Bench(App.BenchLoggerEmit, "Logger Emit");
    }
    
    static string LogStudent(Student st) {
        string res = "";
        res += "Nr: " + st.Nr;
        res += "Name: " + st.Name;
        res += "Group: " + st.Group;
        res += "Github: " + st.GithubId;
        res += "BirthDate: " + st.BirthDate;
        return res;
    }
    
    public static void BenchLoggerStudent() {
        LogStudent(st);
    }
    
    public static void BenchLoggerReflect() {
        reflect.Log(st);
    }
    
    public static void BenchLoggerEmit() {
        emit.Log(st);
    }
}

[Logger(LogEnum.Properties)]
public class Student
{
    public int Nr { get; set; }
    public string Name{ get; set; }
    public int Group{ get; set; }
    public string GithubId{ get; set; }
    public DateTime BirthDate {get; set; }

    public Student(int nr, String name, int group, string githubId, DateTime b)
    {
        this.Nr = nr;
        this.Name = name;
        this.Group = group;
        this.GithubId = githubId;
        this.BirthDate = b;
    }
}
