using System;

class A : I1, I2 {} // tipo referencia
class B : A, I3, I4 {} // tipo referencia
class C : B {}

interface I {}
struct S {}  // tipo valor

interface I1 {}
interface I2 {}
interface I3 {}
interface I4 {}

struct Point{
    int x, y;
    A a;
    S s;
}

public class App {

    static bool isInstanceOf(object target, Type t) {
        return target.GetType().Equals(t);
    }
    static bool isCompatible(object target, Type t) {
        Type klass = target.GetType();
        foreach(Type i in klass.GetInterfaces()) {
            if(isCompatible(i, t)) return true;
        }
        return isCompatible(klass, t);
    }
    static bool isCompatible(Type klass, Type t) {
        Console.WriteLine(klass);
        // if(klass.Equals(t)) return true;
        // if(klass == t) return true; // <=>
        if(Object.ReferenceEquals(klass, t)) return true;
        Type super = klass.BaseType;
        return super == null? false : isCompatible(super, t);
    }

    public static void Main(String [] args) {
        // testIsCompatible();
        printTypeHierarchyTraversal();
    }
    
    static void printTypeHierarchyTraversal(){
        Console.WriteLine(isCompatible(new C(), typeof(String))); // false
    }
    
    static void testIsCompatible(){
        Console.WriteLine("######### Test isCompatible #############");
        object o = "";
        I1 i = new A();
        Console.WriteLine(isCompatible(o, typeof(String))); // true
        Console.WriteLine(isCompatible(77, typeof(System.ValueType))); // true
        Console.WriteLine(isCompatible(o, typeof(object))); // true
        Console.WriteLine(isCompatible(new Point(), typeof(object))); // true
        Console.WriteLine(isCompatible(new Point(), typeof(System.ValueType))); // true
        Console.WriteLine(isCompatible(new Point(), typeof(string))); // false
        Console.WriteLine(isCompatible(i, typeof(A))); // true
        Console.WriteLine(isCompatible(i, typeof(I1))); // true
    }
    
    static void testInstanceOf(){
        object o = "";
        Console.WriteLine("######### Test isInstanceOf #############");
        Console.WriteLine(isInstanceOf(o, typeof(String))); // true
        // NÃO Fazer:
        Console.WriteLine(isInstanceOf(o, "ola".GetType())); // true !!!!! NÃO Fazer !!!!!
        
        Console.WriteLine(isInstanceOf(o, typeof(int)));    // false
        Console.WriteLine(isInstanceOf(new Point(), typeof(Point))); // true
        Console.WriteLine(isInstanceOf(new Point(), typeof(object))); // false

    }
}