using System;
using System.Collections.Generic;
using System.IO;

namespace Lab17v3;

public sealed class FileProcessor
{
    private int _attempts;

    public List<string> GetLines(string path)
    {
        _attempts++;
        Console.WriteLine($"  FileProcessor читає '{path}'.");
        if (_attempts <= 2)
            throw new IOException($"Тимчасова помилка читання (спроба {_attempts}).");

        return new List<string> { "order-1042", "order-1043", "order-1044" };
    }
}
