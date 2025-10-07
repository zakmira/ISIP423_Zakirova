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

public class UniversityManager
{
    private List<Student> _students = new List<Student>();
    private List<Teacher> _teachers = new List<Teacher>();
    private List<Course> _courses = new List<Course>();
    private int _nextPersonId = 1;
    private int _nextCourseId = 1;

    // методы для работы со студентами
    public void AddStudent(string name, int age, string contactInfo)
    {
        var student = new Student(_nextPersonId++, name, age, contactInfo);
        _students.Add(student);
        Console.WriteLine($"Добавлен новый студент: {name}");
    }

    public void DisplayAllStudents()
    {
        Console.WriteLine("\nВсе студенты");
        foreach (var student in _students)
            student.DisplayInfo();
    }

    public void DisplayStudentCourses(int studentId)
    {
        var student = _students.FirstOrDefault(s => s.Id == studentId);
        if (student != null)
        {
            Console.WriteLine($"\nКурсы студента {student.Name}:");
            foreach (var course in student.Courses)
                Console.WriteLine($"  - {course.Name}");
        }
        else
            Console.WriteLine("Студент не найден!");
    }

    // методы для работы с преподавателями
    public void AddTeacher(string name, int age, string contactInfo)
    {
        var teacher = new Teacher(_nextPersonId++, name, age, contactInfo);
        _teachers.Add(teacher);
        Console.WriteLine($"Добавлен новый преподаватель: {name}");
    }

    public void DisplayAllTeachers()
    {
        Console.WriteLine("\nВсе преподаватели");
        foreach (var teacher in _teachers)
            teacher.DisplayInfo();
    }

    // методы для работы с курсами
    public void CreateCourse(string name, string description)
    {
        var course = new Course(_nextCourseId++, name, description);
        _courses.Add(course);
        Console.WriteLine($"Создан новый курс: {name}");
    }

    public void DisplayAllCourses()
    {
        Console.WriteLine("\nВсе курсы");
        foreach (var course in _courses)
            course.DisplayInfo();
    }

    public void DisplayCourseStudents(int courseId)
    {
        var course = _courses.FirstOrDefault(c => c.Id == courseId);
        if (course != null)
        {
            Console.WriteLine($"\nСтуденты курса {course.Name}:");
            foreach (var student in course.Students)
                Console.WriteLine($"  - {student.Name}");
        }
        else
            Console.WriteLine("Курс не найден!");
    }

    // методы связывания сущностей
    public void EnrollStudentInCourse(int studentId, int courseId)
    {
        var student = _students.FirstOrDefault(s => s.Id == studentId);
        var course = _courses.FirstOrDefault(c => c.Id == courseId);

        if (student == null || course == null)
        {
            Console.WriteLine("Ошибка: Студент или курс не найден!");
            return;
        }

        student.EnrollInCourse(course);
        Console.WriteLine($"Студент {student.Name} записан на курс {course.Name}");
    }

    public void AssignTeacherToCourse(int teacherId, int courseId)
    {
        var teacher = _teachers.FirstOrDefault(t => t.Id == teacherId);
        var course = _courses.FirstOrDefault(c => c.Id == courseId);

        if (teacher == null || course == null)
        {
            Console.WriteLine("Ошибка: Преподаватель или курс не найден!");
            return;
        }

        teacher.AssignToCourse(course);
        Console.WriteLine($"Преподаватель {teacher.Name} назначен на курс {course.Name}");
    }

    // методы для проверки существования ID
    public bool StudentExists(int id) => _students.Any(s => s.Id == id);
    public bool TeacherExists(int id) => _teachers.Any(t => t.Id == id);
    public bool CourseExists(int id) => _courses.Any(c => c.Id == id);
}