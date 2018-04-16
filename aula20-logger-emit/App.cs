using System;
using System.Collections.Generic;
using System.Reflection;

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


public struct Point{
    int x, y;
    public Point(int x, int y) {
        this.x = x;
        this.y = y;
    }
    
    // NÃO há relação entre campo x <-> propriedade X na METADATA.
    public int X {
        get{return x;}
        set{ x = value; }
    }
    public int Y { // RW
        get{return y;}
        set{ y = value; }
    }
    
    public double Module { // READ ONLY
        get {
            return Math.Sqrt(x*x + y*y);
        }
    }
}


public class Triangle {
    readonly Point p1, p2, p3;
    public Triangle(Point p1, Point p2, Point p3) {
        this.p1 = p1;
        this.p2 = p2;
        this.p3 = p3;
    }
}

public class App {
    public static void Main(String [] args) {
        LoggerEmit logger = new LoggerEmit();
        logger.Add(typeof(Point));
        logger.Add(typeof(Student));
        logger.Add(typeof(Triangle));
        
        // Console.WriteLine(logger.Log(new Point(5, 7)));
        // Console.WriteLine(logger.Log(new Point(11, 7)));
        // Console.WriteLine(logger.Log(new Point(7, 9)));
        // Console.WriteLine();
        
        Student s = new Student(154134, "Ze Manel", 5243, "ze", new DateTime(1990, 12,7));
        Console.WriteLine(logger.Log(s));
        Console.WriteLine();
        
        /*
        Point [] pts = {new Point(5, 7), new Point(11, 7), new Point(7, 9)};
        Console.WriteLine(logger.Log(pts));
        
        
        Triangle t = new Triangle(new Point(5, 7), new Point(11, 7), new Point(7, 9));
        Console.WriteLine(logger.Log(t));
        */
    }
    
}