using System;
using System.Threading;

namespace Lab17v3;

public static class RetryHelper
{
    public static T ExecuteWithRetry<T>(
        Func<T> operation,
        int retryCount = 3,
        TimeSpan initialDelay = default,
        Func<Exception, bool>? shouldRetry = null)
    {
        ArgumentNullException.ThrowIfNull(operation);
        if (retryCount < 0)
            throw new ArgumentOutOfRangeException(nameof(retryCount), "Кількість повторів не може бути від'ємною.");
        if (initialDelay < TimeSpan.Zero)
            throw new ArgumentOutOfRangeException(nameof(initialDelay), "Затримка не може бути від'ємною.");

        for (int attempt = 1; ; attempt++)
        {
            Console.WriteLine($"  Спроба {attempt}...");
            try
            {
                T result = operation();
                Console.WriteLine("  Операція успішна.");
                return result;
            }
            catch (Exception exception)
            {
                bool canRetry = attempt <= retryCount && (shouldRetry?.Invoke(exception) ?? true);
                Console.WriteLine($"  Помилка: {exception.GetType().Name}: {exception.Message}");

                if (!canRetry)
                {
                    Console.WriteLine(attempt > retryCount
                        ? "  Ліміт повторів вичерпано."
                        : "  Цей тип помилки не підлягає повтору.");
                    throw;
                }

                TimeSpan delay = TimeSpan.FromTicks(
                    (long)Math.Min(TimeSpan.MaxValue.Ticks, initialDelay.Ticks * Math.Pow(2, attempt - 1)));
                if (delay > TimeSpan.Zero)
                {
                    Console.WriteLine($"  Повтор через {delay.TotalMilliseconds:F0} мс.");
                    Thread.Sleep(delay);
                }
            }
        }
    }
}
