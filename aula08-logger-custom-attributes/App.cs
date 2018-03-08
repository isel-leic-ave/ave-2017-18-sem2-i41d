using System;
using System.Collections.Generic;
using System.Reflection;

[Logger(Log.Properties)]
public class Student
{
    public int Nr { get; set; }
    public string Name{ get; set; }
    public int Group{ get; set; }
    [Ignore] public string GithubId{ get; set; }

    public Student(int nr, String name, int group, string githubId)
    {
        this.Nr = nr;
        this.Name = name;
        this.Group = group;
        this.GithubId = githubId;
    }
}

[Logger(Log.Fields)]
[Logger(Log.Methods)]
struct Point{
    int x, y;
    public Point(int x, int y) {
        this.x = x;
        this.y = y;
    }
    
    // NÃO há relação entre campo x <-> propriedade X na METADATA.
    [Ignore] public int X {
        get{return x;}
        set{ x = value; }
    }
    [Ignore] public int Y { // RW
        get{return y;}
        set{ y = value; }
    }
    
    public double Module { // READ ONLY
        get {
            return Math.Sqrt(x*x + y*y);
        }
    }
}


public class App {
    public static void Main(String [] args) {
        Logger logger = new Logger();
        logger.Add(typeof(Point));
        logger.Add(typeof(Student));
        
        logger.Log(new Point(5, 7));
        logger.Log(new Point(11, 7));
        logger.Log(new Point(7, 9));
        
        Student s = new Student(154134, "Ze Manel", 5243, "ze");
        logger.Log(s);
    }
    
}