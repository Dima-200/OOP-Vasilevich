using System;
using System.IO;

namespace Lab18v3;

internal static class Program
{
    private static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        var service = new FileProcessorService();
        string samplePath = Path.Combine(AppContext.BaseDirectory, "sample.txt");

        Header("Лабораторна робота №18 — власні винятки та патерн Result");
        Console.WriteLine("Варіант 3: обробка файлів. Очікуваний формат: .txt.");

        Header("Читання файлу правильного формату");
        try
        {
            Console.WriteLine(service.ReadFile(samplePath));
        }
        catch (InvalidFileFormatException exception)
        {
            PrintFileFormatError(exception);
        }
        catch (IOException exception)
        {
            Console.WriteLine($"Помилка читання: {exception.Message}");
        }

        Header("Власний виняток для неправильного формату");
        try
        {
            service.ReadFile("report.csv");
        }
        catch (InvalidFileFormatException exception)
        {
            PrintFileFormatError(exception);
        }

        Header("Запис файлу через Result<T> — успіх");
        var writeResult = service.WriteToFile("written-result.txt", "Результат записано успішно.");
        PrintResult(writeResult);

        Header("Запис файлу через Result<T> — помилка очікуваного сценарію");
        var invalidPathResult = service.WriteToFile("", "Текст");
        PrintResult(invalidPathResult);

        Header("Висновок");
        Console.WriteLine("Власний виняток дає змогу чітко позначити неправильний формат файлу — це помилка виконання операції.");
        Console.WriteLine("Result<bool> зручний для очікуваної невдачі запису, як-от порожній шлях або відсутність дозволу: викликач перевіряє результат без винятку.");
    }

    private static void PrintFileFormatError(InvalidFileFormatException exception)
    {
        Console.WriteLine($"{exception.Message} Ім'я: {exception.FileName}; очікуваний формат: {exception.ExpectedFormat}.");
    }

    private static void PrintResult(Result<bool> result)
    {
        if (result.IsSuccess)
            Console.WriteLine($"Успіх: файл записано ({result.Value}).");
        else
            Console.WriteLine($"Невдача: {result.Error}");
    }

    private static void Header(string title)
    {
        Console.WriteLine();
        Console.WriteLine($"=== {title} ===");
    }
}
