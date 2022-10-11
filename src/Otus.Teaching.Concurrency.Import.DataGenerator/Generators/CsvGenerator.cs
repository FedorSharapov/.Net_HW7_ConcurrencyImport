using System.IO;
using Otus.Teaching.Concurrency.Import.Handler.Data;
using ServiceStack.Text;

namespace Otus.Teaching.Concurrency.Import.DataGenerator.Generators
{
    public class CsvGenerator : IDataGenerator
    {        
        public void Generate(string fileName, int dataCount)
        {
            var customers = RandomCustomerGenerator.Generate(dataCount);
            using var stream = File.Create(fileName);
            CsvSerializer.SerializeToStream(customers, stream);
        }
    }
}