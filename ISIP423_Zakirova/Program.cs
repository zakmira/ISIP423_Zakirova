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

public class Teacher : Person
{
    private List<Course> _courses = new List<Course>();

    public Teacher(int id, string name, int age, string contactInfo)
        : base(id, name, age, contactInfo) { }

    public IReadOnlyList<Course> Courses => _courses.AsReadOnly();

    public void AssignToCourse(Course course)
    {
        if (!_courses.Contains(course))
        {
            _courses.Add(course);
            course.AssignTeacher(this);
        }
    }

    public override void DisplayInfo()
    {
        Console.WriteLine($"Преподаватель [ID:{Id}] {Name}, Возраст: {Age}, Контакты: {ContactInfo}");
        Console.WriteLine("Ведет курсы:");
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

class Program
{
    static void Main(string[] args)
    {
        var manager = new UniversityManager();

        while (true)
        {
            Console.WriteLine("\n=== Система управления университетом ===");
            Console.WriteLine("1. Добавить студента");
            Console.WriteLine("2. Показать всех студентов");
            Console.WriteLine("3. Показать курсы студента");
            Console.WriteLine("4. Добавить преподавателя");
            Console.WriteLine("5. Показать всех преподавателей");
            Console.WriteLine("6. Создать курс");
            Console.WriteLine("7. Показать все курсы");
            Console.WriteLine("8. Показать студентов курса");
            Console.WriteLine("9. Записать студента на курс");
            Console.WriteLine("10. Назначить преподавателя на курс");
            Console.WriteLine("0. Выход");

            Console.Write("Выберите действие: ");
            var choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        AddStudent(manager);
                        break;
                    case "2":
                        manager.DisplayAllStudents();
                        break;
                    case "3":
                        DisplayStudentCourses(manager);
                        break;
                    case "4":
                        AddTeacher(manager);
                        break;
                    case "5":
                        manager.DisplayAllTeachers();
                        break;
                    case "6":
                        CreateCourse(manager);
                        break;
                    case "7":
                        manager.DisplayAllCourses();
                        break;
                    case "8":
                        DisplayCourseStudents(manager);
                        break;
                    case "9":
                        EnrollStudent(manager);
                        break;
                    case "10":
                        AssignTeacher(manager);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор!");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }

    static void AddStudent(UniversityManager manager)
    {
        Console.Write("Введите имя студента: ");
        var name = Console.ReadLine();
        Console.Write("Введите возраст: ");
        var age = int.Parse(Console.ReadLine());
        Console.Write("Введите контактную информацию: ");
        var contact = Console.ReadLine();

        manager.AddStudent(name, age, contact);
    }

    static void AddTeacher(UniversityManager manager)
    {
        Console.Write("Введите имя преподавателя: ");
        var name = Console.ReadLine();
        Console.Write("Введите возраст: ");
        var age = int.Parse(Console.ReadLine());
        Console.Write("Введите контактную информацию: ");
        var contact = Console.ReadLine();

        manager.AddTeacher(name, age, contact);
    }

    static void CreateCourse(UniversityManager manager)
    {
        Console.Write("Введите название курса: ");
        var name = Console.ReadLine();
        Console.Write("Введите описание курса: ");
        var description = Console.ReadLine();

        manager.CreateCourse(name, description);
    }

    static void DisplayStudentCourses(UniversityManager manager)
    {
        Console.Write("Введите ID студента: ");
        var id = int.Parse(Console.ReadLine());

        if (manager.StudentExists(id))
            manager.DisplayStudentCourses(id);
        else
            Console.WriteLine("Студент с таким ID не найден!");
    }

    static void DisplayCourseStudents(UniversityManager manager)
    {
        Console.Write("Введите ID курса: ");
        var id = int.Parse(Console.ReadLine());

        if (manager.CourseExists(id))
            manager.DisplayCourseStudents(id);
        else
            Console.WriteLine("Курс с таким ID не найден!");
    }

    static void EnrollStudent(UniversityManager manager)
    {
        Console.Write("Введите ID студента: ");
        var studentId = int.Parse(Console.ReadLine());
        Console.Write("Введите ID курса: ");
        var courseId = int.Parse(Console.ReadLine());

        manager.EnrollStudentInCourse(studentId, courseId);
    }

    static void AssignTeacher(UniversityManager manager)
    {
        Console.Write("Введите ID преподавателя: ");
        var teacherId = int.Parse(Console.ReadLine());
        Console.Write("Введите ID курса: ");
        var courseId = int.Parse(Console.ReadLine());

        manager.AssignTeacherToCourse(teacherId, courseId);
    }
}