using System;

public class NetworkStream : IDisposable
{
    private string _address;
    private bool _isStreamOpen;
    private bool _disposed;

    public string Address
    {
        get { return _address; }
        set { _address = value; }
    }

    public bool IsStreamOpen
    {
        get { return _isStreamOpen; }
    }

    public NetworkStream(string address)
    {
        _address = address;
        _isStreamOpen = true;
        _disposed = false;

        Console.WriteLine($"Мережевий потік відкрито: {_address}");
    }

    public void Send(string data)
    {
        if (_isStreamOpen)
        {
            Console.WriteLine($"Надсилання даних: {data}");
        }
        else
        {
            Console.WriteLine("Помилка: потік закритий.");
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                Console.WriteLine("Звільнення керованих ресурсів.");
            }

            if (_isStreamOpen)
            {
                Console.WriteLine($"Закриття мережевого потоку: {_address}");
                _isStreamOpen = false;
            }

            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~NetworkStream()
    {
        Console.WriteLine($"Деструктор: {_address}");
        Dispose(false);
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("===== Лабораторна робота №3 =====");
        Console.WriteLine("Варіант 3 — NetworkStream");
        Console.WriteLine();

        Console.WriteLine("----- Сценарій 1: using -----");

        using (NetworkStream stream1 = new NetworkStream("192.168.0.1"))
        {
            stream1.Send("Привіт, сервер!");
        }

        Console.WriteLine();

        Console.WriteLine("----- Сценарій 2: явний Dispose() -----");

        NetworkStream stream2 = new NetworkStream("10.0.0.1");
        stream2.Send("Тестове повідомлення");
        stream2.Dispose();

        Console.WriteLine();

        Console.WriteLine("----- Сценарій 3: GC.Collect() -----");

        CreateStreamWithoutDispose();

        GC.Collect();
        GC.WaitForPendingFinalizers();

        Console.WriteLine();
        Console.WriteLine("===== Роботу завершено =====");
    }

    static void CreateStreamWithoutDispose()
    {
        NetworkStream stream3 = new NetworkStream("172.16.0.1");
        stream3.Send("Дані без явного Dispose()");
    }
}