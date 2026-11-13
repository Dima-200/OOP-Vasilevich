using System;
using System.IO;

namespace Lab18v3;

public sealed class FileProcessorService
{
    private const string ExpectedExtension = ".txt";

    public string ReadFile(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        string fileName = Path.GetFileName(path);
        if (!string.Equals(Path.GetExtension(path), ExpectedExtension, StringComparison.OrdinalIgnoreCase))
            throw new InvalidFileFormatException(fileName, ExpectedExtension);

        return File.ReadAllText(path);
    }

    public Result<bool> WriteToFile(string path, string content)
    {
        if (string.IsNullOrWhiteSpace(path))
            return Result<bool>.Failure("Шлях до файлу не може бути порожнім.");

        try
        {
            File.WriteAllText(path, content ?? string.Empty);
            return Result<bool>.Success(true);
        }
        catch (Exception exception) when (exception is ArgumentException
                                          or IOException
                                          or UnauthorizedAccessException
                                          or NotSupportedException
                                          or System.Security.SecurityException)
        {
            return Result<bool>.Failure(exception);
        }
    }
}
