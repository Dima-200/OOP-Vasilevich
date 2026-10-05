using System;

namespace Lab6v3
{
    class Vehicle
    {
        private string _brand;
        private int _year;

        public string Brand
        {
            get { return _brand; }
            set { _brand = value; }
        }

        public int Year
        {
            get { return _year; }
            set { _year = value; }
        }

        public Vehicle(string brand, int year)
        {
            Brand = brand;
            Year = year;
        }

        public virtual void Drive()
        {
            Console.WriteLine($"Транспортний засіб {Brand} {Year} рухається.");
        }

        public string GetVehicleType()
        {
            return "Транспортний засіб";
        }
    }

    class Car : Vehicle
    {
        private int _numDoors;

        public int NumDoors
        {
            get { return _numDoors; }
            set { _numDoors = value; }
        }

        public Car(string brand, int year, int numDoors)
            : base(brand, year)
        {
            NumDoors = numDoors;
        }

        public override void Drive()
        {
            Console.WriteLine(
                $"Автомобіль {Brand} {Year} рухається по дорозі."
            );
        }

        public void OpenTrunk()
        {
            Console.WriteLine(
                $"Багажник автомобіля {Brand} відкрито."
            );
        }

        public new string GetVehicleType()
        {
            return "Автомобіль";
        }
    }

    class Motorcycle : Vehicle
    {
        private bool _hasSidecar;

        public bool HasSidecar
        {
            get { return _hasSidecar; }
            set { _hasSidecar = value; }
        }

        public Motorcycle(string brand, int year, bool hasSidecar)
            : base(brand, year)
        {
            HasSidecar = hasSidecar;
        }

        public override void Drive()
        {
            Console.WriteLine(
                $"Мотоцикл {Brand} {Year} рухається по дорозі."
            );
        }

        public void Wheelie()
        {
            Console.WriteLine(
                $"Мотоцикл {Brand} виконує їзду на задньому колесі."
            );
        }
    }

    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("===== Лабораторна робота №6 =====");
            Console.WriteLine("Тема: Наслідування, virtual, override, base, new");
            Console.WriteLine("Варіант №3: Vehicle → Car → Motorcycle");
            Console.WriteLine();

            Vehicle vehicle = new Vehicle("Toyota", 2020);

            Car car = new Car("BMW", 2022, 4);
            Motorcycle motorcycle = new Motorcycle("Honda", 2023, false);

            Console.WriteLine("===== 1. Базовий клас =====");
            vehicle.Drive();
            Console.WriteLine($"Тип: {vehicle.GetVehicleType()}");
            Console.WriteLine();

            Console.WriteLine("===== 2. Робота класу Car =====");
            car.Drive();
            car.OpenTrunk();
            Console.WriteLine($"Кількість дверей: {car.NumDoors}");
            Console.WriteLine($"Тип через Car: {car.GetVehicleType()}");
            Console.WriteLine();

            Console.WriteLine("===== 3. Робота класу Motorcycle =====");
            motorcycle.Drive();
            motorcycle.Wheelie();
            Console.WriteLine($"Є боковий причіп: {motorcycle.HasSidecar}");
            Console.WriteLine();

            Console.WriteLine("===== 4. Поліморфізм через virtual/override =====");

            Vehicle carAsVehicle = new Car("Audi", 2021, 4);
            Vehicle motorcycleAsVehicle = new Motorcycle("Yamaha", 2024, true);

            carAsVehicle.Drive();
            motorcycleAsVehicle.Drive();

            Console.WriteLine();

            Console.WriteLine("===== 5. Різниця між override та new =====");

            Console.WriteLine("Car посилання:");
            Console.WriteLine($"GetVehicleType(): {car.GetVehicleType()}");

            Console.WriteLine("Vehicle посилання на Car:");
            Console.WriteLine($"GetVehicleType(): {carAsVehicle.GetVehicleType()}");

            Console.WriteLine();

            Console.WriteLine("Пояснення:");
            Console.WriteLine(
                "Drive() використовує override, тому через посилання Vehicle " +
                "викликається реалізація Car."
            );

            Console.WriteLine(
                "GetVehicleType() використовує new, тому результат залежить " +
                "від типу посилання."
            );

            Console.WriteLine();

            Console.WriteLine("===== Завершення роботи =====");
        }
    }
}