
using System;
using System.Collections.Generic;

abstract class ReportGenerator
{
    public string ReportName { get; }
    public string Status { get; protected set; } = "Не створено";

    protected ReportGenerator(string reportName)
    {
        ReportName = reportName;
    }

    public void Generate()
    {
        Console.WriteLine($"\n=== Генерація звіту: {ReportName} ===");

        FetchData();
        ProcessData();
        FormatReport();
        AddHeader();
        AddFooter();
        Save();

        Status = "Успішно створено";
        Console.WriteLine($"Статус: {Status}");
    }

    protected abstract void FetchData();

    protected abstract void ProcessData();

    protected abstract void FormatReport();

    protected virtual void AddHeader()
    {
        Console.WriteLine("Додано стандартний заголовок звіту.");
    }

    protected virtual void AddFooter()
    {
        Console.WriteLine("Додано стандартний підвал звіту.");
    }

    protected abstract void Save();
}

class PDFReport : ReportGenerator
{
    public string PageSize { get; }

    public PDFReport(string reportName, string pageSize)
        : base(reportName)
    {
        PageSize = pageSize;
    }

    protected override void FetchData()
    {
        Console.WriteLine("Отримання даних для PDF-звіту.");
    }

    protected override void ProcessData()
    {
        Console.WriteLine("Обробка та аналіз даних для PDF.");
    }

    protected override void FormatReport()
    {
        Console.WriteLine($"Форматування PDF-звіту. Розмір сторінки: {PageSize}.");
    }

    protected override void AddHeader()
    {
        Console.WriteLine("Додано заголовок PDF-звіту з назвою документа.");
    }

    protected override void Save()
    {
        Console.WriteLine("Збереження звіту у форматі PDF.");
    }
}

class HTMLReport : ReportGenerator
{
    public string Theme { get; }

    public HTMLReport(string reportName, string theme)
        : base(reportName)
    {
        Theme = theme;
    }

    protected override void FetchData()
    {
        Console.WriteLine("Отримання даних для HTML-звіту.");
    }

    protected override void ProcessData()
    {
        Console.WriteLine("Обробка даних для відображення на вебсторінці.");
    }

    protected override void FormatReport()
    {
        Console.WriteLine($"Форматування HTML-звіту. Тема: {Theme}.");
    }

    protected override void AddFooter()
    {
        Console.WriteLine("Додано HTML-підвал із датою та контактами.");
    }

    protected override void Save()
    {
        Console.WriteLine("Збереження звіту у форматі HTML.");
    }
}

class ExcelReport : ReportGenerator
{
    public int WorksheetCount { get; }

    public ExcelReport(string reportName, int worksheetCount)
        : base(reportName)
    {
        WorksheetCount = worksheetCount;
    }

    protected override void FetchData()
    {
        Console.WriteLine("Отримання даних для Excel-звіту.");
    }

    protected override void ProcessData()
    {
        Console.WriteLine("Обчислення підсумків і статистичних показників.");
    }

    protected override void FormatReport()
    {
        Console.WriteLine($"Форматування таблиць Excel. Кількість аркушів: {WorksheetCount}.");
    }

    protected override void AddHeader()
    {
        Console.WriteLine("Додано заголовки стовпців Excel.");
    }

    protected override void AddFooter()
    {
        Console.WriteLine("Додано підсумковий рядок Excel.");
    }

    protected override void Save()
    {
        Console.WriteLine("Збереження звіту у форматі Excel.");
    }
}

class Program
{
    static void Main()
    {
        List<ReportGenerator> reports = new List<ReportGenerator>
        {
            new PDFReport("Фінансовий звіт", "A4"),
            new HTMLReport("Звіт про продажі", "Світла"),
            new ExcelReport("Статистика студентів", 3)
        };

        foreach (ReportGenerator report in reports)
        {
            report.Generate();
        }

        Console.WriteLine("\nУсі звіти успішно згенеровано.");
    }
}
