using System;

class A {} // tipo referencia
class I {} // tipo referencia

struct S {}  // tipo valor

struct Point{
    int x, y;
    A a;
    S s;
}

public class App {

    public static void Main(String [] args) {
        int n = 7; // n variável de tipo primtivo valor
        string str; // str variável de tipo primtivo referência
        System.String msg; // msg é de tipo referência NÃO primitivo.
        object o = ""; // o variável de tipo primitivo
        S s = new S(); // s é de tipo VALOR não primitivo.
        A a = new A();
        Point p = new Point();
        
        Console.WriteLine(o.GetType()); // System.String
        Console.WriteLine(s.GetType()); // S
        Console.WriteLine(a.GetType()); // A
        Console.WriteLine(n.GetType()); // System.Int32
        Console.WriteLine(o.GetType().BaseType);
        Console.WriteLine(s.GetType().BaseType);
        Console.WriteLine(a.GetType().BaseType);
        Console.WriteLine(n.GetType().BaseType);
    }
}