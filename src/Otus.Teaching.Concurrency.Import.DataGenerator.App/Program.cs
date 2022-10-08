using System;
using System.IO;
using Otus.Teaching.Concurrency.Import.Common;

namespace Otus.Teaching.Concurrency.Import.DataGenerator.App
{
    class Program
    {
        private static readonly string _dataFileDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private static string _dataFileName; 
        private static int _dataCount = 100;
        private static string _typeFile;

        static int Main(string[] args)
        {
            if (!TryValidateAndParseArgs(args))
                return 1;
            
            Console.WriteLine($"Generating data...");

            var generator = GeneratorFactory.GetGenerator(_typeFile, _dataFileName, _dataCount);
            generator.Generate();
            
            ConsoleHelper.WriteLine($"Generated data in [{_dataFileName}]\r\n");

            return 0;
        }

        private static bool TryValidateAndParseArgs(string[] args)
        {
            if (args != null && args.Length > 0)
            {
                _typeFile = args[0];
            }
            else
            {
                Console.WriteLine("File type is required");
                return false;
            }

            if (args.Length > 1)
            {
                _dataFileName = Path.Combine(_dataFileDirectory, $"{args[1]}.{_typeFile}");
            }
            else
            {
                Console.WriteLine("Data file name without extension is required");
                return false;
            }


            if (args.Length > 2)
            {
                if (!int.TryParse(args[2], out _dataCount))
                {
                    Console.WriteLine("Data must be integer");
                    return false;
                }
            }

            return true;
        }
    }
}