
using System;
using System.IO;

public class InvalidFileFormatException : Exception
{
    public string FileName { get; }
    public string ExpectedFormat { get; }

    public InvalidFileFormatException(string fileName, string expectedFormat)
        : base($"Файл '{fileName}' має неправильний формат. Очікується: {expectedFormat}")
    {
        FileName = fileName;
        ExpectedFormat = expectedFormat;
    }
}

public class Result<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public Exception? Error { get; }

    private Result(bool isSuccess, T? value, Exception? error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    public static Result<T> Success(T value)
    {
        return new Result<T>(true, value, null);
    }

    public static Result<T> Failure(Exception error)
    {
        return new Result<T>(false, default, error);
    }
}

public class FileProcessorService
{
    public string ReadFile(string path)
    {
        string extension = Path.GetExtension(path);

        if (!extension.Equals(".txt", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidFileFormatException(
                Path.GetFileName(path),
                ".txt"
            );
        }

        return File.ReadAllText(path);
    }

    public Result<bool> WriteToFile(string path, string content)
    {
        try
        {
            File.WriteAllText(path, content);
            return Result<bool>.Success(true);
        }
        catch (Exception ex) when (
            ex is IOException ||
            ex is UnauthorizedAccessException ||
            ex is ArgumentException)
        {
            return Result<bool>.Failure(ex);
        }
    }
}

public class Program
{
    public static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        FileProcessorService service = new FileProcessorService();

        Console.WriteLine("========================================");
        Console.WriteLine("ЛАБОРАТОРНА РОБОТА №18");
        Console.WriteLine("Власні винятки та патерн Result");
        Console.WriteLine("========================================");

        File.WriteAllText("example.txt", "Це приклад текстового файлу.");
        File.WriteAllText("example.csv", "id,name");

        Console.WriteLine("\nЗАВДАННЯ 1. READFILE");
        Console.WriteLine("----------------------------------------");

        try
        {
            string content = service.ReadFile("example.txt");
            Console.WriteLine("Файл успішно прочитано:");
            Console.WriteLine(content);
        }
        catch (InvalidFileFormatException ex)
        {
            Console.WriteLine("Помилка формату файлу:");
            Console.WriteLine(ex.Message);
            Console.WriteLine($"Назва файлу: {ex.FileName}");
            Console.WriteLine($"Очікуваний формат: {ex.ExpectedFormat}");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Помилка читання: {ex.Message}");
        }

        Console.WriteLine("\nПеревірка неправильного формату:");

        try
        {
            service.ReadFile("example.csv");
        }
        catch (InvalidFileFormatException ex)
        {
            Console.WriteLine($"Перехоплено виняток: {ex.Message}");
        }

        Console.WriteLine("\nЗАВДАННЯ 2. WRITETOFILE");
        Console.WriteLine("----------------------------------------");

        Result<bool> writeResult = service.WriteToFile(
            "output.txt",
            "Дані успішно записані у файл."
        );

        if (writeResult.IsSuccess)
        {
            Console.WriteLine("Запис файлу виконано успішно.");
            Console.WriteLine($"Результат: {writeResult.Value}");
        }
        else
        {
            Console.WriteLine($"Помилка запису: {writeResult.Error?.Message}");
        }

        Console.WriteLine("\nЗАВДАННЯ 3. RESULT FAILURE");
        Console.WriteLine("----------------------------------------");

        Result<bool> failureResult = Result<bool>.Failure(
            new IOException("Демонстраційна помилка запису.")
        );

        Console.WriteLine($"Успішне виконання: {failureResult.IsSuccess}");
        Console.WriteLine($"Помилка: {failureResult.Error?.Message}");

        Console.WriteLine("\n========================================");
        Console.WriteLine("ВИКОНАННЯ ПРОГРАМИ ЗАВЕРШЕНО!");
        Console.WriteLine("========================================");
    }
}
