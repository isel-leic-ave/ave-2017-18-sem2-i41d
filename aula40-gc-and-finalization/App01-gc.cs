using System;

class App
{
    /**
     * !!!! compilar com /optimize+ <=> modo Release
     */
    static void Main()
    {
        object root = new object();
        Console.WriteLine(GC.GetGeneration(root));
        GC.Collect(0);
        Console.WriteLine(GC.GetGeneration(root));
        GC.Collect(0);
        Console.WriteLine(GC.GetGeneration(root));
        GC.Collect(); // Corre para todas as gerações <=> GC.Collect(2);
        Console.WriteLine(GC.GetGeneration(root));

        Console.WriteLine(GC.GetTotalMemory(false));
        object some = MakeSomeGarbage();
        Console.WriteLine(GC.GetTotalMemory(false)); // >>> memoria
        GC.Collect();
        Console.WriteLine(GC.GetTotalMemory(false)); // <<< memoria
        // some.GetHashCode();
    }
    
    private const long maxGarbage = 4096;        
    static object[] MakeSomeGarbage()
    {
        Console.WriteLine("..... Making garbage...");
        object[] vts = new object[maxGarbage];

        for(int i = 0; i < maxGarbage; i++)
        {
            vts[i] = new object();
        }
        return vts;
    }
}