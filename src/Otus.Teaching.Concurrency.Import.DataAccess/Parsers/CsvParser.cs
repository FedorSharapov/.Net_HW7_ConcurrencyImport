using Otus.Teaching.Concurrency.Import.Core.Parsers;
using Otus.Teaching.Concurrency.Import.Handler.Entities;
using ServiceStack.Text;
using System.Collections.Generic;
using System.IO;

namespace Otus.Teaching.Concurrency.Import.DataAccess.Parsers
{
    public class CsvParser : IDataParser<IEnumerable<Customer>>
    {
        string _path;

        public CsvParser(string filePath)
        {
            _path = filePath;
        }

        public IEnumerable<Customer> Parse()
        {
            using var fileStream = new FileStream(_path, FileMode.Open);
            var customers = CsvSerializer.DeserializeFromStream<IEnumerable<Customer>>(fileStream);
            return customers;
        }
    }
}
