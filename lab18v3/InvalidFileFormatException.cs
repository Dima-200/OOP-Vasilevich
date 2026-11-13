namespace Lab18v3;

public sealed class InvalidFileFormatException : Exception
{
    public string FileName { get; }
    public string ExpectedFormat { get; }

    public InvalidFileFormatException(string fileName, string expectedFormat)
        : base($"Файл '{fileName}' має неправильний формат. Очікується {expectedFormat}.")
    {
        FileName = fileName;
        ExpectedFormat = expectedFormat;
    }

    public InvalidFileFormatException(string fileName, string expectedFormat, Exception innerException)
        : base($"Не вдалося обробити файл '{fileName}'. Очікується {expectedFormat}.", innerException)
    {
        FileName = fileName;
        ExpectedFormat = expectedFormat;
    }
}
