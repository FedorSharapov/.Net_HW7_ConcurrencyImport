﻿using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using Otus.Teaching.Concurrency.Import.Core.Loaders;
using Otus.Teaching.Concurrency.Import.DataAccess.EF;
using Otus.Teaching.Concurrency.Import.Handler.Entities;
using Otus.Teaching.Concurrency.Import.DataAccess.Repositories;
using Otus.Teaching.Concurrency.Import.Common;
using Otus.Teaching.Concurrency.Import.DataGenerator;
using Otus.Teaching.Concurrency.Import.DataAccess;
using Serilog;
using Serilog.Formatting.Compact;

namespace Otus.Teaching.Concurrency.Import.Loader
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args != null && args.Length == 1)
                AppSettings.DataFilePath = args[0];

            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(new CompactJsonFormatter(), "log.clef")
                .CreateLogger();

            var text = $"Loader started with process [Id {Process.GetCurrentProcess().Id}].\r\n";
            Log.Information(text);
            ConsoleHelper.WriteLine(text);

            // инициализация настроек приложения
            AppSettings.DisplayMessageError += AppSettings_DisplayMessageError;
            if (!AppSettings.Init())
                WaitingProgrammExit();

            // генерация данных клиентов в файл
            if (!GenerateCustomersDataFile())
                WaitingProgrammExit();

            // десериализация данных клиентов из файла
            var customers = DeserializationCustomersDataFile();
            if(!customers.Any())
                WaitingProgrammExit();

            // Очистка базы данных
            if (!ClearDataBase())
                WaitingProgrammExit();

            // Загрузка данных клиентов в базу данных
            LoadCustomersToDataBase(customers);

            WaitingProgrammExit();
        }

        private static void WaitingProgrammExit()
        {
            Log.CloseAndFlush();
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        /// <summary>
        /// Обработчик события отображния сообщений при инициализации приложения
        /// </summary>
        /// <param name="message">сообщение</param>
        private static void AppSettings_DisplayMessageError(string message)
        {
            ConsoleHelper.WriteLineError(message);
        }

        /// <summary>
        /// Генерация файла с вымышленными данными клиентов
        /// </summary>
        /// <returns>True - файл сгенерирован</returns>
        static bool GenerateCustomersDataFile()
        {
            try
            {
                if (AppSettings.IsGenerateDataByProcess)
                {
                    ProcessStartInfo procInfo = new ProcessStartInfo();
                    procInfo.FileName = AppSettings.GeneratorFullFileName;
                    procInfo.ArgumentList.Add(AppSettings.TypeFile);
                    procInfo.ArgumentList.Add("customers");
                    procInfo.ArgumentList.Add(AppSettings.NumData.ToString());

                    var process = Process.Start(procInfo);
                    ConsoleHelper.WriteLine($"Starting the [{AppSettings.TypeFile}] file generator [by process Id {process.Id}]...");
                    process.WaitForExit();

                    return process.ExitCode == 0;
                }
                else
                {
                    ConsoleHelper.WriteLine($"Starting the [{AppSettings.TypeFile}] file generator [by method]...");
                    ConsoleHelper.WriteLine($"Generating data...");

                    var generator = GeneratorFactory.GetGenerator(AppSettings.TypeFile);
                    generator.Generate(AppSettings.DataFilePath, AppSettings.NumData);
                    ConsoleHelper.WriteLine($"Generated data in [{AppSettings.DataFilePath}]\r\n");

                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Warning(ex.Message);
                ConsoleHelper.WriteLineError(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Десериализация данных из файла
        /// </summary>
        /// <returns>коллекция клиентов</returns>
        static IEnumerable<Customer> DeserializationCustomersDataFile()
        {
            try
            {
                var stopwatch = new Stopwatch();

                ConsoleHelper.WriteLine($"File deserialization...");
                stopwatch.Start();

                var customers = ParserFactory.GetParser(AppSettings.TypeFile).Parse(AppSettings.DataFilePath);

                stopwatch.Stop();
                ConsoleHelper.WriteLine($"Deserializated for [{stopwatch.ElapsedMilliseconds} ms]\r\n");

                if (!customers.Any())
                    ConsoleHelper.WriteLineError("File doesn't contain client data.");

                return customers;
            }
            catch (Exception ex)
            {
                Log.Logger.Warning(ex.Message);
                ConsoleHelper.WriteLineError(ex.Message);
                return new List<Customer>();
            }
        }

        /// <summary>
        /// Полная очистка базы данных
        /// </summary>
        private static bool ClearDataBase()
        {
            try
            {
                var stopwatch = new Stopwatch();

                using var dbContext = DatabaseContextFactory.CreateDbContext(AppSettings.TypeDb, AppSettings.DbConnectionString);
                var customerRepository = new CustomerRepository(dbContext, Log.Logger);

                Console.WriteLine("Clearing data base...");
                stopwatch.Start();

                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();

                stopwatch.Stop();
                ConsoleHelper.WriteLine($"Cleared for [{stopwatch.ElapsedMilliseconds} ms]\r\n");

                return true;
            }
            catch (Exception ex)
            {
                Log.Logger.Warning(ex.Message);
                ConsoleHelper.WriteLineError(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Загрузка клиентов в базу данных 
        /// </summary>
        /// <param name="customers">коллекция клиентов</param>
        static void LoadCustomersToDataBase(IEnumerable<Customer> customers)
        {
            try
            {
                var stopwatch = new Stopwatch();

                ConsoleHelper.WriteLine($"Loading [{AppSettings.NumData}] customers to [{AppSettings.TypeDb}] in [{AppSettings.NumThreads} threads]...");
                stopwatch.Start();

                if (AppSettings.NumThreads == 0)
                {
                    using var dbContext = DatabaseContextFactory.CreateDbContext(AppSettings.TypeDb, AppSettings.DbConnectionString);
                    dbContext.ChangeTracker.AutoDetectChangesEnabled = false;

                    var loader = new CustomersDataLoader(new CustomerRepository(dbContext, Log.Logger), customers);
                    loader.DisplayMessage += Loader_DisplayMessage;
                    loader.LoadData();
                }
                else
                {
                    var loader = new CustomersDataLoaderThreads(customers);
                    loader.DisplayMessage += Loader_DisplayMessage;
                    loader.LoadData();
                }

                stopwatch.Stop();
                ConsoleHelper.WriteLine($"Loaded for [{stopwatch.ElapsedMilliseconds} ms]\r\n");
            }
            catch (Exception ex)
            {
                Log.Logger.Warning(ex.Message);
                ConsoleHelper.WriteLineError(ex.Message);
            }
        }

        /// <summary>
        /// Обработчик события отображния сообщений из загрузчика данных в БД
        /// </summary>
        /// <param name="message">сообщение</param>
        private static void Loader_DisplayMessage(string message)
        {
            ConsoleHelper.WriteLine(message);
        }
    }
}