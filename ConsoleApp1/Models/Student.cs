namespace ConsoleApp1.Models;

public class Student
{
    public int Id  { get; set; }
    public string? Name { get; set; }
}

public static class Students
{
    public static List<Student> GetStudents() => new List<Student>
    {
        new Student { Id = 1, Name = "John Doe" },
        new Student { Id = 2, Name = "Jane" },
        new Student { Id = 3, Name = "Bob" }
    };
    
    public static Student Random() {
        var random = new Random();
        return GetStudents().Single(w => w.Id == random.Next(GetStudents().Count));
    }
}