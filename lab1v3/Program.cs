using System;

class Student
{
    private string name;
    private int id;

    public double AverageMark { get; set; }

    public Student(string name, int id, double averageMark)
    {
        this.name = name;
        this.id = id;
        AverageMark = averageMark;
    }

    public void PrintCard()
    {
        Console.WriteLine("----- Студентський квиток -----");
        Console.WriteLine($"Ім'я: {name}");
        Console.WriteLine($"ID: {id}");
        Console.WriteLine($"Середній бал: {AverageMark}");
        Console.WriteLine();
    }
}

class Program
{
    static void Main()
    {
        Student student1 = new Student("Іван Петренко", 101, 10.5);
        Student student2 = new Student("Олександр Коваль", 102, 9.2);
        Student student3 = new Student("Андрій Бондар", 103, 11.1);

        student1.PrintCard();
        student2.PrintCard();
        student3.PrintCard();
    }
}
