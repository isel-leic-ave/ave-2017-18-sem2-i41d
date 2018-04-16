using System;

public class Student
{
    public int Nr { get; set; }
    public string Name{ get; set; }
    public int Group{ get; set; }
    public string GithubId{ get; set; }
    public DateTime BirthDate {get; set; }

    public Student(int nr, String name, int group, string githubId, DateTime b)
    {
        this.Nr = nr;
        this.Name = name;
        this.Group = group;
        this.GithubId = githubId;
        this.BirthDate = b;
    }
}

interface DynamicLogger {
    string Log(object target);
}

class LoggerStudent : DynamicLogger{
    public string Log(object target) {
        Student st = (Student) target;
        String res = "";
        res += st.Nr;
        res += st.Name;
        res += st.Group;
        res += st.GithubId;
        res += st.BirthDate;
        return res;
    }
}