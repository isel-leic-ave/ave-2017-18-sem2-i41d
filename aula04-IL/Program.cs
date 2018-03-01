using System;

public class Program {
    public static bool Palind(String msg) {
        if(msg.Length == 1) return true;
        if(msg[0] != msg[msg.Length-1]) return false;
        if(msg.Length == 2) return true;
        return Palind(msg.Substring(1, msg.Length-2));
    }

    public static void Main(String [] args) {
        System.Console.WriteLine(App.Foo("ana"));
        System.Console.WriteLine(App.Foo("abel"));
        System.Console.WriteLine(App.Foo("x"));
        System.Console.WriteLine(App.Foo("xx"));
        
        System.Console.WriteLine("###############");
        
        System.Console.WriteLine(Palind("ana"));
        System.Console.WriteLine(Palind("abel"));
        System.Console.WriteLine(Palind("x"));
        System.Console.WriteLine(Palind("xx"));
    }
}
