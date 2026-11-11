using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
 
namespace Lab15v3
{
    public class Author
    {
        public int Id { get; }
        public string Name { get; }
        public string Email { get; }
 
        public Author(int id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }
    }
 
    public class Post
    {
        public int Id { get; }
        public string Title { get; }
        public string Content { get; }
        public int AuthorId { get; }
 
        public Post(int id, string title, string content, int authorId)
        {
            Id = id;
            Title = title;
            Content = content;
            AuthorId = authorId;
        }
 
        public override string ToString() => $"#{Id} \"{Title}\" (автор: {AuthorId})";
    }
 
    internal class Program
    {
        private const int Iterations = 10_000;
 
        private static readonly List<(string Query, string Syntax, double FirstMs, double AvgUs)> Timings = new();
 
        private static readonly List<Author> Authors = new()
        {
            new Author(1, "Олена Коваль", "olena.koval@blog.ua"),
            new Author(2, "Андрій Шевчук", "andrii.shevchuk@blog.ua"),
            new Author(3, "Марія Бондар", "maria.bondar@blog.ua"),
            new Author(4, "Іван Мельник", "ivan.melnyk@blog.ua"),
            new Author(5, "Софія Ткачук", "sofia.tkachuk@blog.ua"),
            new Author(6, "Максим Кравчук", "maksym.kravchuk@blog.ua"),
            new Author(7, "Наталія Олійник", "nataliia.oliinyk@blog.ua"),
            new Author(8, "Дмитро Лисенко", "dmytro.lysenko@blog.ua"),
            new Author(9, "Ірина Савчук", "iryna.savchuk@blog.ua"),
            new Author(10, "Петро Гнатюк", "petro.hnatiuk@blog.ua")
        };
 
        private static readonly List<Post> Posts = new()
        {
            new Post(1, "Вступ до LINQ",
                "LINQ дозволяє писати запити до колекцій безпосередньо мовою C#.", 1),
            new Post(2, "Основи C#",
                "Змінні, типи даних та оператори. У наступних статтях розглянемо LINQ.", 1),
            new Post(3, "Робота з файлами",
                "Читання та запис текстових файлів за допомогою класів File і StreamReader.", 1),
            new Post(4, "Колекції в .NET",
                "List, Dictionary, HashSet, Queue та Stack: коли яку колекцію обрати.", 2),
            new Post(5, "Join та GroupJoin у LINQ",
                "Об'єднання двох колекцій за ключем і відмінність між Join та GroupJoin.", 2),
            new Post(6, "Принципи SOLID",
                "Принципи SOLID допомагають будувати гнучкий і підтримуваний код. " +
                "Принцип єдиної відповідальності вимагає, щоб у класу була лише одна причина для змін. " +
                "Принцип відкритості та закритості радить розширювати поведінку без зміни наявного коду. " +
                "Принцип підстановки Лісков гарантує, що похідні класи можна використовувати замість базових. " +
                "Принцип розділення інтерфейсів радить створювати невеликі спеціалізовані інтерфейси. " +
                "Принцип інверсії залежностей відокремлює високорівневу логіку від деталей реалізації.", 3),
            new Post(7, "Асинхронне програмування",
                "async та await спрощують роботу з потоками. Порівняємо з обробкою даних через linq.", 3),
            new Post(8, "Entity Framework для початківців",
                "ORM дозволяє працювати з базою даних через об'єкти C# без написання SQL вручну.", 3),
            new Post(9, "Unit-тести в xUnit",
                "Атрибути Fact і Theory, перевірки Assert та організація тестових проєктів.", 4),
            new Post(10, "Git для команди",
                "Гілки, злиття та pull request: як налагодити спільну роботу над проєктом.", 5),
            new Post(11, "Агрегації в LINQ",
                "Sum, Average, Min, Max, Count та Aggregate для обчислень над наборами даних.", 5),
            new Post(12, "Патерни проєктування",
                "Singleton, Factory, Observer та Strategy: типові рішення типових задач у програмуванні.", 6),
            new Post(13, "Docker для .NET-розробника",
                "Контейнеризація застосунків: Dockerfile, образи та запуск контейнерів.", 7),
            new Post(14, "Огляд Visual Studio 2022",
                "Нові можливості середовища розробки, налагодження та інструменти для рефакторингу.", 8),
            new Post(15, "Відкладене виконання в LINQ",
                "Запит виконується лише під час ітерації, а ToList виконує його негайно.", 8)
        };
 
        private static void Header(string text)
        {
            Console.WriteLine();
            Console.WriteLine($"=== {text} ===");
        }
         private static T Run<T>(string queryName, string syntax, Func<T> query)
        {
            var sw = Stopwatch.StartNew();
            T result = query();
            sw.Stop();
            double firstMs = sw.Elapsed.TotalMilliseconds;
 
            sw.Restart();
            for (int i = 0; i < Iterations; i++)
                query();
            sw.Stop();
            double avgUs = sw.Elapsed.TotalMilliseconds * 1000.0 / Iterations;
 
            Timings.Add((queryName, syntax, firstMs, avgUs));
            Console.WriteLine($"  [{syntax}] перший запуск: {firstMs:F4} мс; " +
                              $"середнє за {Iterations} запусків: {avgUs:F3} мкс");
            return result;
        }
 
        private static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
 
            Header("Запит 1. Пости, що містять слово \"LINQ\" у заголовку або вмісті (Where)");
 
