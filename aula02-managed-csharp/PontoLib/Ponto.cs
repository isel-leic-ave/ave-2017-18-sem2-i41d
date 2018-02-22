using System;

public class Ponto {

	public int _x, _y;

	public Ponto(int x, int y) {
        this._x = x;
        this._y = y;
    }

	public void print() {
        System.Console.WriteLine(
            String.Format("Print V2 (x = {0}, y = {1})\n", _x, _y)
        );
    }
};