using System;


public struct Point{
    int x, y;
    public Point(int x, int y) {
        this.x = x;
        this.y = y;
    }
    
    public int X {
        get{return x;}
        set{ x = value; }
    }
    public int Y { // RW
        get{return y;}
        set{ y = value; }
    }  
    public double Module { // READ ONLY
        get {
            return Math.Sqrt(x*x + y*y);
        }
    }
}

interface DynamicLogger {
    string Log(object target);
}

class LoggerPoint : DynamicLogger{
    public string Log(object target) {
        Point res = (Point) target;
        String str = "";
        str += res.X + ",";
        str += res.Y + ",";
        str += res.Module + ",";
        return str;
    }
}