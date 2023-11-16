using MyORM;

namespace TestMyORM;

public class Program
{
    static void Main()
    {
        var db = new MyDataContext("Server=(localdb)\\MSSQLLocalDB;Database=MyORMExample");
        var stud = new Student("Lisa");
        // db.Add<Student>(stud);
        var mystud = db.SelectById<Student>(3);
        Console.WriteLine(mystud.Name);
        // db.Update(stud);
        // db.Delete<Student>(1);
    }
}

public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }

    public Student(string name)
    {
        Name = name;
    }
    
    public Student(){ }
}