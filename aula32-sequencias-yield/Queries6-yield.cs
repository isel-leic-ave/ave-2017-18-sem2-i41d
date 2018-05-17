using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

delegate bool Predicate<T>(T item);

delegate R Function<T,R>(T item);


class App {
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
     
    static IEnumerable<R> Convert<T,R>(IEnumerable<T> src, Function<T,R> mapper) {
        foreach(T item in src) {
            yield return mapper(item);
        }
    }  
    static IEnumerable<T> Distinct<T>(IEnumerable<T> src) {
        IList res = new ArrayList();
        foreach(T item in src) {
            if (!res.Contains(item)) { 
                res.Add(item);
                yield return item; 
            }
        }
    }
    static IEnumerable<T> Filter<T>(IEnumerable<T> src, Predicate<T> pred) {
        foreach(T item in src) {
            if(pred(item))
                yield return item;
        }
    }
    
    static void Main()
    {
        IEnumerable names = 
            Distinct(
                Convert(
                    Filter(
                        Convert(
                            Lines("i41d.txt"),
                            Student.Parse),
                        item => { 
                            // Console.WriteLine("Filter2..." + item); 
                            return item.name.StartsWith("R");
                        }),
                    item => { 
                        // Console.WriteLine("Convert..." + item); 
                        return item.name.Split(' ')[0];
                    })
            );
                        
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
