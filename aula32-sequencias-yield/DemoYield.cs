using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

class App {
    
    static IEnumerable<string> Foo() {
        Console.WriteLine("Processing element 0");
        yield return "1";
        Console.WriteLine("Processing element 1");
        yield return "3";
        Console.WriteLine("Processing element 2");
        yield return "7";
    }

    static void Main()
    {
        IEnumerator<String> iter = Foo().GetEnumerator();
        while(iter.MoveNext())
        {
            Console.WriteLine(iter.Current);
            Console.ReadLine();
        }
    }
}