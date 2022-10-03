using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Otus.Teaching.Concurrency.Import.Core.Loaders;
using Otus.Teaching.Concurrency.Import.DataGenerator.Generators;


namespace Otus.Teaching.Concurrency.Import.Loader
{
    class Program
    {
        private static string _dataFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "customers.xml");
        
        static void Main(string[] args)
        {
            if (args != null && args.Length == 1)
            {
                _dataFilePath = args[0];
            }

            Console.WriteLine($"Loader started with process Id {Process.GetCurrentProcess().Id}...");

            GenerateCustomersDataFile();
            
            var loader = new FakeDataLoader();

            loader.LoadData();
        }

        static void GenerateCustomersDataFile()
        {
            Console.WriteLine("1 - Запуск генератора файла ПРОЦЕССОМ");
            Console.WriteLine("2 - Запуск генератора файла МЕТОДОМ");

            var input = Console.ReadLine();
            Console.WriteLine();

            if (input == "1")
            {
                var currentDirectory = Directory.GetCurrentDirectory()
                    .Replace("Otus.Teaching.Concurrency.Import.Loader", "Otus.Teaching.Concurrency.Import.DataGenerator.App");

                var path = Path.Combine(currentDirectory, "Otus.Teaching.Concurrency.Import.DataGenerator.App.exe");

                ProcessStartInfo procInfo = new ProcessStartInfo();
                procInfo.FileName = path;
                procInfo.ArgumentList.Add("customers");
                procInfo.ArgumentList.Add("1000");
                var process = Process.Start(procInfo);
                Console.WriteLine($"DataGenerator started with process Id {process.Id}...");
                process.WaitForExit();
            }
            else if (input == "2")
            {
                Console.WriteLine("Запуск генератора файла МЕТОДОМ.");
                var xmlGenerator = new XmlGenerator(_dataFilePath, 1000);
                xmlGenerator.Generate();
            }
        }
    }
}