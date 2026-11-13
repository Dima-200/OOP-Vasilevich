using System;
using System.Net.Http;

namespace Lab17v3;

public sealed class NetworkClient
{
    private int _attempts;

    public string GetApiResponse(string endpoint)
    {
        _attempts++;
        Console.WriteLine($"  NetworkClient надсилає запит до {endpoint}.");
        if (_attempts <= 4)
            throw new HttpRequestException($"Тимчасова помилка API (спроба {_attempts}).");

        return "API відповів успішно: отримано 3 замовлення.";
    }
}
