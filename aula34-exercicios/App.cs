using System;
using System.Reflection;
using System.Collections.Generic;

class App {
    static void Main()
    {
        Student s = new Student(20, "Anacleto", 11);
        Validator<Student> validator = new Validator<Student>() 
                            .AddValidation("Age", new Above18()) 
                            .AddValidation("Name", new NotNull()); 
        validator.Validate(s); 
        // validator.Validate(new Student(16, "Ze MAnel", 11)); 
        validator.Validate(new Student(20, null, 11)); 
        
    }
}

class Above18 :IValidation{
    public bool Validate(object o ){
        return ((int) o) > 18;
    }
}
class NotNull :IValidation{
    public bool Validate(object o ){
        return o != null;
    }
}

interface IValidation { bool Validate(object obj); } 

class ValidationException : Exception {
    public ValidationException(string desc) : base(desc) {}
}

class Validator<T> {
    // Desconta cerca de 20%
    // MELHOR: usar Dictionary<PropertyInfo, IValidation>
    //
    Dictionary<string, IValidation> validations = new Dictionary<string, IValidation>();
    
    public Validator<T> AddValidation(string prop, IValidation val) {
        validations.Add(prop, val);
        return this;
    }
    
    public void Validate(T target) {
        foreach(KeyValuePair<string,IValidation> pair in validations) {
            PropertyInfo prop = typeof(T).GetProperty(pair.Key); // !!!! OVERHEAD
            object val = prop.GetValue(target);
            if(pair.Value.Validate(val) == false)
                throw new ValidationException(
                    String.Format(
                        "Validation {0} failed for property {1} for {2}",
                        pair.Value.GetType().Name,
                        pair.Key,
                        target.ToString()));
        }
    }
}

public class Student
{
    public int Age {get; set;}
    public string Name {get; set;}
    public int Group {get; set;}

    public Student(int age, String name, int group)
    {
        this.Age = age;
        this.Name = name;
        this.Group = group;
    }

    public override String ToString()
    {
        return String.Format("{0} {1} ({2})", Age, Name, Group);
    }
    public void Print()
    {
        Console.WriteLine(this.ToString());
    }
}
