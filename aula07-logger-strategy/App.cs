using System;
using System.Collections.Generic;
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
    public double Module {
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