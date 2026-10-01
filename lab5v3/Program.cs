using System;
using System.Collections.Generic;
using System.Linq;

class SimpleDictionary
{
    private Dictionary<string, int> _data;

    public int this[string key]
    {
        get
        {
            if (!_data.ContainsKey(key))
                throw new KeyNotFoundException("Ключ не знайдено.");

            return _data[key];
        }
        set
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Ключ не може бути порожнім.");

            _data[key] = value;
        }
    }

    public int Count
    {
        get { return _data.Count; }
    }

    public SimpleDictionary()
    {
        _data = new Dictionary<string, int>();
    }

    public bool ContainsKey(string key)
    {
        return _data.ContainsKey(key);
    }

    public static SimpleDictionary operator +(SimpleDictionary a, SimpleDictionary b)
    {
        SimpleDictionary result = new SimpleDictionary();

        foreach (var item in a._data)
        {
            result._data[item.Key] = item.Value;
        }

        foreach (var item in b._data)
        {
            result._data[item.Key] = item.Value;
        }

        return result;
    }

    public static bool operator ==(SimpleDictionary? a, SimpleDictionary? b)
    {
        if (ReferenceEquals(a, b))
            return true;

        if (a is null || b is null)
            return false;

        return a._data.Count == b._data.Count &&
               a._data.All(x =>
                   b._data.ContainsKey(x.Key) &&
                   b._data[x.Key] == x.Value);
    }

    public static bool operator !=(SimpleDictionary? a, SimpleDictionary? b)
    {
        return !(a == b);
    }

    public override string ToString()
    {
        if (_data.Count == 0)
            return "Словник порожній.";

        return string.Join(", ",
            _data.Select(x => $"{x.Key}: {x.Value}"));
    }

    public override bool Equals(object? obj)
    {
        if (obj is not SimpleDictionary other)
            return false;

        return this == other;
    }

    public override int GetHashCode()
    {
        int hash = 17;

        foreach (var item in _data.OrderBy(x => x.Key))
        {
            hash = hash * 31 + HashCode.Combine(item.Key, item.Value);
        }

        return hash;
    }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("===== Лабораторна робота №5 =====");
        Console.WriteLine("Варіант №3 — клас SimpleDictionary");
        Console.WriteLine();

        SimpleDictionary dictionary1 = new SimpleDictionary();
        SimpleDictionary dictionary2 = new SimpleDictionary();

        dictionary1["Іван"] = 101;
        dictionary1["Олександр"] = 102;
        dictionary1["Андрій"] = 103;

        dictionary2["Марія"] = 104;
        dictionary2["Олена"] = 105;

        Console.WriteLine("Перший словник:");
        Console.WriteLine(dictionary1);
        Console.WriteLine();

        Console.WriteLine("Другий словник:");
        Console.WriteLine(dictionary2);
        Console.WriteLine();

        Console.WriteLine("===== Робота індексатора =====");

        Console.WriteLine($"ID студента Іван: {dictionary1["Іван"]}");

        dictionary1["Іван"] = 201;

        Console.WriteLine("Після зміни значення:");
        Console.WriteLine($"ID студента Іван: {dictionary1["Іван"]}");
        Console.WriteLine();

        Console.WriteLine("Кількість елементів:");
        Console.WriteLine($"dictionary1.Count = {dictionary1.Count}");
        Console.WriteLine($"dictionary2.Count = {dictionary2.Count}");
        Console.WriteLine();

        Console.WriteLine("Перевірка ContainsKey():");
        Console.WriteLine(
            $"Чи існує ключ \"Олександр\": {dictionary1.ContainsKey("Олександр")}");
        Console.WriteLine(
            $"Чи існує ключ \"Петро\": {dictionary1.ContainsKey("Петро")}");
        Console.WriteLine();

        Console.WriteLine("===== Оператор + =====");

        SimpleDictionary dictionary3 = dictionary1 + dictionary2;

        Console.WriteLine("Об'єднаний словник:");
        Console.WriteLine(dictionary3);
        Console.WriteLine();

        Console.WriteLine("===== Оператори == та != =====");

        SimpleDictionary dictionary4 = new SimpleDictionary();

        dictionary4["Іван"] = 201;
        dictionary4["Олександр"] = 102;
        dictionary4["Андрій"] = 103;

        Console.WriteLine(
            $"dictionary1 == dictionary4: {dictionary1 == dictionary4}");

        Console.WriteLine(
            $"dictionary1 != dictionary2: {dictionary1 != dictionary2}");

        Console.WriteLine();

        Console.WriteLine("===== Equals() =====");

        Console.WriteLine(
            $"dictionary1.Equals(dictionary4): {dictionary1.Equals(dictionary4)}");

        Console.WriteLine();

        Console.WriteLine("===== GetHashCode() =====");

        Console.WriteLine(
            $"HashCode dictionary1: {dictionary1.GetHashCode()}");

        Console.WriteLine(
            $"HashCode dictionary4: {dictionary4.GetHashCode()}");

        Console.WriteLine();

        Console.WriteLine("===== Кінець програми =====");
    }
}