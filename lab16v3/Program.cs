
using System;
using System.Collections.Generic;
using System.Linq;

Subject math = new Subject(1, "Математика");
Subject programming = new Subject(2, "Програмування");
Subject english = new Subject(3, "Англійська мова");
Subject oop = new Subject(4, "ООП");

List<Student> students = new List<Student>
{
    new Student(1, "Іван Петренко", new List<Grade>
    {
        new Grade(math, 90),
        new Grade(programming, 95),
        new Grade(oop, 88)
    }),

    new Student(2, "Марія Коваленко", new List<Grade>
    {
        new Grade(math, 85),
        new Grade(english, 92),
        new Grade(oop, 90)
    }),

    new Student(3, "Олег Шевченко", new List<Grade>
    {
        new Grade(programming, 78),
        new Grade(math, 82),
        new Grade(oop, 85)
    }),

    new Student(4, "Анна Мельник", new List<Grade>
    {
        new Grade(math, 95),
        new Grade(programming, 98),
        new Grade(english, 90)
    }),

    new Student(5, "Дмитро Бондар", new List<Grade>
    {
        new Grade(oop, 88),
        new Grade(math, 75),
        new Grade(programming, 80)
    }),

    new Student(6, "Олександр Ткаченко", new List<Grade>
    {
        new Grade(math, 70),
        new Grade(english, 85),
        new Grade(oop, 78)
    }),

    new Student(7, "Софія Кравченко", new List<Grade>
    {
        new Grade(programming, 96),
        new Grade(math, 93),
        new Grade(oop, 97)
    }),

    new Student(8, "Андрій Козак", new List<Grade>
    {
        new Grade(math, 80),
        new Grade(english, 88),
        new Grade(programming, 84)
    }),

    new Student(9, "Ірина Савченко", new List<Grade>
    {
        new Grade(oop, 91),
        new Grade(math, 89),
        new Grade(english, 94)
    }),

    new Student(10, "Максим Поліщук", new List<Grade>
    {
        new Grade(programming, 76),
        new Grade(math, 83),
        new Grade(oop, 79)
    }),

    new Student(11, "Катерина Лисенко", new List<Grade>
    {
        new Grade(math, 99),
        new Grade(programming, 97),
        new Grade(english, 96)
    }),

    new Student(12, "Богдан Мороз", new List<Grade>
    {
        new Grade(oop, 82),
        new Grade(math, 86),
        new Grade(programming, 89)
    })
};

Console.WriteLine("========================================");
Console.WriteLine("ЗАВДАННЯ 1. SELECTMANY");
Console.WriteLine("Усі оцінки всіх студентів");
Console.WriteLine("========================================");

var allGrades = students.SelectMany(
    student => student.Grades,
    (student, grade) => new
    {
        StudentName = student.Name,
        SubjectName = grade.Subject.Name,
        grade.Score
    });

foreach (var grade in allGrades)
{
    Console.WriteLine(
        $"Студент: {grade.StudentName,-25} " +
        $"Предмет: {grade.SubjectName,-20} " +
        $"Оцінка: {grade.Score}");
}


Console.WriteLine("\n========================================");
Console.WriteLine("ЗАВДАННЯ 2. ПАГІНАЦІЯ");
Console.WriteLine("По 5 студентів на сторінку");
Console.WriteLine("========================================");

int pageSize = 5;

int totalPages = (int)Math.Ceiling(
    (double)students.Count / pageSize);

for (int page = 1; page <= totalPages; page++)
{
    Console.WriteLine($"\n--- СТОРІНКА {page} ---");

    var pageStudents = students
        .Skip((page - 1) * pageSize)
        .Take(pageSize);

    foreach (var student in pageStudents)
    {
        Console.WriteLine(
            $"ID: {student.Id} | Ім'я: {student.Name}");
    }
}

Console.WriteLine("\n========================================");
Console.WriteLine("ЗАВДАННЯ 3. AVERAGESCORE()");
Console.WriteLine("Середній бал кожного студента");
Console.WriteLine("========================================");

foreach (var student in students)
{
    double averageScore = student.Grades.AverageScore();

    Console.WriteLine(
        $"{student.Name,-25} Середній бал: {averageScore:F2}");
}

Console.WriteLine("\n========================================");
Console.WriteLine("ВИКОНАННЯ ПРОГРАМИ ЗАВЕРШЕНО!");
Console.WriteLine("========================================");

public class Subject
{
    public int Id { get; set; }
    public string Name { get; set; }

    public Subject(int id, string name)
    {
        Id = id;
        Name = name;
    }
}


public class Grade
{
    public Subject Subject { get; set; }
    public int Score { get; set; }

    public Grade(Subject subject, int score)
    {
        Subject = subject;
        Score = score;
    }
}

public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Grade> Grades { get; set; }

    public Student(int id, string name, List<Grade> grades)
    {
        Id = id;
        Name = name;
        Grades = grades;
    }
}


public static class GradeExtensions
{
    public static double AverageScore(this IEnumerable<Grade> grades)
    {
        if (!grades.Any())
        {
            return 0;
        }

        return grades.Average(grade => grade.Score);
    }
}
