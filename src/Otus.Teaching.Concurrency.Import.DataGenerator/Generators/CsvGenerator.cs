using System.IO;
using Otus.Teaching.Concurrency.Import.Handler.Data;
using ServiceStack.Text;

namespace Otus.Teaching.Concurrency.Import.DataGenerator.Generators
{
    public class CsvGenerator : IDataGenerator
    {
        private readonly string _fileName;
        private readonly int _dataCount;

        public CsvGenerator(string fileName, int dataCount)
        {
            _fileName = fileName;
            _dataCount = dataCount;
        }
        
        public void Generate()
        {
            var customers = RandomCustomerGenerator.Generate(_dataCount);
            using var stream = File.Create(_fileName);
            CsvSerializer.SerializeToStream(customers, stream);
        }
    }
}