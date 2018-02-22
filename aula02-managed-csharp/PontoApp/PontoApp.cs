using System;

class PontoApp {

    public static void Main(String[] args)
    {
        Ponto p = new Ponto(5, 7);
        p.print();
        System.Console.WriteLine(
            String.Format("p._x = {0}\n", p._x));
        System.Console.WriteLine(
            String.Format("p._y = {0}\n", p._y));
    }
}