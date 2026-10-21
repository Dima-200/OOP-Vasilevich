
using System;
using System.Collections.Generic;
using System.Text;

interface IEncryptor
{
    string Encrypt(string data);
    string Decrypt(string encryptedData);
}

class CaesarCipher : IEncryptor
{
    private const int Shift = 3;

    public string Encrypt(string data)
    {
        StringBuilder result = new StringBuilder();

        foreach (char c in data)
        {
            if (c >= 'a' && c <= 'z')
                result.Append((char)('a' + (c - 'a' + Shift) % 26));
            else if (c >= 'A' && c <= 'Z')
                result.Append((char)('A' + (c - 'A' + Shift) % 26));
            else
                result.Append(c);
        }

        return result.ToString();
    }

    public string Decrypt(string encryptedData)
    {
        StringBuilder result = new StringBuilder();

        foreach (char c in encryptedData)
        {
            if (c >= 'a' && c <= 'z')
                result.Append((char)('a' + (c - 'a' - Shift + 26) % 26));
            else if (c >= 'A' && c <= 'Z')
                result.Append((char)('A' + (c - 'A' - Shift + 26) % 26));
            else
                result.Append(c);
        }

        return result.ToString();
    }
}

class ReverseEncryptor : IEncryptor
{
    public string Encrypt(string data)
    {
        char[] characters = data.ToCharArray();
        Array.Reverse(characters);
        return new string(characters);
    }

    public string Decrypt(string encryptedData)
    {
        return Encrypt(encryptedData);
    }
}

abstract class ReportGenerator
{
    protected string reportContent = "";

    public abstract void CollectData();
    public abstract void FormatReport();

    public void GenerateOutput()
    {
        CollectData();
        FormatReport();

        Console.WriteLine(reportContent);
    }
}

class PdfReportGenerator : ReportGenerator
{
    public override void CollectData()
    {
        reportContent = "Звіт містить результати лабораторної роботи.";
    }

    public override void FormatReport()
    {
        reportContent = "[PDF-ЗВІТ]\n" + reportContent +
                        "\nФормат: PDF";
    }
}

class HtmlReportGenerator : ReportGenerator
{
    public override void CollectData()
    {
        reportContent = "Звіт містить результати лабораторної роботи.";
    }

    public override void FormatReport()
    {
        reportContent = "<html>\n<body>\n<h1>HTML-ЗВІТ</h1>\n<p>" +
                        reportContent +
                        "</p>\n</body>\n</html>";
    }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        Console.WriteLine("=== ЛАБОРАТОРНА РОБОТА №10 ===");
        Console.WriteLine("Варіант 3: IEncryptor та ReportGenerator\n");

        List<IEncryptor> encryptors = new List<IEncryptor>
        {
            new CaesarCipher(),
            new ReverseEncryptor()
        };

        string message = "Hello World";

        Console.WriteLine("=== ШИФРУВАННЯ ===");
        Console.WriteLine($"Початковий текст: {message}\n");

        foreach (IEncryptor encryptor in encryptors)
        {
            string encrypted = encryptor.Encrypt(message);
            string decrypted = encryptor.Decrypt(encrypted);

            Console.WriteLine($"Клас: {encryptor.GetType().Name}");
            Console.WriteLine($"Зашифрований текст: {encrypted}");
            Console.WriteLine($"Розшифрований текст: {decrypted}\n");
        }

        List<ReportGenerator> generators = new List<ReportGenerator>
        {
            new PdfReportGenerator(),
            new HtmlReportGenerator()
        };

        Console.WriteLine("=== ГЕНЕРАЦІЯ ЗВІТІВ ===");

        foreach (ReportGenerator generator in generators)
        {
            Console.WriteLine($"\nГенератор: {generator.GetType().Name}");
            generator.GenerateOutput();
        }

        Console.WriteLine("\nПрограму завершено.");
    }
}
