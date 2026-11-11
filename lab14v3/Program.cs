using System;
using System.Collections.Generic;
using System.Diagnostics;
 
namespace Lab14v3
{
    public class Document
    {
        public int Id { get; }
        public string Title { get; }
        public string User { get; }
        public int Pages { get; }
 
        public Document(int id, string title, string user, int pages)
        {
            Id = id;
            Title = title;
            User = user;
            Pages = pages;
        }
 
        public override string ToString() =>
            $"#{Id} \"{Title}\" (користувач: {User}, сторінок: {Pages})";
    }
 
    public class PrintSystem
    {
        private readonly HashSet<string> _users = new HashSet<string>();
 
        private readonly Queue<Document> _printQueue = new Queue<Document>();
 
        private readonly Stack<Document> _printedHistory = new Stack<Document>();
 
        public void Submit(Document doc)
        {
            _printQueue.Enqueue(doc);
            bool isNewUser = _users.Add(doc.User);
            Console.WriteLine($"  Додано в чергу: {doc}");
            Console.WriteLine(isNewUser
                ? $"  Новий користувач: {doc.User}"
                : $"  Користувач {doc.User} уже є в множині (дублікат не додано)");
        }
 
        public bool HasUser(string user) => _users.Contains(user);
 
        public bool RemoveUser(string user) => _users.Remove(user);
 
        public void PrintUsers()
        {
            Console.WriteLine($"  Унікальних користувачів: {_users.Count}");
            foreach (var u in _users)
                Console.WriteLine($"    - {u}");
        }
 
        public HashSet<string> Users => _users;
         public Document? PeekNext() =>
            _printQueue.Count > 0 ? _printQueue.Peek() : null;
 
        public Document? PrintNext()
        {
            if (_printQueue.Count == 0)
            {
                Console.WriteLine("  Черга порожня.");
                return null;
            }
 
            Document doc = _printQueue.Dequeue();
            Console.WriteLine($"  Друкується: {doc}");
            _printedHistory.Push(doc);
            return doc;
        }
 
        public void PrintAll()
        {
            while (_printQueue.Count > 0)
                PrintNext();
        }
 
        public int QueueCount => _printQueue.Count;
 
        public void ShowQueue()
        {
            Console.WriteLine($"  Документів у черзі: {_printQueue.Count}");
            int pos = 1;
            foreach (var d in _printQueue)
                Console.WriteLine($"    {pos++}. {d}");
        }
 
        public Document? PeekLastPrinted() =>
            _printedHistory.Count > 0 ? _printedHistory.Peek() : null;
 
        public Document? PopLastPrinted()
        {
            if (_printedHistory.Count == 0)
            {
                Console.WriteLine("  Історія порожня.");
                return null;
            }
            return _printedHistory.Pop();
        }
 
        public int HistoryCount => _printedHistory.Count;
 
        public void ShowHistory()
        {
            Console.WriteLine($"  Надрукованих документів в історії: {_printedHistory.Count}");
            foreach (var d in _printedHistory) 
                Console.WriteLine($"    {d}");
        }
    }
 
    internal class Program
    {
        private static void Header(string text)
        {
            Console.WriteLine();
            Console.WriteLine($"=== {text} ===");
        }
 
        private static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            var system = new PrintSystem();
 
            Header("1. Надсилання документів (Queue + HashSet)");
            system.Submit(new Document(1, "Курсова робота", "Олена", 30));
            system.Submit(new Document(2, "Договір", "Андрій", 5));
            system.Submit(new Document(3, "Реферат", "Олена", 12));
            system.Submit(new Document(4, "Звіт з лабораторної", "Марія", 8));
            system.Submit(new Document(5, "Презентація", "Андрій", 20)); 
 
            Header("2. HashSet: унікальні користувачі");
            system.PrintUsers();
            Console.WriteLine($"  Чи є користувач Марія? {system.HasUser("Марія")}");
            Console.WriteLine($"  Чи є користувач Іван? {system.HasUser("Іван")}");
            Console.WriteLine($"  Видалення користувача Марія: {system.RemoveUser("Марія")}");
            Console.WriteLine($"  Повторне видалення Марія: {system.RemoveUser("Марія")}");
            system.PrintUsers();
 
            var otherUsers = new HashSet<string> { "Андрій", "Іван", "Софія" };
            var union = new HashSet<string>(system.Users);
            union.UnionWith(otherUsers);
            var intersect = new HashSet<string>(system.Users);
            intersect.IntersectWith(otherUsers);
            var except = new HashSet<string>(system.Users);
            except.ExceptWith(otherUsers);
            Console.WriteLine($"  UnionWith:     {string.Join(", ", union)}");
            Console.WriteLine($"  IntersectWith: {string.Join(", ", intersect)}");
            Console.WriteLine($"  ExceptWith:    {string.Join(", ", except)}");
 
            Header("3. Queue: черга друку (FIFO)");
            system.ShowQueue();
            Console.WriteLine($"  Peek (наступний на друк): {system.PeekNext()}");
            Console.WriteLine("  Друкуємо перші два документи:");
            system.PrintNext();
            system.PrintNext();
            Console.WriteLine("  Стан черги після друку:");
            system.ShowQueue();
            Console.WriteLine("  Друкуємо решту:");
            system.PrintAll();
            system.PrintNext();
 
            Header("4. Stack: історія надрукованих (LIFO)");
            system.ShowHistory();
            Console.WriteLine($"  Peek (останній надрукований): {system.PeekLastPrinted()}");
            Console.WriteLine($"  Pop: {system.PopLastPrinted()}");
            Console.WriteLine($"  Pop: {system.PopLastPrinted()}");
            Console.WriteLine("  Історія після двох Pop:");
            system.ShowHistory();
 
            Header("5. Порівняння продуктивності: List vs HashSet (Contains)");
            const int n = 100_000;
            var list = new List<int>();
            var set = new HashSet<int>();
            for (int i = 0; i < n; i++)
            {
                list.Add(i);
                set.Add(i);
            }
 
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < 1000; i++) list.Contains(n - 1);
            sw.Stop();
            long listMs = sw.ElapsedMilliseconds;
 
            sw.Restart();
            for (int i = 0; i < 1000; i++) set.Contains(n - 1);
            sw.Stop();
            long setMs = sw.ElapsedMilliseconds;
 
            Console.WriteLine($"  List<int>.Contains x1000 при {n} елементів: {listMs} мс (O(n))");
            Console.WriteLine($"  HashSet<int>.Contains x1000 при {n} елементів: {setMs} мс (O(1))");
 
            Console.WriteLine();
            Console.WriteLine("Готово.");
        }
    }
}
 