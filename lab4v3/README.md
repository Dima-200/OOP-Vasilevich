# Лабораторна робота №4

## Тема

**Властивості та статичні члени. Індексатори й перевантаження операторів.**

## Мета роботи

Навчитися реалізовувати властивості з перевіркою значень, використовувати статичні члени класу, а також перевантажувати оператори для створення більш виразних та зручних класів.

## Варіант

**Варіант №3 — клас Money**

---

## Завдання

Реалізувати клас `Money`, який містить:

- приватні поля `_amount` та `_currency`;
- властивість `Amount` з валідацією — сума не може бути від'ємною;
- властивість `Currency`;
- статичний член `DefaultCurrency`;
- перевантажений оператор `+` для додавання двох сум;
- перевантажений оператор `*` для множення суми на число;
- перевантажені оператори `==` та `!=`;
- перевизначені методи `ToString()`, `Equals()` та `GetHashCode()`.

Індексатор для класу `Money` не використовується, оскільки для даної сутності він недоцільний.

---

## Хід роботи

### 1. Створення проєкту

Проєкт створено за допомогою команди:

```powershell
dotnet new console -o lab4v3

OOP-Vasilevich/
└── lab4v3/
    ├── Program.cs
    └── README.md

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

![alt text](image.png)

Контрольні питання
1. Яка різниця між звичайною властивістю та індексатором?
Звичайна властивість використовується для доступу до конкретної характеристики об'єкта. Наприклад, у класі Money властивості Amount та Currency дозволяють отримувати та змінювати суму і валюту.
Індексатор дозволяє звертатися до об'єкта за індексом або ключем за допомогою квадратних дужок []. Індексатори доцільно використовувати для класів, які представляють колекції або містять набір елементів.
У класі Money індексатор не використовується, оскільки об'єкт представляє одну грошову суму, а не колекцію.

2. Навіщо потрібно перевизначати Equals() та GetHashCode() при перевантаженні операторів == та !=?
Equals() використовується для логічного порівняння об'єктів за їхнім вмістом.
У класі Money два об'єкти вважаються однаковими, якщо вони мають однакову суму та однакову валюту.
GetHashCode() повертає хеш-код об'єкта. Якщо два об'єкти вважаються рівними, вони повинні мати однаковий хеш-код.
Тому при перевантаженні == та != доцільно забезпечити узгоджену поведінку Equals() і GetHashCode().

3. У яких випадках доцільно використовувати статичні члени класу?
Статичні члени використовуються тоді, коли значення або метод належить самому класу, а не окремому об'єкту.
Наприклад, у класі Money статична властивість:
Money.DefaultCurrency
містить валюту за замовчуванням і є спільною для всіх об'єктів класу.
Статичні члени зручно використовувати для спільних налаштувань, констант, лічильників та допоміжних методів.

4. Як інкапсуляція допомагає підтримувати цілісність даних у класі, який ви реалізували?
Інкапсуляція дозволяє приховати внутрішні поля класу та контролювати доступ до них через публічні властивості.
У класі Money поля _amount та _currency є приватними. Змінити їх безпосередньо ззовні неможливо.
Значення Amount проходить перевірку перед присвоєнням. Завдяки цьому неможливо встановити від'ємну суму.
Таким чином, інкапсуляція допомагає захищати дані та підтримувати коректний стан об'єкта.

Висновок
Під час виконання лабораторної роботи було створено клас Money мовою C#. Було реалізовано приватні поля та публічні властивості з валідацією даних. Також використано статичний член класу DefaultCurrency.
Було виконано перевантаження операторів +, *, == та !=, а також перевизначено методи ToString(), Equals() і GetHashCode().
У методі Main() продемонстровано створення та використання обєктів класу, зміну властивостей, виконання операцій з грошовими сумами та роботу валідації.
У результаті було закріплено практичні навички роботи з властивостями, статичними членами, інкапсуляцією та перевантаженням операторів у C#.
