using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab16v3;

public sealed class Student
{
    public int Id { get; }
    public string Name { get; }
    public List<Grade> Grades { get; }

    public Student(int id, string name, IEnumerable<Grade> grades)
    {
        Id = id;
        Name = name;
        Grades = grades.ToList();
    }

    public override string ToString() => $"#{Id}: {Name}";
}

public sealed class Grade
{
    public Subject Subject { get; }
    public double Score { get; }

    public Grade(Subject subject, double score)
    {
        Subject = subject;
        Score = score;
    }

    public override string ToString() => $"{Subject.Name}: {Score:F1}";
}

public sealed class Subject
{
    public int Id { get; }
    public string Name { get; }

    public Subject(int id, string name)
    {
        Id = id;
        Name = name;
    }
}

public static class GradeExtensions
{
    public static double AverageScore(this IEnumerable<Grade> grades)
    {
        ArgumentNullException.ThrowIfNull(grades);
        var scores = grades.Select(grade => grade.Score).ToList();
        return scores.Count == 0 ? 0 : scores.Average();
    }
}

internal static class Program
{
    private const int PageSize = 5;

    private static readonly List<Subject> Subjects = new()
    {
        new(1, "Об'єктно-орієнтоване програмування"),
        new(2, "Алгоритми та структури даних"),
        new(3, "Бази даних"),
        new(4, "Комп'ютерні мережі")
    };

    private static readonly List<Student> Students = new()
    {
        new(1, "Андрій Бондар", new[] { new Grade(Subjects[0], 91), new Grade(Subjects[1], 84), new Grade(Subjects[2], 88) }),
        new(2, "Марія Василенко", new[] { new Grade(Subjects[0], 95), new Grade(Subjects[1], 92), new Grade(Subjects[3], 90) }),
        new(3, "Олег Гриценко", new[] { new Grade(Subjects[0], 78), new Grade(Subjects[2], 81), new Grade(Subjects[3], 76) }),
        new(4, "Ірина Данилюк", new[] { new Grade(Subjects[1], 87), new Grade(Subjects[2], 93), new Grade(Subjects[3], 89) }),
        new(5, "Дмитро Жук", new[] { new Grade(Subjects[0], 69), new Grade(Subjects[1], 74), new Grade(Subjects[2], 72) }),
        new(6, "Софія Коваль", new[] { new Grade(Subjects[0], 98), new Grade(Subjects[1], 96), new Grade(Subjects[3], 94) }),
        new(7, "Максим Левченко", new[] { new Grade(Subjects[0], 82), new Grade(Subjects[2], 85), new Grade(Subjects[3], 80) }),
        new(8, "Олена Мельник", new[] { new Grade(Subjects[1], 90), new Grade(Subjects[2], 88), new Grade(Subjects[3], 92) }),
        new(9, "Тарас Петренко", new[] { new Grade(Subjects[0], 73), new Grade(Subjects[1], 79), new Grade(Subjects[2], 77) }),
        new(10, "Наталія Савчук", new[] { new Grade(Subjects[0], 86), new Grade(Subjects[2], 91), new Grade(Subjects[3], 87) }),
        new(11, "Віктор Ткачук", new[] { new Grade(Subjects[1], 68), new Grade(Subjects[2], 75), new Grade(Subjects[3], 71) }),
        new(12, "Юлія Шевченко", new[] { new Grade(Subjects[0], 93), new Grade(Subjects[1], 89), new Grade(Subjects[3], 97) })
    };

    private static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        Header("Лабораторна робота №16 — LINQ: SelectMany, пагінація та extension methods");
        Console.WriteLine("Варіант 3: студенти та їхні оцінки з предметів.");
        Console.WriteLine($"Тестові дані: {Students.Count} студентів, {Subjects.Count} предмети, " +
                          $"{Students.Sum(student => student.Grades.Count)} оцінок.\n");

        Header("1. SelectMany — оцінки всіх студентів в одному списку");
        var allGrades = Students
            .SelectMany(student => student.Grades,
                (student, grade) => new { Student = student, Grade = grade })
            .ToList();

        foreach (var item in allGrades)
            Console.WriteLine($"  {item.Student.Name,-20} | {item.Grade}");
        Console.WriteLine($"Разом оцінок: {allGrades.Count}");

        Header("2. Пагінація студентів — по 5 записів на сторінку");
        int pageCount = (int)Math.Ceiling(Students.Count / (double)PageSize);
        for (int page = 1; page <= pageCount; page++)
        {
            var pageStudents = Students
                .Skip((page - 1) * PageSize)
                .Take(PageSize);

            Console.WriteLine($"\nСторінка {page} з {pageCount}:");
            foreach (var student in pageStudents)
                Console.WriteLine($"  {student}");
        }

        Header("3. Власний метод розширення AverageScore()");
        foreach (var student in Students)
            Console.WriteLine($"  {student.Name,-20} середній бал: {student.Grades.AverageScore():F2}");

        var selectedStudent = Students[0];
        Console.WriteLine($"\nПриклад: середній бал {selectedStudent.Name} = " +
                          $"{selectedStudent.Grades.AverageScore():F2}");

        Header("Висновок");
        Console.WriteLine("SelectMany об'єднав вкладені списки оцінок студентів у плоску послідовність.");
        Console.WriteLine("Skip і Take сформували сторінки студентів, а AverageScore обчислив середній бал.");
    }

    private static void Header(string title)
    {
        Console.WriteLine();
        Console.WriteLine($"=== {title} ===");
    }
}
