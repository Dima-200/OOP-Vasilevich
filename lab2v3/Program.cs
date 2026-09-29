using System;

class Student
{
private string _name;
private string _studentId;
private double _averageMark;


public string Name
{
    get { return _name; }
    set { _name = value; }
}

public string StudentId
{
    get { return _studentId; }
    set { _studentId = value; }
}

public double AverageMark
{
    get { return _averageMark; }
    set
    {
        if (value < 0 || value > 100)
        {
            throw new ArgumentException("Середній бал має бути від 0 до 100.");
        }

        _averageMark = value;
    }
}

// Конструктор за замовчуванням
public Student() : this("New Student", "N/A", 0.0)
{
}

// Параметризований конструктор
public Student(string name, string studentId, double averageMark)
{
    _name = name;
    _studentId = studentId;
    AverageMark = averageMark;

    Console.WriteLine($"Створено студента: {Name}");
}

// Метод для отримання інформації про студента
public string GetStudentCard()
{
    return $"Ім'я: {Name}\nID: {StudentId}\nСередній бал: {AverageMark:F1}";
}

// Деструктор
~Student()
{
    Console.WriteLine($"Деструктор: об'єкт студента {Name} знищено.");
}


}

class Program
{
static void Main()
{
Console.OutputEncoding = System.Text.Encoding.UTF8;


    Console.WriteLine("===== Лабораторна робота №2 =====");
    Console.WriteLine("===== Варіант 3 — Student =====");
    Console.WriteLine();

    Console.WriteLine("Створення об'єктів:");
    Console.WriteLine();

    Student student1 = new Student();

    Student student2 = new Student(
        "Іван Петренко",
        "101",
        89.5
    );

    Student student3 = new Student(
        "Олександр Коваль",
        "102",
        95.2
    );

    Console.WriteLine();
    Console.WriteLine("===== Дані студентів =====");
    Console.WriteLine();

    Console.WriteLine("Студент 1:");
    Console.WriteLine(student1.GetStudentCard());
    Console.WriteLine();

    Console.WriteLine("Студент 2:");
    Console.WriteLine(student2.GetStudentCard());
    Console.WriteLine();

    Console.WriteLine("Студент 3:");
    Console.WriteLine(student3.GetStudentCard());
    Console.WriteLine();

    Console.WriteLine("===== Перевірка валідації =====");

    try
    {
        student1.AverageMark = 120;
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"Помилка: {ex.Message}");
    }

    Console.WriteLine();
    Console.WriteLine("===== Демонстрація роботи Garbage Collector =====");

    CreateTemporaryStudents();

    GC.Collect();
    GC.WaitForPendingFinalizers();

    Console.WriteLine();
    Console.WriteLine("===== Кінець програми =====");
}

static void CreateTemporaryStudents()
{
    Student temporaryStudent1 = new Student(
        "Тимчасовий студент 1",
        "TMP01",
        75.5
    );

    Student temporaryStudent2 = new Student(
        "Тимчасовий студент 2",
        "TMP02",
        82.0
    );
}


}

