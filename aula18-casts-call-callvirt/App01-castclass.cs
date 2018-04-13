using System;
using System.Collections.Generic;
using System.Reflection;

public class Person {}

public class Student : Person
{
    public int Nr { get; set; }
    public int Group{ get; set; }
    public string Name{ get; set; }
    public string GithubId{ get; set; }
    public DateTime BirthDate {get; set; }

    public Student() {
    }
    public Student(int nr, String name, int group, string githubId, DateTime b)
    {
        this.Nr = nr;
        this.Name = name;
        this.Group = group;
        this.GithubId = githubId;
        this.BirthDate = b;
    }
}


struct Point{
    int x, y;
    
    // public Point() {} // NãO é possível porque não existe forma de chamar este construtor.
    public Point(int x, int y) {
        this.x = x;
        this.y = y;
    }  
    public int X {
        get{return x;}
        set{ x = value; }
    }
    public int Y { 
        get{return y;}
        set{ y = value; }
    }
    public double Module { // READ ONLY
        get {
            return Math.Sqrt(x*x + y*y);
        }
    }
}

struct S {}
class C {}

public class App {
    public static void Main(String [] args) {
        Student s = new Student(); // newobj Student::ctor() + stloc.0
        Person p = s; // Cast Implicito =>  ldloc.0 + stloc.1
        
        Student s2 = (Student) p; // Cast Explicito =>  ldloc.1 + castclass Student + stloc.2
        Person p2 = new Person();
        s2 = p2 as Student; // ldloc.3 + insinst Student + stloc.2
        string msg = s2 == null? "null": s2.ToString();
        Console.WriteLine(msg); // > null
        
        s2 = (Student) p2; // InvalidCastException => InvalidCastException !!!!!
        
    }
    
    static void testBoxUnbox() {
        S s = new S(); // => inicializado => IL initobj => Iniciar espaço a Zeros 
        C c = new C(); // => instanciado  => IL newobj => malloc() + call .ctor
        Point p1 = new Point();    // => initobj
        Point p2 = new Point(6,7); // => ___ + ldc.i4 6 +  ldc.i4 7  + call Point::.ctor
        // Student std = new Student();
        
        Object o = p1;        // box
        Point p3 = (Point) o; // unbox
        
        p1.GetType(); // => box
    }
}