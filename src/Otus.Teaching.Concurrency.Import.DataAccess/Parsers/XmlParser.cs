using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Otus.Teaching.Concurrency.Import.Core.Parsers;
using Otus.Teaching.Concurrency.Import.DataGenerator.Dto;
using Otus.Teaching.Concurrency.Import.Handler.Entities;

namespace Otus.Teaching.Concurrency.Import.DataAccess.Parsers
{
    public class XmlParser : IDataParser<List<Customer>>
    {
        string _path;

        public XmlParser(string filePath)
        {
            _path = filePath;
        }

        public List<Customer> Parse()
        {
            //Parse data
            using var reader = new FileStream(_path, FileMode.Open);

            var deserializer = new XmlSerializer(typeof(CustomersList));
            var customersList = (CustomersList)deserializer.Deserialize(reader);
            return customersList.Customers;

        }
    }
}