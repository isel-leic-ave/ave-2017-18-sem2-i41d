class A<T>{}

class App{
    static void foo() {
        A<String> a1 = new A<>();
        A a2 = a1;
    }
}