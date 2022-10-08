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
        
        static int Main(string[] args)
        {
            if (!TryValidateAndParseArgs(args))
                return 1;
            
            Console.WriteLine("Generating xml data...");

            var generator = GeneratorFactory.GetGenerator(_dataFileName, _dataCount);
            generator.Generate();
            
            ConsoleHelper.WriteLine($"Generated xml data in [{_dataFileName}]\r\n");

            return 0;
        }

        private static bool TryValidateAndParseArgs(string[] args)
        {
            if (args != null && args.Length > 0)
            {
                _dataFileName = Path.Combine(_dataFileDirectory, $"{args[0]}.xml");
            }
            else
            {
                Console.WriteLine("Data file name without extension is required");
                return false;
            }
            
            if (args.Length > 1)
            {
                if (!int.TryParse(args[1], out _dataCount))
                {
                    Console.WriteLine("Data must be integer");
                    return false;
                }
            }

            return true;
        }
    }
}