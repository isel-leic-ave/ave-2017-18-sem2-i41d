using System;

class A{}
class A<T> : A{}

class App{
    static void Main() {
        A<String> a1 = new A<String>();
        A a2 = a1;
    }
}