class PontoApp {

    public static void main(String[] args)
    {
        Ponto p = new Ponto(5, 7);
        p.print();
        System.out.println(
            String.format("p._x = %d\n", p._x));
        System.out.println(
            String.format("p._z = %d\n", p._z));
    }
}