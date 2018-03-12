using System;
using System.Collections.Generic;
using System.Reflection;

class BirthDateFormat : MemberFormat {
    public string Format(object val) {
        DateTime dt = (DateTime) val;
        return dt.Month + " of " + dt.Year;
    }
}

class FirstNameFormat : MemberFormat {
    public string Format(object val) {
        string name  = (string) val;
        return name.Split(' ')[0];
    }
}


[Logger(LogEnum.Properties)]
public class Student
{
    public int Nr { get; set; }
    [Format(typeof(FirstNameFormat))]public string Name{ get; set; }
    public int Group{ get; set; }
    [Ignore] public string GithubId{ get; set; }
    [Format(typeof(BirthDateFormat))]public DateTime BirthDate {get; set; }

    public Student(int nr, String name, int group, string githubId, DateTime b)
    {
        this.Nr = nr;
        this.Name = name;
        this.Group = group;
        this.GithubId = githubId;
        this.BirthDate = b;
    }
}

[Logger(LogEnum.Properties)]
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

[Logger(LogEnum.Fields)]
class Triangle {
    readonly Point p1, p2, p3;
    public Triangle(Point p1, Point p2, Point p3) {
        this.p1 = p1;
        this.p2 = p2;
        this.p3 = p3;
    }
}

public class App {
    public static void Main(String [] args) {
        Logger logger = new Logger();
        logger.Add(typeof(Point));
        logger.Add(typeof(Student));
        logger.Add(typeof(Triangle));
        
        logger.Log(new Point(5, 7));
        logger.Log(new Point(11, 7));
        logger.Log(new Point(7, 9));
        Console.WriteLine();
        
        Student s = new Student(154134, "Ze Manel", 5243, "ze", new DateTime(1990, 12,7));
        logger.Log(s);
        Console.WriteLine();
        
        Point [] pts = {new Point(5, 7), new Point(11, 7), new Point(7, 9)};
        logger.Log(pts);
        
        Triangle t = new Triangle(new Point(5, 7), new Point(11, 7), new Point(7, 9));
        logger.Log(t);
    }
    
}