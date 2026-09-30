using System;

public class Book
{
    private string _title;
    private string _author;
    private int _pages;

    public string Title
    {
        get { return _title; }
        set { _title = value; }
    }

    public string Author
    {
        get { return _author; }
        set { _author = value; }
    }

    public int Pages
    {
        get { return _pages; }
    }

    public Book(string title, string author, int pages)
    {
        _title = title;
        _author = author;
        _pages = pages;
    }

    public bool IsLongBook()
    {
        return _pages >= 300;
    }

    public void PrintInfo()
    {
        Console.WriteLine($"Книга: {_title}");
        Console.WriteLine($"Автор: {_author}");
        Console.WriteLine($"Кількість сторінок: {_pages}");
        Console.WriteLine($"Об'ємна книга: {(IsLongBook() ? "так" : "ні")}");
    }
}

public class BankAccount
{
    private string _owner;
    private decimal _balance;

    public string Owner
    {
        get { return _owner; }
        set { _owner = value; }
    }

    public decimal Balance
    {
        get { return _balance; }
    }

    public BankAccount(string owner, decimal balance)
    {
        _owner = owner;
        _balance = balance;
    }

    public void Deposit(decimal amount)
    {
        if (amount > 0)
        {
            _balance += amount;
        }
    }

    public void PrintInfo()
    {
        Console.WriteLine($"Власник рахунку: {_owner}");
        Console.WriteLine($"Баланс: {_balance:F2} грн");
    }
}

public class Car
{
    private string _brand;
    private double _fuel;
    private double _fuelConsumption;

    public string Brand
    {
        get { return _brand; }
        set { _brand = value; }
    }

    public double Fuel
    {
        get { return _fuel; }
        set { _fuel = value; }
    }

    public Car(string brand, double fuel, double fuelConsumption)
    {
        _brand = brand;
        _fuel = fuel;
        _fuelConsumption = fuelConsumption;
    }

    public double CalculateDistance()
    {
        return (_fuel / _fuelConsumption) * 100;
    }

    public void PrintInfo()
    {
        Console.WriteLine($"Автомобіль: {_brand}");
        Console.WriteLine($"Пального: {_fuel:F1} л");
        Console.WriteLine($"Витрата: {_fuelConsumption:F1} л/100 км");
        Console.WriteLine($"Можлива відстань: {CalculateDistance():F1} км");
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("===== Самостійна робота №1 =====");
        Console.WriteLine("Тема: Базовий синтаксис C# і оголошення класів");
        Console.WriteLine();

        Book book = new Book("Кобзар", "Тарас Шевченко", 350);

        BankAccount account = new BankAccount("Іван Петренко", 5000);

        Car car = new Car("Toyota", 40, 8);

        Console.WriteLine("----- Клас Book -----");
        book.PrintInfo();
        Console.WriteLine();

        Console.WriteLine("----- Клас BankAccount -----");
        account.PrintInfo();
        account.Deposit(1500);
        Console.WriteLine("Після поповнення рахунку:");
        account.PrintInfo();
        Console.WriteLine();

        Console.WriteLine("----- Клас Car -----");
        car.PrintInfo();
        Console.WriteLine();

        Console.WriteLine("===== Роботу завершено =====");
    }
}