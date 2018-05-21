using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

delegate bool Predicate<T>(T item);
delegate R Function<T,R>(T item);

class A {}
class B {}
class C {
    readonly string name;
    public C(string name) { this.name = name; }
    public bool Match(C other) { return name.Equals(other.name); }
}

class App {
    static B Foo(A arg) { return new B(); }
    static void Main()
    {
        Predicate<C> p4 = new C("game").Match; // ??? IL
        
        Function<A, B> f1 = arg => new B(); // Lambda => <Main>b__1_0'
        Function<A, B> f2 = App.Foo;        // Method Reference
        // <=> Function<A, B> f2 = new Function<A,B>(App.Foo);
        
        f1(new A()); // <=> f1.Invoke(new A());
        f2.Invoke(new A());
     
        // Predicate<C> p = C.Match; // !!!!! Erro de compilação
        C c = new C("ISEL"); // stloc2
        Predicate<C> p = c.Match; // ldloc2 + ldftn C::Match + newobj Predicate::ctor
        p(new C("super"));
        
        Predicate<String> p2 = s => false;
        // Predicate<String> p3 = s => new A(); // Erro de compilação
        
        
    }
}

