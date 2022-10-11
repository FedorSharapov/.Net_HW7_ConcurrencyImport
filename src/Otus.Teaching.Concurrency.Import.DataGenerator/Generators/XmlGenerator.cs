using System.IO;
using System.Xml.Serialization;
using Otus.Teaching.Concurrency.Import.Common.Dto;
using Otus.Teaching.Concurrency.Import.Handler.Data;

namespace Otus.Teaching.Concurrency.Import.DataGenerator.Generators
{
    public class XmlGenerator : IDataGenerator
    {
        public void Generate(string fileName, int dataCount)
        {
            var customers = RandomCustomerGenerator.Generate(dataCount);
            using var stream = File.Create(fileName);
            new XmlSerializer(typeof(CustomersList)).Serialize(stream, new CustomersList()
            {
                Customers = customers
            });
        }
    }
}