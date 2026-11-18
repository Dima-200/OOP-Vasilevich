
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;


Console.OutputEncoding = System.Text.Encoding.UTF8;

FileProcessor fileProcessor = new FileProcessor();
NetworkClient networkClient = new NetworkClient();

Func<Exception, bool> shouldRetry = exception =>
    exception is IOException ||
    exception is HttpRequestException;


Console.WriteLine("========================================");
Console.WriteLine("ЗАВДАННЯ 1. FILE PROCESSOR");
Console.WriteLine("Отримання рядків із файлу");
Console.WriteLine("========================================");

try
{
    List<string> lines = RetryHelper.ExecuteWithRetry(
        () => fileProcessor.GetLines("data.txt"),
        retryCount: 3,
        initialDelay: TimeSpan.FromMilliseconds(200),
        shouldRetry: shouldRetry
    );

    Console.WriteLine("\nОтримані рядки:");

    foreach (string line in lines)
    {
        Console.WriteLine(line);
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Помилка: {ex.Message}");
}

Console.WriteLine("\n========================================");
Console.WriteLine("ЗАВДАННЯ 2. NETWORK CLIENT");
Console.WriteLine("Запит до API");
Console.WriteLine("========================================");

try
{
    string response = RetryHelper.ExecuteWithRetry(
        () => networkClient.GetApiResponse(
            "https://api.example.com/users"),
        retryCount: 5,
        initialDelay: TimeSpan.FromMilliseconds(200),
        shouldRetry: shouldRetry
    );

    Console.WriteLine("\nВідповідь API:");
    Console.WriteLine(response);
}
catch (Exception ex)
{
    Console.WriteLine($"Помилка: {ex.Message}");
}


Console.WriteLine("\n========================================");
Console.WriteLine("ЗАВДАННЯ 3. SHOULDRETRY");
Console.WriteLine("Перевірка типу винятку");
Console.WriteLine("========================================");

Exception ioException = new IOException("Помилка файлу");
Exception networkException = new HttpRequestException("Помилка мережі");
Exception argumentException = new ArgumentException("Неправильний аргумент");

Console.WriteLine(
    $"IOException: {shouldRetry(ioException)}");

Console.WriteLine(
    $"HttpRequestException: {shouldRetry(networkException)}");

Console.WriteLine(
    $"ArgumentException: {shouldRetry(argumentException)}");

Console.WriteLine("\n========================================");
Console.WriteLine("ВИКОНАННЯ ПРОГРАМИ ЗАВЕРШЕНО!");
Console.WriteLine("========================================");


public class FileProcessor
{
    private int attempts = 0;

    public List<string> GetLines(string path)
    {
        attempts++;

        Console.WriteLine($"FileProcessor: спроба {attempts}");

        if (attempts <= 2)
        {
            throw new IOException(
                "Тимчасова помилка читання файлу.");
        }

        return new List<string>
        {
            "Перший рядок файлу",
            "Другий рядок файлу",
            "Третій рядок файлу"
        };
    }
}


public class NetworkClient
{
    private int attempts = 0;

    public string GetApiResponse(string endpoint)
    {
        attempts++;

        Console.WriteLine($"NetworkClient: спроба {attempts}");

        if (attempts <= 4)
        {
            throw new HttpRequestException(
                "Тимчасова помилка підключення до API.");
        }

        return "Успішна відповідь від API!";
    }
}

public static class RetryHelper
{
    public static T ExecuteWithRetry<T>(
        Func<T> operation,
        int retryCount = 3,
        TimeSpan initialDelay = default,
        Func<Exception, bool> shouldRetry = null)
    {
        int attempt = 0;

        while (true)
        {
            try
            {
                attempt++;

                Console.WriteLine(
                    $"\nВиконання операції. Спроба {attempt}");

                return operation();
            }
            catch (Exception ex)
            {
                bool canRetry = shouldRetry == null ||
                                shouldRetry(ex);

                if (!canRetry || attempt > retryCount)
                {
                    Console.WriteLine(
                        $"Повторення неможливе: {ex.Message}");

                    throw;
                }

                double delay = initialDelay.TotalMilliseconds *
                               Math.Pow(2, attempt - 1);

                Console.WriteLine(
                    $"Помилка: {ex.Message}");

                Console.WriteLine(
                    $"Повторна спроба через {delay} мс");

                Thread.Sleep(TimeSpan.FromMilliseconds(delay));
            }
        }
    }
}
