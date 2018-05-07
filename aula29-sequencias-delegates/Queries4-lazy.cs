using System;
using System.Collections;
using System.Text;
using System.IO;

delegate bool Predicate(object item);

delegate object Function(object item);

class ConvertEnumerable : IEnumerable {
    IEnumerable src;
    Function mapper;
    public ConvertEnumerable(IEnumerable src, Function mapper) {
        this.src = src;
        this.mapper = mapper;
    }
    public IEnumerator GetEnumerator() {
        return new ConvertEnumerator(src, mapper);
    }
}
class ConvertEnumerator : IEnumerator {
    IEnumerator srcIter;
    Function mapper;
    public ConvertEnumerator(IEnumerable src, Function mapper) {
        this.srcIter = src.GetEnumerator();
        this.mapper = mapper;
    }
    public bool MoveNext() {
        return srcIter.MoveNext();
    }
    public object Current {
        get { return mapper(srcIter.Current); }
    }
    public void Reset() {
        srcIter.Reset();
    }
}

class FilterEnumerable : IEnumerable {
    IEnumerable src;
    Predicate p;
    public FilterEnumerable(IEnumerable src, Predicate p) {
        this.src = src;
        this.p = p;
    }
    public IEnumerator GetEnumerator() {
        return new FilterEnumerator(src, p);
    }
}
class FilterEnumerator : IEnumerator {
    IEnumerator srcIter;
    Predicate p;
    public FilterEnumerator(IEnumerable src, Predicate p) {
        this.srcIter = src.GetEnumerator();
        this.p = p;
    }
    public bool MoveNext() {
        while(srcIter.MoveNext()) {
            if(p(srcIter.Current))
                return true;
        }
        return false;
    }
    public object Current {
        get { return srcIter.Current; }
    }
    public void Reset() {
        srcIter.Reset();
    }
}

class App {
    static IEnumerable Lines(string path)
    {
        string line;
        IList res = new ArrayList();
        
        using(StreamReader file = new StreamReader(path)) // <=> try-with resources do Java >= 7
        {
            while ((line = file.ReadLine()) != null)
            {
                res.Add(line);
            }
        }
        return res;
    }
     
    static IEnumerable Convert(IEnumerable src, Function mapper) {
        return new ConvertEnumerable(src, mapper);
    }
    
    static IEnumerable Distinct(IEnumerable src) {
        return src;
    }
    
    static IEnumerable Filter(IEnumerable src, Predicate pred) {
        return new FilterEnumerable(src, pred);
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
                            return ((Student) item).name.StartsWith("R");
                        }),
                    item => { 
                        // Console.WriteLine("Convert..." + item); 
                        return ((Student) item).name.Split(' ')[0];
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
