using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Otus.Teaching.Concurrency.Import.Common.Dto;
using Otus.Teaching.Concurrency.Import.Core.Parsers;
using Otus.Teaching.Concurrency.Import.Handler.Entities;

namespace Otus.Teaching.Concurrency.Import.DataAccess.Parsers
{
    public class XmlParser : IDataParser<IEnumerable<Customer>>
    {
        public IEnumerable<Customer> Parse(string filePath)
        {
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var xmlSerializer = new XmlSerializer(typeof(CustomersList));
            var customers = xmlSerializer.Deserialize(fileStream) as CustomersList;
            return customers.Customers;
        }
    }
}