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
        string _path;

        public XmlParser(string filePath)
        {
            _path = filePath;
        }

        public IEnumerable<Customer> Parse()
        {
            using var fileStream = new FileStream(_path, FileMode.Open);
            var xmlSerializer = new XmlSerializer(typeof(CustomersList));
            var customers = xmlSerializer.Deserialize(fileStream) as CustomersList;
            return customers.Customers;
        }
    }
}