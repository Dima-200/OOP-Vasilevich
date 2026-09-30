using System;

class Money
{
    private decimal _amount;
    private string _currency;

    public decimal Amount
    {
        get { return _amount; }
        set
        {
            if (value < 0)
                throw new ArgumentException("Сума не може бути від'ємною.");

            _amount = value;
        }
    }

    public string Currency
    {
        get { return _currency; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Валюта не може бути порожньою.");

            _currency = value.ToUpper();
        }
    }

    public static string DefaultCurrency { get; } = "UAH";

    public Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public static Money operator +(Money a, Money b)
    {
        if (a.Currency != b.Currency)
            throw new InvalidOperationException(
                "Не можна додавати суми в різних валютах.");

        return new Money(a.Amount + b.Amount, a.Currency);
    }

    public static Money operator *(Money money, decimal multiplier)
    {
        if (multiplier < 0)
            throw new ArgumentException(
                "Множник не може бути від'ємним.");

        return new Money(money.Amount * multiplier, money.Currency);
    }

    public static bool operator ==(Money? a, Money? b)
    {
        if (ReferenceEquals(a, b))
            return true;

        if (a is null || b is null)
            return false;

        return a.Amount == b.Amount &&
               a.Currency == b.Currency;
    }

    public static bool operator !=(Money? a, Money? b)
    {
        return !(a == b);
    }

    public override string ToString()
    {
        return $"{Amount:0.00} {Currency}";
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Money other)
            return false;

        return this == other;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Amount, Currency);
    }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("===== Лабораторна робота №4 =====");
        Console.WriteLine("Варіант №3 — клас Money");
        Console.WriteLine();

        Console.WriteLine($"Валюта за замовчуванням: {Money.DefaultCurrency}");
        Console.WriteLine();

        Money money1 = new Money(1500m, "UAH");
        Money money2 = new Money(750m, "UAH");
        Money money3 = new Money(100m, "USD");

        Console.WriteLine("Створені об'єкти:");
        Console.WriteLine($"money1 = {money1}");
        Console.WriteLine($"money2 = {money2}");
        Console.WriteLine($"money3 = {money3}");
        Console.WriteLine();

        money1.Amount = 2000m;
        money1.Currency = "uah";

        Console.WriteLine("Після зміни властивостей money1:");
        Console.WriteLine($"money1 = {money1}");
        Console.WriteLine();

        Money sum = money1 + money2;

        Console.WriteLine("Оператор +:");
        Console.WriteLine($"{money1} + {money2} = {sum}");
        Console.WriteLine();

        Money multiplied = money2 * 3;

        Console.WriteLine("Оператор *:");
        Console.WriteLine($"{money2} * 3 = {multiplied}");
        Console.WriteLine();

        Money money4 = new Money(2000m, "UAH");

        Console.WriteLine("Оператори == та !=:");
        Console.WriteLine($"{money1} == {money4}: {money1 == money4}");
        Console.WriteLine($"{money1} != {money3}: {money1 != money3}");
        Console.WriteLine();

        Console.WriteLine("Метод Equals():");
        Console.WriteLine($"money1.Equals(money4): {money1.Equals(money4)}");
        Console.WriteLine();

        Console.WriteLine("Метод GetHashCode():");
        Console.WriteLine($"HashCode money1: {money1.GetHashCode()}");
        Console.WriteLine($"HashCode money4: {money4.GetHashCode()}");
        Console.WriteLine();

        Console.WriteLine("Перевірка валідації:");

        try
        {
            money2.Amount = -500m;
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Помилка: {ex.Message}");
        }

        Console.WriteLine();

        try
        {
            Money wrongSum = money1 + money3;
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Помилка: {ex.Message}");
        }

        Console.WriteLine();
        Console.WriteLine("===== Роботу завершено =====");
    }
}