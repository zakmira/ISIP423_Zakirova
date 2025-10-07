using System;
using System.Collections.Generic;
using System.Linq;

public abstract class Person
{
    public int Id { get; }
    public string Name { get; }
    public int Age { get; }
    public string ContactInfo { get; }

    protected Person(int id, string name, int age, string contactInfo)
    {
        Id = id;
        Name = name;
        Age = age;
        ContactInfo = contactInfo;
    }

    public abstract void DisplayInfo();
}

public class Student : Person
{
    private List<Course> _courses = new List<Course>();

    public Student(int id, string name, int age, string contactInfo)
        : base(id, name, age, contactInfo) { }

    public IReadOnlyList<Course> Courses => _courses.AsReadOnly();

    public void EnrollInCourse(Course course)
    {
        if (!_courses.Contains(course))
        {
            _courses.Add(course);
            course.AddStudent(this);
        }
    }

    public override void DisplayInfo()
    {
        Console.WriteLine($"Студент [ID:{Id}] {Name}, Возраст: {Age}, Контакты: {ContactInfo}");
        Console.WriteLine("Записан на курсы:");
        foreach (var course in _courses)
            Console.WriteLine($"  - {course.Name}");
    }
}

