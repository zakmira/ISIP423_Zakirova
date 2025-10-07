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

public class Course
{
    public int Id { get; }
    public string Name { get; }
    public string Description { get; }
    private Teacher _teacher;
    private List<Student> _students = new List<Student>();

    public Course(int id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }

    public Teacher Teacher => _teacher;
    public IReadOnlyList<Student> Students => _students.AsReadOnly();

    public void AssignTeacher(Teacher teacher)
    {
        if (_teacher != teacher)
        {
            _teacher = teacher;
            if (!teacher.Courses.Contains(this))
                teacher.AssignToCourse(this);
        }
    }

    public void AddStudent(Student student)
    {
        if (!_students.Contains(student))
        {
            _students.Add(student);
            if (!student.Courses.Contains(this))
                student.EnrollInCourse(this);
        }
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"Курс [ID:{Id}] {Name}");
        Console.WriteLine($"Описание: {Description}");
        Console.WriteLine($"Преподаватель: {_teacher?.Name ?? "Не назначен"}");
        Console.WriteLine("Студенты:");
        foreach (var student in _students)
            Console.WriteLine($"  - {student.Name}");
    }
}