            var q1Query = Run("Запит 1", "синтаксис запитів", () =>
                (from p in Posts
                 where p.Title.Contains("LINQ", StringComparison.OrdinalIgnoreCase)
                       || p.Content.Contains("LINQ", StringComparison.OrdinalIgnoreCase)
                 select p).ToList());
 
            var q1Method = Run("Запит 1", "синтаксис методів", () =>
                Posts.Where(p => p.Title.Contains("LINQ", StringComparison.OrdinalIgnoreCase)
                                 || p.Content.Contains("LINQ", StringComparison.OrdinalIgnoreCase))
                     .ToList());
 
            Console.WriteLine($"  Знайдено постів: {q1Method.Count}");
            foreach (var p in q1Method)
                Console.WriteLine($"    {p}");
            Console.WriteLine($"  Результати обох синтаксисів збігаються: {q1Query.SequenceEqual(q1Method)}");
 
            Header("Запит 2. Пости з авторами: заголовок та ім'я автора (Join)");
 
            var q2Query = Run("Запит 2", "синтаксис запитів", () =>
                (from p in Posts
                 join a in Authors on p.AuthorId equals a.Id
                 select new { p.Title, AuthorName = a.Name }).ToList());
 
            var q2Method = Run("Запит 2", "синтаксис методів", () =>
                Posts.Join(Authors,
                           p => p.AuthorId,
                           a => a.Id,
                           (p, a) => new { p.Title, AuthorName = a.Name })
                     .ToList());
 
            foreach (var item in q2Method)
                Console.WriteLine($"    \"{item.Title}\" - {item.AuthorName}");
            Console.WriteLine($"  Результати обох синтаксисів збігаються: {q2Query.SequenceEqual(q2Method)}");
 
            Header("Запит 3. Кількість постів для кожного автора (GroupJoin + Count)");
 
            var q3Query = Run("Запит 3", "синтаксис запитів", () =>
                (from a in Authors
                 join p in Posts on a.Id equals p.AuthorId into authorPosts
                 select new { a.Name, PostCount = authorPosts.Count() }).ToList());
 
            var q3Method = Run("Запит 3", "синтаксис методів", () =>
                Authors.GroupJoin(Posts,
                                  a => a.Id,
                                  p => p.AuthorId,
                                  (a, authorPosts) => new { a.Name, PostCount = authorPosts.Count() })
                       .ToList());
 
            foreach (var item in q3Method)
                Console.WriteLine($"    {item.Name}: {item.PostCount}");
            Console.WriteLine($"  Результати обох синтаксисів збігаються: {q3Query.SequenceEqual(q3Method)}");
 
            Header("Запит 4. Пост з найдовшим вмістом (orderby + First / MaxBy)");
 
            var q4Query = Run("Запит 4", "синтаксис запитів", () =>
                (from p in Posts
                 orderby p.Content.Length descending
                 select p).First());
 
            var q4Method = Run("Запит 4", "синтаксис методів", () =>
                Posts.MaxBy(p => p.Content.Length)!);
 
            Console.WriteLine($"  Найдовший пост: {q4Method}, довжина вмісту: {q4Method.Content.Length} символів");
            Console.WriteLine($"  Результати обох синтаксисів збігаються: {ReferenceEquals(q4Query, q4Method)}");
 
            Header("Відкладене та негайне виконання");
 
            var deferred = Posts.Where(p => p.AuthorId == 1);
            var immediate = Posts.Where(p => p.AuthorId == 1).ToList();
 
            Console.WriteLine($"  До додавання поста: deferred = {deferred.Count()}, immediate = {immediate.Count}");
 
            var temp = new Post(16, "Тимчасовий пост", "Пост для демонстрації відкладеного виконання.", 1);
            Posts.Add(temp);
            Console.WriteLine($"  Після додавання поста: deferred = {deferred.Count()}, immediate = {immediate.Count}");
            Posts.Remove(temp);
 
            Console.WriteLine("  Where без ToList виконується під час ітерації й бачить нові дані,");
            Console.WriteLine("  а ToList зберігає знімок результату на момент виклику.");
 
            Header("Аналіз продуктивності (відсортовано за середнім часом, від найповільнішого)");
            Console.WriteLine($"  {"Запит",-10}{"Синтаксис",-20}{"Перший запуск, мс",20}{"Середнє, мкс",18}");
            foreach (var t in Timings.OrderByDescending(t => t.AvgUs))
                Console.WriteLine($"  {t.Query,-10}{t.Syntax,-20}{t.FirstMs,20:F4}{t.AvgUs,18:F3}");
 
            Header("Висновки");
            Console.WriteLine("  1. Перший запуск кожного запиту значно повільніший за середнє значення:");
            Console.WriteLine("     на ньому позначаються JIT-компіляція та ініціалізація LINQ.");
            Console.WriteLine("  2. Фільтрація з Contains переглядає символи заголовка та вмісту кожного поста,");
            Console.WriteLine("     тому її вартість залежить від довжини текстів.");
            Console.WriteLine("  3. Join і GroupJoin будують хеш-таблицю за ключем однієї колекції,");
            Console.WriteLine("     тому працюють за O(n + m), а не O(n * m), як вкладені цикли.");
            Console.WriteLine("  4. orderby + First сортує всю колекцію (O(n log n)), а MaxBy робить один прохід (O(n)).");
            Console.WriteLine("     Для пошуку максимуму сортування зайве.");
            Console.WriteLine("  5. Обидва синтаксиси компілюються в однакові виклики методів,");
            Console.WriteLine("     тому різниця у швидкості між ними зумовлена лише різними операторами,");
            Console.WriteLine("     а не самим синтаксисом.");
        }
    }
}
 