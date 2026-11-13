using System;
using System.IO;
using System.Net.Http;

namespace Lab17v3;

internal static class Program
{
    private static readonly Func<Exception, bool> RetryTransientIoAndNetworkErrors =
        exception => exception is IOException or HttpRequestException;

    private static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        TimeSpan initialDelay = TimeSpan.FromMilliseconds(25);

        Header("Лабораторна робота №17 — Retry та обробка IO/мережевих помилок");
        Console.WriteLine("Варіант 3: отримання списку замовлень.");

        Header("Читання замовлень із файлу (2 тимчасові IOException)");
        var fileProcessor = new FileProcessor();
        var orderIds = RetryHelper.ExecuteWithRetry(
            () => fileProcessor.GetLines("orders.txt"),
            retryCount: 3,
            initialDelay: initialDelay,
            shouldRetry: RetryTransientIoAndNetworkErrors);
        Console.WriteLine("Результат: " + string.Join(", ", orderIds));

        Header("Отримання замовлень з API (4 тимчасові HttpRequestException)");
        var networkClient = new NetworkClient();
        string response = RetryHelper.ExecuteWithRetry(
            () => networkClient.GetApiResponse("https://api.example.test/orders"),
            retryCount: 5,
            initialDelay: initialDelay,
            shouldRetry: RetryTransientIoAndNetworkErrors);
        Console.WriteLine("Результат: " + response);

        Header("Вибірковий повтор: помилка формату не повторюється");
        try
        {
            RetryHelper.ExecuteWithRetry(
                () => throw new InvalidDataException("Некоректний формат даних."),
                retryCount: 3,
                initialDelay: initialDelay,
                shouldRetry: RetryTransientIoAndNetworkErrors);
        }
        catch (InvalidDataException exception)
        {
            Console.WriteLine($"Оброблено без повтору: {exception.Message}");
        }

        Header("Висновок");
        Console.WriteLine("Retry повторює операцію після тимчасових IO або мережевих помилок, доки вона не завершиться успішно або не вичерпає ліміт.");
        Console.WriteLine("Експоненційна затримка подвоюється після кожної невдалої спроби: початкова затримка × 2^(номер повтору − 1).");
        Console.WriteLine("Делегат shouldRetry дозволяє повторювати лише ті типи винятків, які можуть бути тимчасовими.");
    }

    private static void Header(string title)
    {
        Console.WriteLine();
        Console.WriteLine($"=== {title} ===");
    }
}
