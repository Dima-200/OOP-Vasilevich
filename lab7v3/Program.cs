using System;

class Employee
{
    public string Name { get; set; }

    public Employee(string name)
    {
        Name = name;
    }

    public virtual void Work()
    {
        Console.WriteLine($"{Name}: Employee is working.");
    }
}

class Manager : Employee
{
    public Manager(string name) : base(name)
    {
    }

    public override void Work()
    {
        Console.WriteLine($"{Name}: Manager is managing the team.");
    }

    public void HoldMeeting()
    {
        Console.WriteLine($"{Name}: Manager is holding a meeting.");
    }
}

class Intern : Employee
{
    public Intern(string name) : base(name)
    {
    }

    public new void Work()
    {
        Console.WriteLine($"{Name}: Intern is doing practical tasks.");
    }

    public void Learn()
    {
        Console.WriteLine($"{Name}: Intern is learning.");
    }
}

class Program
{
    static void Main()
    {
        Employee employee = new Employee("Олександр");
        Employee manager = new Manager("Дмитро");
        Employee intern = new Intern("Іван");

        Console.WriteLine("=== Виклик через посилання базового класу ===");

        employee.Work();
        manager.Work();
        intern.Work();

        Console.WriteLine();
        Console.WriteLine("=== Виклик через посилання похідного класу ===");

        ((Manager)manager).Work();
        ((Intern)intern).Work();

        Console.WriteLine();
        Console.WriteLine("=== Додаткові методи похідних класів ===");

        ((Manager)manager).HoldMeeting();
        ((Intern)intern).Learn();

        Console.WriteLine();
        Console.WriteLine("=== Демонстрація різниці override та new ===");

        Console.WriteLine("Manager використовує override:");
        manager.Work();
        ((Manager)manager).Work();

        Console.WriteLine();

        Console.WriteLine("Intern використовує new:");
        intern.Work();
        ((Intern)intern).Work();
    }
}