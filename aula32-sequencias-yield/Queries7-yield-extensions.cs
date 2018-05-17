using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

delegate bool Predicate<T>(T item);

delegate R Function<T,R>(T item);


static class App {
    static IEnumerable<string> Lines(string path)
    {
        using(StreamReader file = new StreamReader(path)) // <=> try-with resources do Java >= 7
        {
            string line;
            while ((line = file.ReadLine()) != null)
            {
                yield return line;
            }
        }
    }
     
    static IEnumerable<R> Convert<T,R>(this IEnumerable<T> src, Function<T,R> mapper) {
        foreach(T item in src) {
            yield return mapper(item);
        }
    }  
    static IEnumerable<T> Distinct<T>(this IEnumerable<T> src) {
        IList res = new ArrayList();
        foreach(T item in src) {
            if (!res.Contains(item)) { 
                res.Add(item);
                yield return item; 
            }
        }
    }
    static IEnumerable<T> Filter<T>(this IEnumerable<T> src, Predicate<T> pred) {
        foreach(T item in src) {
            if(pred(item))
                yield return item;
        }
    }
    
    static void Main()
    {
        IEnumerable names = Lines("i41d.txt")
            .Convert(Student.Parse)
            .Filter(item => item.name.StartsWith("R"))
            .Convert(item => item.name.Split(' ')[0])
            .Distinct();

        /* <=>
        names = Lines("i41d.txt")
            .Select(Student.Parse)
            .Where(item => item.name.StartsWith("R"))
            .Select(item => item.name.Split(' ')[0])
            .Distinct();
        */
        
        // Console.ReadLine();
        foreach(object l in names)
            Console.WriteLine(l);
            
    }
}

public class Student
{
    public readonly int nr;
    public readonly string name;
    public readonly int group;
    public readonly string email;
    public readonly string githubId;

    public Student(int nr, String name, int group, string email, string githubId)
    {
        this.nr = nr;
        this.name = name;
        this.group = group;
        this.email = email;
        this.githubId = githubId;
    }

    public override String ToString()
    {
        return String.Format("{0} {1} ({2}, {3})", nr, name, group, githubId);
    }
    public void Print()
    {
        Console.WriteLine(this.ToString());
    }
    
    public static Student Parse(object src){
        return Student.Parse(src.ToString());
    }
    
    public static Student Parse(string src){
        string [] words = src.Split('|');
        return new Student(
            int.Parse(words[0]),
            words[1],
            int.Parse(words[2]),
            words[3],
            words[4]);
    }
}
