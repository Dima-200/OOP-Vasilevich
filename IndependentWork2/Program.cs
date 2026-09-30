using System;

public class Product
{
    private int _id;
    private string _name;
    private decimal _price;
    private string _category;
    private int _stockCount;

    public int Id
    {
        get { return _id; }
    }

    public string Name
    {
        get { return _name; }
    }

    public decimal Price
    {
        get { return _price; }
    }

    public string Category
    {
        get { return _category; }
    }

    public int StockCount
    {
        get { return _stockCount; }
    }

    public Product(
        int id,
        string name,
        decimal price,
        string category,
        int stockCount)
    {
        _id = id;
        _name = name;
        _price = price;
        _category = category;
        _stockCount = stockCount;
    }

    public Product(int id, string name, decimal price)
        : this(id, name, price, "Uncategorized", 0)
    {
    }

    public Product(Product other)
        : this(
            other.Id,
            other.Name,
            other.Price,
            other.Category,
            other.StockCount)
    {
    }

    public override string ToString()
    {
        return $"ID: {Id}, Name: {Name}, Price: {Price:N2} ₴, " +
               $"Category: {Category}, Stock: {StockCount}";
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("===== Самостійна робота №2 =====");
        Console.WriteLine("Тема: Перевантаження конструкторів");
        Console.WriteLine();

        Product product1 = new Product(
            101,
            "Laptop",
            35000m,
            "Electronics",
            15);

        Product product2 = new Product(
            102,
            "Mouse",
            800m);

        Product product3 = new Product(product1);

        Console.WriteLine("Створення товарів");
        Console.WriteLine();

        Console.WriteLine("Товар 1 (основний конструктор):");
        Console.WriteLine(product1);
        Console.WriteLine();

        Console.WriteLine("Товар 2 (скорочений конструктор):");
        Console.WriteLine(product2);
        Console.WriteLine();

        Console.WriteLine("Товар 3 (конструктор копіювання):");
        Console.WriteLine(product3);
        Console.WriteLine();

        Console.WriteLine("===== Роботу завершено =====");
    }
}